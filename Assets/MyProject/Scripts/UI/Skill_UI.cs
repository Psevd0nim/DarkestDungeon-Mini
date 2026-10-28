using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyProject
{
    public class Skill_UI : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Skill_UI> OnSkillPressed;

        public Skill Skill => _skill;

        [SerializeField] private Image _skillView;
        [SerializeField] private Outline _selectedOutline;
        [SerializeField] private TextMeshProUGUI _skillTipText;
        [SerializeField] private GameObject _tip;

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
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SkillButtonPressed();
        }

        public void SetSelectedActive(bool isActive)
        {
            _selectedOutline.enabled = isActive;
        }

        public void ShowSkillTip(bool show)
        {
            _tip.SetActive(show);
        }

        private void SkillButtonPressed()
        {
            OnSkillPressed?.Invoke(this);
        }
    }
}
