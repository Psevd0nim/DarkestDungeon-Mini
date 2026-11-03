using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class RangedAttackSkill : Skill, ITargetedSkill
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

        public void Execute(Action onCompleted)
        {
            /*int penalty = Owner.IsFrontliner ? _config.frontlineAttackPenalty : 0;
            _skillExecutionManager.Attack(Owner, target, UnityEngine.Random.Range(_minDamage, _maxDamage + 1), penalty, Config, onCompleted);*/
            onCompleted?.Invoke();
        }
    }
}
