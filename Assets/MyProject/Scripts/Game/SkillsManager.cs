using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillsManager : MonoBehaviour
    {
        [SerializeField] private SkipTurnSkillConfig _requiredSkillConfig;

        private Game_UI_Manager _game_UI_Manager;
        private CharactersManager _charactersManager;
        private List<Skill> _targetSkills;
        private SkillsBoard_UI _skillsBoard_UI;

        private Skill _currentActiveSkill;
        private ITargetedSkill _targetedSkill;
        private Action _callbackForEndTurn;
        private List<Character> _targetList = new List<Character>();

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

        public List<Skill> GetSkillListBySkillConfigs(List<SkillConfig> skillConfigList, Character character)
        {
            List<Skill> skills = new List<Skill>();
            skills.Add(_requiredSkillConfig.CreateSkill());
            foreach(var skillConfig in skillConfigList)
            {
                Skill skill = skillConfig.CreateSkill();
                skill.SetCharacter(character);
                skills.Add(skill);
            }
            return skills;
        }

        private void ShowSkills(List<Skill> skills)
        {
            _targetSkills = skills;
            _skillsBoard_UI.ShowSkills(_targetSkills);
            foreach(var skill in _targetSkills)
            {
                switch (skill)
                {
                    case MeleeAttackSkill meleeAttackSkill:
                        meleeAttackSkill.Prepare(_charactersManager.EnemySquadController);
                        break;
                    default: 
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
                    if (_currentActiveSkill != null)
                        _currentActiveSkill = null;
                    immediateSkill.Execute(AfterSkillUsed);
                    break;
                case ITargetedSkill targetedSkill:
                    if(_currentActiveSkill != null)
                    {
                        if (_currentActiveSkill == targetedSkill)
                            return;
                        else
                            _currentActiveSkill.Skill_UI.SetSelectedActive(false);
                    }
                    _currentActiveSkill = skill;
                    _targetedSkill = targetedSkill;
                    _currentActiveSkill.Skill_UI.SetSelectedActive(true);
                    _targetList = targetedSkill.GetAvailableTargets();
                    ShowSkillTargets(_targetList);
                    _charactersManager.OnCharacterPressed += CheckForTargetAccept;
                    break;
            }
        }

        private void ShowSkillTargets(List<Character> targets)
        {
            foreach (Character character in targets)
            {
                character.SetTargetActive(true);
            }
        }

        private void CheckForTargetAccept(Character potentionalTarget)
        {
            if (!_targetList.Contains(potentionalTarget))
                return;

            _charactersManager.OnCharacterPressed -= CheckForTargetAccept;
            foreach (Character character in _targetList)
                character.SetTargetActive(false);
            _targetedSkill.Execute(potentionalTarget, _callbackForEndTurn);
        }

        private void AfterSkillUsed()
        {
            _skillsBoard_UI.ClearBoard();
            _callbackForEndTurn?.Invoke();
        }
    }
}
