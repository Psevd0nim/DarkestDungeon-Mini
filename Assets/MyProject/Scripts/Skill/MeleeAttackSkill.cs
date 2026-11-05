using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
    [Serializable]
    public class MeleeAttackSkill : Skill
    {
        public override bool CanUse => base.CanUse && Owner.IsFrontliner;

        private int _minDamage;
        private int _maxDamage;
        private SquadController _enemySquad;
        private SkillBoard_UI _skillBoard_UI;
        private bool _isTargetSelected;
        private Character _target;
        private Func<int, int, bool> _rollHit;

        public MeleeAttackSkill(int minDamage, int maxDamage, MeleeSkillConfig config) : base(config)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
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
            ClearTargetSelection();
            base.StopExecute();
        }

        private IEnumerator ExecuteCoroutine(Action onCompleted)
        {
            _isTargetSelected = false;
            ShowTarget();

            if (_isPlayerControlled)
            {
                yield return new WaitUntil(() => _isTargetSelected);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                AfterTargetSelected(_target);
            }

            ClearTargetSelection();
            _skillBoard_UI.SetInteractable(false);
            bool isImpactTime = false;
            bool isAttackAnimationEnd = false;
            Owner.PlayAttack(() => isImpactTime = true, () => isAttackAnimationEnd = true);
            yield return new WaitUntil(() => isImpactTime);
            bool isTargetAnimEnd = false;
            if(_rollHit(Owner.Attack, _target.Defense))
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

        private void ShowTarget()
        {
            _target = _enemySquad.FrontlineCharacter;
            if (_isPlayerControlled)
            {
                _target.OnPressed += AfterTargetSelected;
            }
            _target.SetTargetActive(true);
        }

        private void AfterTargetSelected(Character target)
        {
            target.OnPressed -= AfterTargetSelected;
            _isTargetSelected = true;
        }

        private void ClearTargetSelection()
        {
            if (_target != null)
            {
                _target.OnPressed -= AfterTargetSelected;
                _target.SetTargetActive(false);
            }
        }
    }
}
