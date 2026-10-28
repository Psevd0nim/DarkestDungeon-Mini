using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillsBoard_UI : MonoBehaviour
    {
        public event Action<Skill_UI> OnSkillPressed;

        private Factory _factory;
        private List<Skill_UI> _targetSkills = new List<Skill_UI>();

        public void Init(Factory factory)
        {
            _factory = factory;
        }

        public void ShowSkills(List<Skill> skills)
        {
            TryClearSkills();
            foreach (var skill in skills)
            {
                Skill_UI skill_UI = _factory.CreateSkill_UI();
                skill_UI.transform.SetParent(transform);
                skill_UI.Init(skill);
                _targetSkills.Add(skill_UI);
                skill_UI.OnSkillPressed += AfterSkillPressed;
            }
        }

        public void TryClearSkills()
        {
            if (_targetSkills.Count > 0)
            {
                foreach (var skill in _targetSkills)
                {
                    Destroy(skill.gameObject);
                }
                _targetSkills.Clear();
            }
        }

        private void AfterSkillPressed(Skill_UI skill_UI)
        {
            OnSkillPressed?.Invoke(skill_UI);
        }
    }
}