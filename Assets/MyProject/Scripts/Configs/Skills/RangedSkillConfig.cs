using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class RangedSkillConfig : SkillConfig
    {
        public int minDamage;
        public int maxDamage;

        public override Skill CreateSkill()
        {
            return new RangedAttackSkill(minDamage, maxDamage, this);
        }
    }
}
