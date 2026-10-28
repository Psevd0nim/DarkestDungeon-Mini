using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class MeleeSkillConfig : SkillConfig
    {
        public float minDamage;
        public float maxDamage;

        public override Skill CreateSkill()
        {
            return new MeleeAttackSkill(minDamage, maxDamage, this);
        }
    }
}
