using System;

namespace MyProject
{
    [Serializable]
    public class SkipTurnSkill : Skill, IImmediateSkill
    {
        public SkipTurnSkill(SkipTurnSkillConfig config) : base(config)
        {
        }

        public void Execute(Action onCompleted)
        {
            _skillExecutionManager.Skip(Owner, Config, onCompleted);
        }
    }
}
