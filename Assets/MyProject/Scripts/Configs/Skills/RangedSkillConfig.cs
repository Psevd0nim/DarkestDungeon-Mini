using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class RangedSkillConfig : SkillConfig
    {
        public override Skill CreateSkill()
        {
            return new RangedAttackSkill(this);
        }
    }
}
