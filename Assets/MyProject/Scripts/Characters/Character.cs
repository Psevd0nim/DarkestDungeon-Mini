using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyProject
{
    public class Character : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Character> OnPressed;
        public event Action<Character> OnDeath;

        public bool IsAlive => Health > 0;
        public Color Color => _colorData;
        public bool IsFrontliner => OwnSquad.FrontlineCharacter == this;

        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Speed { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public List<Skill> SkillList { get; private set; } = new List<Skill>();
        public SquadController OwnSquad { get; private set; }
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

        public void Init(CharacterConfig config, List<Skill> skills)
        {
            CharacterData data = config.characterData;
            DisplayName = string.IsNullOrEmpty(config.displayName) ? config.characterType.ToString() : config.displayName;
            Portrait = config.portrait;
            Health = MaxHealth = data.health;
            Speed = data.speed;
            Attack = data.attack;
            Defense = data.defense;
            SkillList = skills;
            _healthBar.UpdateHealthBar(Health, MaxHealth);
        }

        public void AdditionalInit(SquadController squad, SquadSlot squadSlot, Color color, bool reversed = false)
        {
            OwnSquad = squad;
            _squadSlot = squadSlot;
            _colorData = color;
            if (reversed)
            {
                _healthBar.ReverseHealthBar();
                _visual.ReverseFeedback();
            }
        }

        public void PlayAttack(Action onImpact, Action onCompleted)
        {
            _visual.PlayAttack(onImpact, onCompleted);
        }

        public void ShowFeedback(string text, Color color)
        {
            _visual.ShowFeedback(text, color);
        }

        public void CancelAnimation()
        {
            _visual.CancelAnimation();
        }

        public void TakeDamage(int damage, Action onReactionCompleted = null)
        {
            Health = Mathf.Max(0, Health - damage);
            _healthBar.UpdateHealthBar(Health, MaxHealth);
            _visual.ShowFeedback("-" + damage, new Color(1, 0.45f, 0.35f));
            if (Health == 0)
            {
                Die();
                DeathRattle(onReactionCompleted);
            }
            else
            {
                _visual.PlayHurt(onReactionCompleted);
            }
        }

        public void Heal(int value)
        {
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

        private void Die()
        {
            SetCharacterActive(false);
            SetTargetActive(false);
            OnDeath?.Invoke(this);
        }

        private void DeathRattle(Action onCompleted)
        {
            _visual.PlayDeath(onCompleted);
        }
    }
}
