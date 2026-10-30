using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
    public class SkillsManager : MonoBehaviour
    {
        [SerializeField] private SkipTurnSkillConfig _requiredSkillConfig;

        private Game_UI_Manager _game_UI_Manager;
        private CharactersManager _charactersManager;
        private SkillsBoard_UI _skillsBoard_UI;

        private Skill _currentActiveSkill;
        private ITargetedSkill _targetedSkill;
        private List<Character> _targetCharacterList = new List<Character>();
        private Action _callbackForEndTurn;

        public void Init(Game_UI_Manager game_UI_Manager, CharactersManager charactersManager)
        {
            _game_UI_Manager = game_UI_Manager;
            _charactersManager = charactersManager;
            _skillsBoard_UI = _game_UI_Manager.SkillsBoard_UI;
            _skillsBoard_UI.OnSkillPressed += AfterSkillPressed;
        }

        public void BeginPlayerTurn(Character currentCharacter, Action callback)
        {
            ShowSkills(currentCharacter.SkillList);
            _callbackForEndTurn = callback;
        }

        public void Begin_AI_Turn(Character character, Action callback)
        {
            StartCoroutine(AI_TurnCoroutine(character, callback));
        }

        private IEnumerator AI_TurnCoroutine(Character character, Action callback)
        {
            _callbackForEndTurn = callback;
            List<Skill> skillCanUseList = character.SkillList.Where(x => x.CanUse).ToList();
            Skill skillForUse = skillCanUseList[Random.Range(0, skillCanUseList.Count)];
            switch (skillForUse)
            {
                case IImmediateSkill immediateSkill:
                    yield return new WaitForSeconds(1f);
                    immediateSkill.Execute(AfterSkillUsed);
                    break;
                case ITargetedSkill targetedSkill:
                    {
                        switch (targetedSkill)
                        {
                            case MeleeAttackSkill meleeAttackSkill:
                                meleeAttackSkill.Prepare(_charactersManager.PlayerSquadController);
                                break;
                            case RangedAttackSkill rangedAttackSkill:
                                rangedAttackSkill.Prepare(_charactersManager.PlayerSquadController);
                                break;
                            case HealSkill healSkill:
                                healSkill.Prepare(_charactersManager.EnemySquadController);
                                break;
                        }
                        List<Character> targetCharacterList = targetedSkill.GetAvailableTargets();
                        ShowSkillTargets(targetCharacterList, true);
                        yield return new WaitForSeconds(2f);
                        foreach (Character target in targetCharacterList)
                            target.SetTargetActive(false);
                        Character rndCharacter = targetCharacterList[Random.Range(0, targetCharacterList.Count)];
                        targetedSkill.Execute(rndCharacter, AfterSkillUsed);
                    }
                    break;
            }
        }

        public List<Skill> GetSkillListBySkillConfigs(List<SkillConfig> skillConfigList, Character character)
        {
            List<Skill> skills = new List<Skill>();
            skills.Add(_requiredSkillConfig.CreateSkill());
            foreach (var skillConfig in skillConfigList)
            {
                Skill skill = skillConfig.CreateSkill();
                skill.SetCharacter(character);
                skills.Add(skill);
            }
            return skills;
        }

        private void ShowSkills(List<Skill> skills)
        {
            _skillsBoard_UI.ShowSkills(skills);
            foreach (var skill in skills)
            {
                switch (skill)
                {
                    case MeleeAttackSkill meleeAttackSkill:
                        meleeAttackSkill.Prepare(_charactersManager.EnemySquadController);
                        break;
                    case RangedAttackSkill rangedAttackSkill:
                        rangedAttackSkill.Prepare(_charactersManager.EnemySquadController);
                        break;
                    case HealSkill healSkill:
                        healSkill.Prepare(_charactersManager.PlayerSquadController);
                        break;
                }
            }
        }

        private void AfterSkillPressed(Skill_UI skill_UI)
        {
            BeginUseSkill(skill_UI.Skill);
        }

        private void BeginUseSkill(Skill skill)
        {
            switch (skill)
            {
                case IImmediateSkill immediateSkill:
                    immediateSkill.Execute(AfterSkillUsed);
                    break;
                case ITargetedSkill targetedSkill:
                    if (_targetedSkill != null && _targetedSkill == targetedSkill)
                        return;
                    SetOrChangeTargetSkill(skill, targetedSkill);
                    break;
            }
        }

        private void ShowSkillTargets(List<Character> targets, bool show)
        {
            foreach (Character character in targets)
            {
                character.SetTargetActive(show);
            }
        }

        private void SetOrChangeTargetSkill(Skill skill, ITargetedSkill targetedSkill)
        {
            _charactersManager.OnCharacterPressed -= CheckForTargetAccept;
            _currentActiveSkill?.Skill_UI.SetSelectedActive(false);
            _currentActiveSkill = skill;
            _currentActiveSkill.Skill_UI.SetSelectedActive(true);

            ShowSkillTargets(_targetCharacterList, false);
            _targetedSkill = targetedSkill;
            _targetCharacterList = targetedSkill.GetAvailableTargets();
            ShowSkillTargets(_targetCharacterList, true);

            _charactersManager.OnCharacterPressed += CheckForTargetAccept;
        }

        private void CheckForTargetAccept(Character potentionalTarget)
        {
            if (!_targetCharacterList.Contains(potentionalTarget))
                return;

            _charactersManager.OnCharacterPressed -= CheckForTargetAccept;
            foreach (Character character in _targetCharacterList)
                character.SetTargetActive(false);
            _targetedSkill.Execute(potentionalTarget, AfterSkillUsed);
        }

        private void AfterSkillUsed()
        {
            _currentActiveSkill = null;
            _targetedSkill = null;
            _skillsBoard_UI.ClearBoard();
            _callbackForEndTurn?.Invoke();
        }
    }
}
