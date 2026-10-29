using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyProject
{
    public class Character : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Character> OnPressed;

        public int Health { get; private set; }
        public int Speed { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public List<Skill> SkillList { get; private set; } = new List<Skill>();

        public bool IsAlive => Health > 0;
        public ColorData ColorData => _colorData;
        public int SquadPosNum => _squadPosNum;
        public bool IsFrontliner => _ownSquad.FrontlineCharacter == this;

        [SerializeField] private SpriteRenderer _view;
        [SerializeField] private TextMeshPro _squadPosNumText;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Arrow _activeCharacterSign;
        [SerializeField] private TargetSign _targetSign;

        private ColorData _colorData;
        private int _squadPosNum;
        private float _maxHealth;
        private SquadController _ownSquad;

        private void Awake()
        {
            SetCharacterActive(false);
        }

        public void Init(CharacterData characterData, List<Skill> skillList)
        {
            Health = characterData.health;
            _maxHealth = characterData.health;
            Speed = characterData.speed;
            Attack = characterData.attack;
            Defense = characterData.defense;
            SkillList = skillList;
            _healthBar.UpdateHealthBar(Health, _maxHealth);
        }

        public void AdditionalInit(SquadController squadController, int squadPosNum, ColorData colorData, bool needReverse = false)
        {
            _ownSquad = squadController;
            _squadPosNum = squadPosNum;
            _squadPosNumText.text = squadPosNum.ToString();

            _colorData = colorData;
            _view.color = colorData.color;

            if (needReverse)
            {
                Vector3 scale = _squadPosNumText.transform.localScale;
                scale.x = -scale.x;
                _squadPosNumText.transform.localScale = scale;

                _healthBar.ReverseHealthBar();
            }
        }

        public void TakeDamage(int damageValue)
        {
            Health -= damageValue;
            if (Health < 0)
            {
                Health = 0;
            }
            _healthBar.UpdateHealthBar(Health, _maxHealth);
        }

        public void SetCharacterActive(bool isActive)
        {
            _activeCharacterSign.gameObject.SetActive(isActive);
        }

        public void SetTargetActive(bool isActive)
        {
            _targetSign.SetSignActive(isActive);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPressed?.Invoke(this);
        }
    }
}
