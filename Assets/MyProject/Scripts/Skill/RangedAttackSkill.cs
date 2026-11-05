using System;
using System.Collections;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class RangedAttackSkill : Skill
    {
        private int _minDamage;
        private int _maxDamage;
        private SquadController _enemySquad;
        private RangedSkillConfig _config;

        public RangedAttackSkill(int minDamage, int maxDamage, RangedSkillConfig config) : base(config)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _config = config;
        }

        public void Prepare(SquadController enemySquad)
        {
            _enemySquad = enemySquad;
        }

        public List<Character> GetAvailableTargets()
        {
            return _enemySquad.GetAliveCharacters();
        }

        public override void Execute(Action onCompleted)
        {
            _skillExecuteCoroutine = _coroutineRunner.StartCoroutine(ExecuteCoroutine(onCompleted));
        }

        private IEnumerator ExecuteCoroutine(Action onCompleted)
        {
            Owner.ShowFeedback("Ranged Attack", UnityEngine.Color.yellow);
            yield return new UnityEngine.WaitForSeconds(1f);
            onCompleted?.Invoke();
        }
    }
}
