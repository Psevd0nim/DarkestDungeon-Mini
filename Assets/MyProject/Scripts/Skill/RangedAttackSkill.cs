using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class RangedAttackSkill : Skill, ITargetedSkill
    {
        public RangedAttackSkill(RangedSkillConfig rangedSkillConfig) : base(rangedSkillConfig)
        {
            Config = rangedSkillConfig;
        }

        public void Execute(Character target, Action onCompleted)
        {
            throw new NotImplementedException();
        }

        public List<Character> GetAvailableTargets()
        {
            throw new NotImplementedException();
        }
    }
}
