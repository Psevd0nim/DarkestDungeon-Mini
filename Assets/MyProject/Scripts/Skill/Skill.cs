using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public abstract class Skill
    {
        public virtual bool CanUse => true;

        public SkillConfig Config { get; private protected set; }
        public Character Owner { get; private protected set; }
        public Skill_UI Skill_UI { get; private protected set; }

        public Skill(SkillConfig config)
        {
            Config = config;
        }

        public void SetSkillUI(Skill_UI skill_UI)
        {
            Skill_UI = skill_UI;
        }

        public void SetCharacter(Character character)
        {
            Owner = character;
        }
    }

    public interface ITargetedSkill
    {
        public List<Character> GetAvailableTargets();
        public void Execute(Character target, Action onCompleted);
    }

    public interface IImmediateSkill
    {
        public void Execute(Action onCompleted);
    }
}
