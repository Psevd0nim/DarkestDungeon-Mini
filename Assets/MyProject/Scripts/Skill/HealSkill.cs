using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
    [Serializable]
    public class HealSkill : Skill
    {
        private int _minHealValue;
        private int _maxHealValue;
        private HealTargetMode _targetMode;
        private SquadController _ownSquad;
        private SkillBoard_UI _skillBoard_UI;
        private bool _isTargetSelected;
        private Character _target;
        private List<Character> _targets;

        public HealSkill(int minHealValue, int maxHealValue, HealTargetMode targetMode, HealSkillConfig config) : base(config)
        {
            _minHealValue = minHealValue;
            _maxHealValue = maxHealValue;
            _targetMode = targetMode;
        }

        public void Prepare(SquadController ownSquad, SkillBoard_UI skillBoard_UI)
        {
            _ownSquad = ownSquad;
            _skillBoard_UI = skillBoard_UI;
        }

        public override void Execute(Action onCompleted)
        {
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

            yield return new WaitUntil(() => _isTargetSelected);

            ClearTargetsSelection();
            _skillBoard_UI.SetInteractable(false);
            bool isImpactTime = false;
            bool isAttackAnimationEnd = false;
            Owner.PlayAttack(() => isImpactTime = true, () => isAttackAnimationEnd = true);
            yield return new WaitUntil(() => isImpactTime);
            List<Character> targets = GetTargets();
            List<Character> pendingTargets = new List<Character>(targets);
            foreach (Character target in targets)
            {
                int healValue = Random.Range(_minHealValue, _maxHealValue + 1);
                target.Heal(healValue, () => pendingTargets.Remove(target));
            }

            yield return new WaitUntil(() => isAttackAnimationEnd && pendingTargets.Count == 0);

            onCompleted?.Invoke();
        }

        private List<Character> GetTargets()
        {
            List<Character> targets = new List<Character>();
            switch (_targetMode)
            {
                case HealTargetMode.SelectedAlly:
                    targets.Add(_target);
                    break;
                case HealTargetMode.AllAllies:
                    targets = _ownSquad.GetAliveCharacters();
                    break;
            }

            return targets;
        }

        private void ShowTargets()
        {
            _targets = _ownSquad.GetAliveCharacters();
            foreach (Character target in _targets)
            {
                target.OnPressed += AfterTargetSelected;
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
