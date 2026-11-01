using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class RangedSkillConfig : SkillConfig
    {
        public int minDamage;
        public int maxDamage;
        public int frontlineAttackPenalty = 4;

        public override Skill CreateSkill()
        {
            return new RangedAttackSkill(minDamage, maxDamage, this);
        }

        public override string GetDescription()
        {
            return tip + "\nDamage: " + minDamage + "-" + maxDamage + ".\nFront position: -" + frontlineAttackPenalty + " accuracy points.";
        }
    }
}
