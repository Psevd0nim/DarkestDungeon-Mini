using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillExecutionManager : MonoBehaviour
    {
        public bool IsExecuting { get; private set; }

        public event Action OnImpact;

        private enum ActionPhase
        {
            Idle,
            BeforeImpact,
            Recovery
        }

        private ActionPhase _phase;
        private float _remaining;
        private float _recoveryDuration;
        private Action _impact;
        private Action _completed;

        public static float HitChance(int attack, int defense, int penalty)
        {
            return Mathf.Clamp((75f + attack - defense - penalty) / 100f, 0.05f, 0.95f);
        }

        public void Attack(Character owner, Character target, int damage, int penalty, SkillConfig config, Action completed)
        {
            BeginAction(owner, config, () =>
            {
                bool hit = UnityEngine.Random.value < HitChance(owner.Attack, target.Defense, penalty);
                if (hit)
                {
                    target.TakeDamage(damage);
                }
                else
                {
                    target.Visual.ShowFeedback("MISS", new Color(1, 0.85f, 0.45f));
                }

            }, target.Visual.ReactionDuration, completed);
        }

        public void Heal(Character owner, List<Character> targets, int value, SkillConfig config, Action completed)
        {
            BeginAction(owner, config, () =>
            {
                foreach (var target in targets)
                {
                    target.Heal(value);
                }

            }, 0.7f, completed);
        }

        public void Skip(Character owner, SkillConfig config, Action completed)
        {
            BeginAction(owner, config, () =>
            {
                owner.Visual.ShowFeedback("WAIT", new Color(0.75f, 0.85f, 1));
            }, 0.3f, completed);
        }

        private void BeginAction(Character owner, SkillConfig config, Action impact, float reactionDuration, Action completed)
        {
            IsExecuting = true;
            _impact = impact;
            _completed = completed;
            owner.Visual.PlayAction(config);
            float duration = owner.Visual.ActionDuration(config);
            _remaining = duration * config.impactTime;
            _recoveryDuration = Mathf.Max(duration - _remaining, reactionDuration) + 0.12f;
            _phase = ActionPhase.BeforeImpact;
        }

        private void Update()
        {
            if (_phase == ActionPhase.Idle)
            {
                return;
            }

            _remaining -= Time.deltaTime;
            if (_remaining > 0)
            {
                return;
            }

            switch (_phase)
            {
                case ActionPhase.BeforeImpact:
                    _impact();
                    OnImpact?.Invoke();
                    _remaining = _recoveryDuration;
                    _phase = ActionPhase.Recovery;
                    break;
                case ActionPhase.Recovery:
                    var completed = _completed;
                    Cancel();
                    completed();
                    break;
            }
        }

        public void Cancel()
        {
            _phase = ActionPhase.Idle;
            _impact = null;
            _completed = null;
            IsExecuting = false;
        }
    }
}
