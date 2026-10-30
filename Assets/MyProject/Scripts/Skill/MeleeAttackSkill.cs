using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace MyProject
{
    [Serializable]
    public class MeleeAttackSkill : Skill, ITargetedSkill
    {
        public override bool CanUse => Owner.IsFrontliner;

        private int _minDamage;
        private int _maxDamage;

        private SquadController _enemySquadController;

        public MeleeAttackSkill(int minDamage, int maxDamage, MeleeSkillConfig meleeSkillConfig) : base (meleeSkillConfig)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
        }

        public void Prepare(SquadController enemySquadController)
        {
            _enemySquadController = enemySquadController;
        }

        public List<Character> GetAvailableTargets()
        {
            List<Character> targets = new List<Character>();
            targets.Add(_enemySquadController.FrontlineCharacter);
            return targets;
        }

        public void Execute(Character target, Action onCompleted)
        {
            Owner.PlaySomeAnim(CharacterAnimType.Attack);
            target.TakeDamage(Random.Range(_minDamage, _maxDamage + 1));
            onCompleted?.Invoke();



        }
    }
}
