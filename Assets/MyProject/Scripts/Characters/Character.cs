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
        public event Action<Character> OnDeath;

        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Speed { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public List<Skill> SkillList { get; private set; } = new List<Skill>();
        public bool IsAlive => Health > 0;
        public Color Color => _colorData;
        public int SquadPosNum { get; private set; }
        public SquadController OwnSquad { get; private set; }
        public bool IsFrontliner => OwnSquad.FrontlineCharacter == this;
        public CharacterView Visual => _visual;
        public string DisplayName { get; private set; }
        public Sprite Portrait { get; private set; }

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Arrow _activeCharacterSign;
        [SerializeField] private CharacterView _visual;

        private Color _colorData;
        private SquadSlot _squadSlot;

        private void Awake()
        {
            SetCharacterActive(false);
        }

        public void Init(CharacterData data, List<Skill> skills)
        {
            Health = MaxHealth = data.health;
            Speed = data.speed;
            Attack = data.attack;
            Defense = data.defense;
            SkillList = skills;
            _healthBar.UpdateHealthBar(Health, MaxHealth);
        }

        public void SetIdentity(CharacterConfig config)
        {
            DisplayName = string.IsNullOrEmpty(config.displayName) ? config.characterType.ToString() : config.displayName;
            Portrait = config.portrait;
        }

        public void AdditionalInit(SquadController squad, int slot, Color color, bool reversed = false)
        {
            OwnSquad = squad;
            SquadPosNum = slot;
            _colorData = color;
            if (reversed)
            {
                _healthBar.ReverseHealthBar();
                _visual.ReverseFeedback();
            }
        }

        public void SetSquadSlot(SquadSlot squadSlot)
        {
            _squadSlot = squadSlot;
        }

        public void TakeDamage(int damage)
        {
            if (!IsAlive)
            {
                return;
            }

            Health = Mathf.Max(0, Health - damage);
            _healthBar.UpdateHealthBar(Health, MaxHealth);
            _visual.ShowFeedback("-" + damage, new Color(1, 0.45f, 0.35f));
            if (Health == 0)
            {
                Die();
            }
            else
            {
                _visual.PlayHurt();
            }
        }

        private void Die()
        {
            SetCharacterActive(false);
            SetTargetActive(false);
            OnDeath?.Invoke(this);
        }

        private void DeathRattle()
        {
            _visual.PlayDeath();
        }

        public void Heal(int value)
        {
            if (!IsAlive)
            {
                return;
            }

            int restored = Mathf.Min(value, MaxHealth - Health);
            Health += restored;
            _healthBar.UpdateHealthBar(Health, MaxHealth);
            _visual.ShowFeedback("+" + restored, new Color(0.4f, 1, 0.65f));
        }

        public void SetCharacterActive(bool active)
        {
            _activeCharacterSign.gameObject.SetActive(active);
        }

        public void SetTargetActive(bool active)
        {
            _squadSlot.SetTargetActive(active);
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Left && IsAlive)
            {
                OnPressed?.Invoke(this);
            }
        }
    }
}
