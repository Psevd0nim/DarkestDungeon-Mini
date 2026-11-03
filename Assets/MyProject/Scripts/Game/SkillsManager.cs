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

        private CharactersManager _charactersManager;
        private SkillBoard_UI _skillBoard_UI;
        private Skill _selectedSkill;
        private ITargetedSkill _targetedSkill;
        private List<Character> _targets = new List<Character>();
        private Coroutine _aiRoutine;


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
                        melee.Prepare(_charactersManager.GetOpponentSquad(skillsOwner));
                        break;
                    case RangedAttackSkill ranged:
                        ranged.Prepare(_charactersManager.GetOpponentSquad(skillsOwner));
                        break;
                    case HealSkill heal:
                        heal.Prepare(skillsOwner.OwnSquad);
                        break;
                }
            }
        }

        private void AfterSkillPressed(Skill_UI button)
        {
            if (_selectedSkill == button.Skill)
            {
                CancelSelection();
            }
            else
            {
                _selectedSkill = button.Skill;
                button.SetSelectedActive(true);
                switch (_selectedSkill)
                {
                    case IImmediateSkill immediate:
                        _skillBoard_UI.SetInteractable(false);
                        immediate.Execute(AfterSkillUsed);
                        break;
                    case ITargetedSkill targeted:
                        _skillBoard_UI.SetInteractable(false);
                        targeted.Execute(AfterSkillUsed);
                        break;
                }
            }
        }

        private void AfterSkillUsed()
        {
            OnSkillUsed?.Invoke();
        }

        private void CancelSelection()
        {
            //throw new NotImplementedException();
        }
    }
}
