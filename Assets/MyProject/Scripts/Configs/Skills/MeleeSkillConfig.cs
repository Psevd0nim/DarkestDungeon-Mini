using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class MeleeSkillConfig : SkillConfig
    {
        public int minDamage;
        public int maxDamage;

        public override Skill CreateSkill()
        {
            return new MeleeAttackSkill(minDamage, maxDamage, this);
        }

        public override string GetDescription()
        {
            return tip + "\nDamage: " + minDamage + "-" + maxDamage + ".\nFront position only.";
        }
    }
}
