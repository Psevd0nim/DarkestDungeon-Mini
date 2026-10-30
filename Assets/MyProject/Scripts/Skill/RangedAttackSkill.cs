using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace MyProject
{
    [Serializable]
    public class RangedAttackSkill : Skill, ITargetedSkill
    {
        private int _minDamage;
        private int _maxDamage;

        private SquadController _enemySquadController;

        public RangedAttackSkill(int minDamage, int maxDamage, RangedSkillConfig rangedSkillConfig) : base(rangedSkillConfig)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
        }

        public void Prepare(SquadController enemySquadController)
        {
            _enemySquadController = enemySquadController;
        }

        public void Execute(Character target, Action onCompleted)
        {
            target.TakeDamage(Random.Range(_minDamage, _maxDamage + 1));
            onCompleted?.Invoke();
        }

        public List<Character> GetAvailableTargets()
        {
            return _enemySquadController.Characters;
        }
    }
}
