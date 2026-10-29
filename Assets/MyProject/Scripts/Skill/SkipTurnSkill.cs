using System;

namespace MyProject
{
    [Serializable]
    public class SkipTurnSkill : Skill, IImmediateSkill
    {
        public SkipTurnSkill(SkipTurnSkillConfig skipTurnSkillConfig) : base(skipTurnSkillConfig)
        {
            Config = skipTurnSkillConfig;
        }

        public void Execute(Action onCompleted)
        {
            onCompleted?.Invoke();
        }
    }
}
