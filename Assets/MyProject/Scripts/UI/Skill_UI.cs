using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MyProject
{
    public class Skill_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Skill_UI> OnSkillPressed;
        public Skill Skill => _skill;

        [SerializeField] private Image _skillView;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private TextMeshProUGUI _skillTipText;
        [SerializeField] private GameObject _tip;
        [SerializeField] private CanvasGroup _viewCanvasGroup;
        [SerializeField] private TextMeshProUGUI _nameLabel;
        private Skill _skill;
        private bool _interactable;

        private void Awake()
        {
            _selectedOutline.enabled = false;
            _tip.SetActive(false);
        }

        public void Init(Skill skill)
        {
            _skill = skill;
            _skillView.sprite = skill.Config.sprite;
            _skillTipText.text = skill.Config.GetDescription();
            _nameLabel.text = skill.Config.displayName;
            SetInteractable(skill.CanUse);
        }

        public void SetInteractable(bool active)
        {
            _interactable = active;
            _viewCanvasGroup.alpha = active ? 1 : 0.4f;
        }

        public void SetSelectedActive(bool active)
        {
            _selectedOutline.enabled = active;
        }

        public void ShowSkillTip(bool show)
        {
            _tip.SetActive(show);
        }

        public void SkillButtonPressed()
        {
            if (_interactable)
            {
                OnSkillPressed?.Invoke(this);
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            ShowSkillTip(true);
        }

        public void OnPointerExit(PointerEventData data)
        {
            ShowSkillTip(false);
        }
    }
}
