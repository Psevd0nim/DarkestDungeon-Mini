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
            foreach (var skill in skills)
            {
                var button = _factory.CreateSkill_UI();
                button.transform.SetParent(transform, false);
                button.Init(skill);
                button.OnSkillPressed += AfterSkillPressed;
                _buttons.Add(button);
            }
        }

        public void SetInteractable(bool active)
        {
            foreach (var b in _buttons)
            {
                b.SetInteractable(active && b.Skill.CanUse);
            }
        }

        public void ClearSelection()
        {
            foreach (var b in _buttons)
            {
                b.SetSelectedActive(false);
            }
        }

        public void ClearBoard()
        {
            foreach (var button in _buttons)
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
