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
        [SerializeField] private Image _selectedOutline;
        [SerializeField] private TextMeshProUGUI _skillTipText;
        [SerializeField] private GameObject _tip;
        [SerializeField] private CanvasGroup _viewCanvasGroup;
        [SerializeField] private TextMeshProUGUI _nameLabel;

        private Skill _skill;

        private void Awake()
        {
            _selectedOutline.gameObject.SetActive(false);
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
            _viewCanvasGroup.alpha = active ? 1 : 0.8f;
            _viewCanvasGroup.interactable = active;
        }

        public void SetSelectedActive(bool active)
        {
            _selectedOutline.gameObject.SetActive(active);
        }

        public void ShowSkillTip(bool show)
        {
            _tip.SetActive(show);
        }

        public void SkillButtonPressed()
        {
            OnSkillPressed?.Invoke(this);
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
