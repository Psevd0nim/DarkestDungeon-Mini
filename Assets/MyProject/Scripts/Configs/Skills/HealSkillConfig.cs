using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class HealSkillConfig : SkillConfig
    {
        public override Skill CreateSkill()
        {
            return new HealSkill(this);
        }
    }
}
