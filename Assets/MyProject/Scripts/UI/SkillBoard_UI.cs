using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillBoard_UI : MonoBehaviour
    {
        public event Action<Skill_UI> OnSkillPressed;

        private Factory _factory;
        private List<Skill_UI> _buttons = new List<Skill_UI>();

        public void Init(Factory factory)
        {
            _factory = factory;
        }

        public void ShowSkills(List<Skill> skills)
        {
            ClearBoard();
            foreach (Skill skill in skills)
            {
                Skill_UI button = _factory.CreateSkill_UI();
                button.transform.SetParent(transform, false);
                button.Init(skill);
                button.OnSkillPressed += AfterSkillPressed;
                _buttons.Add(button);
            }
        }

        public Skill_UI GetSkill_UI(Skill skill)
        {
            return _buttons.Find(button => button.Skill == skill);
        }

        public void SetInteractable(bool active)
        {
            foreach (Skill_UI button in _buttons)
            {
                button.SetInteractable(active && button.Skill.CanUse);
            }
        }

        public void ClearBoard()
        {
            foreach (Skill_UI button in _buttons)
            {
                button.OnSkillPressed -= AfterSkillPressed;
                button.gameObject.SetActive(false);
                Destroy(button.gameObject);
            }
            _buttons.Clear();
        }

        private void AfterSkillPressed(Skill_UI button)
        {
            OnSkillPressed?.Invoke(button);
        }
    }
}
