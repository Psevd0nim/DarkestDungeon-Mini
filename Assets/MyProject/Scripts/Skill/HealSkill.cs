using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace MyProject
{
    [Serializable]
    public class HealSkill : Skill, ITargetedSkill
    {
        private int _minHealValue;
        private int _maxHealValue;

        private SquadController _playerSquadController;

        public HealSkill(int minHealValue, int maxHealValue, HealSkillConfig healSkillConfig) : base(healSkillConfig)
        {
            _minHealValue = minHealValue;
            _maxHealValue = maxHealValue;
        }

        public void Prepare(SquadController playerSquadController)
        {
            _playerSquadController = playerSquadController;
        }

        public void Execute(Character target, Action onCompleted)
        {
            target.Heal(Random.Range(_minHealValue, _maxHealValue + 1));
            onCompleted?.Invoke();
        }

        public List<Character> GetAvailableTargets()
        {
            return _playerSquadController.Characters;
        }
    }
}
