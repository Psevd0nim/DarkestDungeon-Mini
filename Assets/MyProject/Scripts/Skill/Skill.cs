using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public abstract class Skill
    {
        public virtual bool CanUse => Owner.IsAlive;
        public SkillConfig Config { get; protected set; }
        public Character Owner { get; private set; }

        protected SkillExecutionManager _skillExecutionManager;

        protected Skill(SkillConfig config)
        {
            Config = config;
        }

        public void Init(Character owner, SkillExecutionManager skillExecutionManager)
        {
            Owner = owner;
            _skillExecutionManager = skillExecutionManager;
        }
    }

    public interface ITargetedSkill
    {
        List<Character> GetAvailableTargets();

        void Execute(Character target, Action onCompleted);
    }

    public interface IImmediateSkill
    {
        void Execute(Action onCompleted);
    }
}
