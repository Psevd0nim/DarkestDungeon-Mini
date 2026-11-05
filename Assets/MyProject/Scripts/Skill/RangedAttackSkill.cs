using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
    [Serializable]
    public class RangedAttackSkill : Skill
    {
        private int _minDamage;
        private int _maxDamage;
        private int _frontlineAttackPenalty;
        private SquadController _enemySquad;
        private SkillBoard_UI _skillBoard_UI;
        private bool _isTargetSelected;
        private Character _target;
        private List<Character> _targets;
        private Func<int, int, bool> _rollHit;

        public RangedAttackSkill(int minDamage, int maxDamage, RangedSkillConfig config) : base(config)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _frontlineAttackPenalty = config.frontlineAttackPenalty;
        }

        public void Prepare(SquadController enemySquad, SkillBoard_UI skillBoard_UI, Func<int, int, bool> rollHit)
        {
            _enemySquad = enemySquad;
            _skillBoard_UI = skillBoard_UI;
            _rollHit = rollHit;
        }

        public override void Execute(Action onCompleted, bool isPlayerControlled = true)
        {
            _isPlayerControlled = isPlayerControlled;
            _skillExecuteCoroutine = _coroutineRunner.StartCoroutine(ExecuteCoroutine(onCompleted));
        }

        public override void StopExecute()
        {
            ClearTargetsSelection();
            base.StopExecute();
        }

        private IEnumerator ExecuteCoroutine(Action onCompleted)
        {
            _isTargetSelected = false;
            ShowTargets();

            if (_isPlayerControlled)
            {
                yield return new WaitUntil(() => _isTargetSelected);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                AfterTargetSelected(_targets[Random.Range(0, _targets.Count)]);
            }

            ClearTargetsSelection();
            _skillBoard_UI.SetInteractable(false);
            bool isImpactTime = false;
            bool isAttackAnimationEnd = false;
            Owner.PlayAttack(() => isImpactTime = true, () => isAttackAnimationEnd = true);
            yield return new WaitUntil(() => isImpactTime);
            bool isTargetAnimEnd = false;
            int attack = Owner.Attack;
            if (Owner.IsFrontliner)
            {
                attack = Mathf.Max(0, attack - _frontlineAttackPenalty);
            }

            if(_rollHit(attack, _target.Defense))
            {
                _target.TakeDamage(Random.Range(_minDamage, _maxDamage + 1), () => isTargetAnimEnd = true);
            }
            else
            {
                _target.ShowFeedback("Miss", Color.yellow, () => isTargetAnimEnd = true);
            }

            yield return new WaitUntil(() => isAttackAnimationEnd && isTargetAnimEnd);

            onCompleted?.Invoke();
        }

        private void ShowTargets()
        {
            _targets = _enemySquad.GetAliveCharacters();
            foreach (Character target in _targets)
            {
                if (_isPlayerControlled)
                {
                    target.OnPressed += AfterTargetSelected;
                }
                target.SetTargetActive(true);
            }
        }

        private void AfterTargetSelected(Character target)
        {
            _target = target;
            ClearTargetsSelection();
            _isTargetSelected = true;
        }

        private void ClearTargetsSelection()
        {
            if (_targets != null)
            {
                foreach (Character target in _targets)
                {
                    target.OnPressed -= AfterTargetSelected;
                    target.SetTargetActive(false);
                }

                _targets = null;
            }
        }
    }
}
