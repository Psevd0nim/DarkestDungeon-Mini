using UnityEngine;

namespace MyProject
{
    public abstract class SkillConfig : ScriptableObject
    {
        public string displayName;
        public Sprite sprite;
        [TextArea] public string tip;

        public abstract Skill CreateSkill();

        public virtual string GetDescription()
        {
            return tip;
        }
    }
}
