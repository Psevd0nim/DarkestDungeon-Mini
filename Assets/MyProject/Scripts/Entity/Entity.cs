using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyProject
{
    public class Entity : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Entity> OnPressed;

        public float Health { get; private set; }
        public float Speed { get; private set; }
        public float Attack { get; private set; }
        public float Defense { get; private set; }
        public List<Skill> SkillList { get; private set; } = new List<Skill>();

        public bool IsAlive => Health > 0;
        public ColorData ColorData => _colorData;
        public int SquadPosNum => _squadPosNum;

        [SerializeField] private SpriteRenderer _view;
        [SerializeField] private TextMeshPro _squadPosNumText;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Arrow _activeCharacterSign;

        private ColorData _colorData;
        private int _squadPosNum;
        private float _maxHealth;

        private void Awake()
        {
            SetCharacterActive(false);
        }

        public void Init(EntityData entityData, List<Skill> skillList)
        {
            Health = entityData.health;
            _maxHealth = entityData.health;
            Speed = entityData.speed;
            Attack = entityData.attack;
            Defense = entityData.defense;
            SkillList = skillList;
            _healthBar.UpdateHealthBar(Health, _maxHealth);
        }

        public void AdditionalInit(int squadPosNum, ColorData colorData, bool needReverse = false)
        {
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

        public void TakeDamage(float damageValue)
        {
            Health -= damageValue;
            if (Health < 0)
            {
                Health = 0;
            }
            _healthBar.UpdateHealthBar(Health, _maxHealth);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPressed?.Invoke(this);
        }

        public void SetCharacterActive(bool isActive)
        {
            _activeCharacterSign.gameObject.SetActive(isActive);
        }
    }
}
