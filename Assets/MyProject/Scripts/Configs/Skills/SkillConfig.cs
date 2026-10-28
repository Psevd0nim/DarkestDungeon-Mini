using UnityEngine;

namespace MyProject
{
    public abstract class SkillConfig : ScriptableObject
    {
        public Sprite sprite;

        public abstract Skill CreateSkill();
    }
}
