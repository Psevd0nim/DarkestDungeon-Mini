using UnityEngine;

namespace MyProject
{
    public abstract class SkillConfig : ScriptableObject
    {
        public Sprite sprite;
        public string tip;

        public abstract Skill CreateSkill();
    }
}
