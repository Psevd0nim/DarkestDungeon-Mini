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

        private protected ICoroutineRunner _coroutineRunner;

        protected Skill(SkillConfig config)
        {
            Config = config;
        }

        public void Init(Character owner, ICoroutineRunner coroutineRunner)
        {
            Owner = owner;
            _coroutineRunner = coroutineRunner;
        }
    }

    public interface ITargetedSkill
    {
        List<Character> GetAvailableTargets();

        void Execute(Action onCompleted);
    }

    public interface IImmediateSkill
    {
        void Execute(Action onCompleted);
    }
}
