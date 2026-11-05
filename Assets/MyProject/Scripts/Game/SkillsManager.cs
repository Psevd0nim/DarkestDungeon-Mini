using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillsManager : MonoBehaviour, ICoroutineRunner
    {
        public event Action OnSkillUsed;

        public bool IsPaused { get; set; }

        [SerializeField] private SkipTurnSkillConfig _requiredSkillConfig;
        [SerializeField, Min(0f)] private float _attackWeightMultiplier = 1f;
        [SerializeField, Min(0f)] private float _defenseWeightMultiplier = 1f;

        private CharactersManager _charactersManager;
        private SkillBoard_UI _skillBoard_UI;
        private Skill _selectedSkill;
        private Skill_UI _selectedSkill_UI;

        public void Init(Game_UI_Manager gameUIManager, CharactersManager charactersManager)
        {
            _charactersManager = charactersManager;
            _skillBoard_UI = gameUIManager.SkillBoard_UI;
            _skillBoard_UI.OnSkillPressed += AfterSkillPressed;
        }

        public void ShowSkills(List<Skill> skills, Character skillsOwner)
        {
            PrepareSkills(skills, skillsOwner);
            _skillBoard_UI.ShowSkills(skills);
        }

        public void CancelProcess()
        {
            if (_selectedSkill != null)
            {
                _selectedSkill.StopExecute();
                _selectedSkill_UI.SetSelectedActive(false);
                _selectedSkill = null;
                _selectedSkill_UI = null;
            }
            _skillBoard_UI.ClearBoard();
        }

        public bool RollHit(int attack, int defense)
        {
            float attackWeight = attack * _attackWeightMultiplier;
            float defenseWeight = defense * _defenseWeightMultiplier;

            if (defenseWeight == 0f)
            {
                return true;
            }

            if (attackWeight == 0f)
            {
                return false;
            }

            float totalWeight = attackWeight + defenseWeight;
            float selectedTicket = UnityEngine.Random.Range(0f, totalWeight);

            return selectedTicket < attackWeight;
        }

        public List<Skill> GetSkillListBySkillConfigs(List<SkillConfig> configs, Character owner)
        {
            List<Skill> skills = new List<Skill>
            {
                _requiredSkillConfig.CreateSkill()
            };
            foreach (SkillConfig config in configs)
            {
                skills.Add(config.CreateSkill());
            }

            foreach (Skill skill in skills)
            {
                skill.Init(owner, this);
            }

            return skills;
        }

        private void PrepareSkills(List<Skill> skills, Character skillsOwner)
        {
            foreach (Skill skill in skills)
            {
                switch (skill)
                {
                    case MeleeAttackSkill melee:
                        melee.Prepare(_charactersManager.GetOpponentSquad(skillsOwner), _skillBoard_UI, RollHit);
                        break;
                    case RangedAttackSkill ranged:
                        ranged.Prepare(_charactersManager.GetOpponentSquad(skillsOwner), _skillBoard_UI, RollHit);
                        break;
                    case HealSkill heal:
                        heal.Prepare(skillsOwner.OwnSquad, _skillBoard_UI);
                        break;
                    case SkipTurnSkill skip:
                        skip.Prepare(_skillBoard_UI);
                        break;
                }
            }
        }

        private void AfterSkillPressed(Skill_UI button)
        {
            if (_selectedSkill != null)
            {
                if (_selectedSkill == button.Skill)
                    return;

                _selectedSkill_UI.SetSelectedActive(false);
                _selectedSkill.StopExecute();
            }

            _selectedSkill = button.Skill;
            _selectedSkill_UI = button;
            _selectedSkill_UI.SetSelectedActive(true);
            _selectedSkill.Execute(AfterSkillUsed);
        }

        private void AfterSkillUsed()
        {
            _selectedSkill = null;
            _selectedSkill_UI = null;
            OnSkillUsed?.Invoke();
        }
    }
}
