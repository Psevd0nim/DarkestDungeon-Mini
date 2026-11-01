using UnityEngine;

namespace MyProject
{
    public abstract class SkillConfig : ScriptableObject
    {
        public string displayName;
        public Sprite sprite;
        [TextArea] public string tip;
        [Range(0.1f, 0.9f)] public float impactTime = 0.5f;

        public abstract Skill CreateSkill();

        public virtual string GetDescription()
        {
            return tip;
        }
    }
}
