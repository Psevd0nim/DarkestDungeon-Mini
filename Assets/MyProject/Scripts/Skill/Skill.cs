using System;

namespace MyProject
{
    [Serializable]
    public abstract class Skill
    {
        public SkillConfig Config { get; private protected set; }

        public abstract void UseSkill(SkillsManager skillsManager);
    }
}
