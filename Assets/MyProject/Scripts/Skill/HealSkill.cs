using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class HealSkill : Skill, ITargetedSkill
    {
        public HealSkill(HealSkillConfig healSkillConfig) : base(healSkillConfig)
        {
            Config = healSkillConfig;
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
