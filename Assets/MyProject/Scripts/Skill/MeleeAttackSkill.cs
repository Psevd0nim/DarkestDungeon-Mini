using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [Serializable]
    public class MeleeAttackSkill : Skill, ITargetedSkill
    {
        private int _minDamage;
        private int _maxDamage;
        private SquadController _enemySquad;
        public override bool CanUse => base.CanUse && Owner.IsFrontliner;

        public MeleeAttackSkill(int minDamage, int maxDamage, MeleeSkillConfig config) : base(config)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
        }

        public void Prepare(SquadController enemySquad)
        {
            _enemySquad = enemySquad;
        }

        public List<Character> GetAvailableTargets()
        {
            var targets = new List<Character>();
            if (CanUse && _enemySquad.FrontlineCharacter != null)
            {
                targets.Add(_enemySquad.FrontlineCharacter);
            }

            return targets;
        }

        public void Execute(Character target, Action onCompleted)
        {
            _skillExecutionManager.Attack(Owner, target, UnityEngine.Random.Range(_minDamage, _maxDamage + 1), 0, Config, onCompleted);
        }
    }
}
