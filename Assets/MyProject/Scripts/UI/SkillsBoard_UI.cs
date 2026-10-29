using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillsBoard_UI : MonoBehaviour
    {
        public event Action<Skill_UI> OnSkillPressed;

        private Factory _factory;
        private List<Skill_UI> _targetSkillsUI = new List<Skill_UI>();

        public void Init(Factory factory)
        {
            _factory = factory;
        }

        public void ShowSkills(List<Skill> skills)
        {
            ClearBoard();
            foreach (var skill in skills)
                CreateSkill_UI(skill);
        }

        private void CreateSkill_UI(Skill skill)
        {
            Skill_UI skill_UI = _factory.CreateSkill_UI();
            skill_UI.transform.SetParent(transform);
            skill_UI.Init(skill);
            _targetSkillsUI.Add(skill_UI);
            skill_UI.OnSkillPressed += AfterSkillPressed;
            skill.SetSkillUI(skill_UI);
        }

        public void ClearBoard()
        {
            if (_targetSkillsUI.Count > 0)
            {
                foreach (var skillUI in _targetSkillsUI)
                {
                    skillUI.Skill.SetSkillUI(null);
                    Destroy(skillUI.gameObject);
                }
                _targetSkillsUI.Clear();
            }
        }

        private void AfterSkillPressed(Skill_UI skill_UI)
        {
            OnSkillPressed?.Invoke(skill_UI);
        }

        
    }
}