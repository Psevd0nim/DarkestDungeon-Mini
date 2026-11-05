using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [Serializable]
    public abstract class Skill
    {
        public virtual bool CanUse => Owner.IsAlive;

        public SkillConfig Config { get; protected set; }
        public Character Owner { get; private set; }

        private protected ICoroutineRunner _coroutineRunner;
        private protected Coroutine _skillExecuteCoroutine;
        private protected bool _isPlayerControlled;

        protected Skill(SkillConfig config)
        {
            Config = config;
        }

        public void Init(Character owner, ICoroutineRunner coroutineRunner)
        {
            Owner = owner;
            _coroutineRunner = coroutineRunner;
        }

        public abstract void Execute(Action onCompleted, bool isPlayerControlled = true);

        public virtual void StopExecute()
        {
            _coroutineRunner.StopCoroutine(_skillExecuteCoroutine);
        }
    }
}
