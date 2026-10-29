using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyProject
{
    public class Skill_UI : MonoBehaviour
    {
        public event Action<Skill_UI> OnSkillPressed;

        public Skill Skill => _skill;

        [SerializeField] private Image _skillView;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private TextMeshProUGUI _skillTipText;
        [SerializeField] private GameObject _tip;
        [SerializeField] private CanvasGroup _viewCanvasGroup;

        private Skill _skill;

        private void Awake()
        {
            _selectedOutline.enabled = false;
            ShowSkillTip(false);
        }

        public void Init(Skill skill)
        {
            _skill = skill;
            _skillView.sprite = skill.Config.sprite;
            _skillTipText.text = skill.Config.tip;
            SetInteractableStatus(skill.CanUse);
        }

        public void SetSelectedActive(bool isActive)
        {
            _selectedOutline.enabled = isActive;
        }

        public void ShowSkillTip(bool show)
        {
            _tip.SetActive(show);
        }

        private void SetInteractableStatus(bool status)
        {
            _viewCanvasGroup.interactable = status;
            _viewCanvasGroup.alpha = status ? 1 : 0.7f;
        }

        public void SkillButtonPressed()
        {
            if(_skill.CanUse)
                OnSkillPressed?.Invoke(this);
        }
    }
}
