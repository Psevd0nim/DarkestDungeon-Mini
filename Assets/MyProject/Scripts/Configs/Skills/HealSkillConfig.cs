using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class HealSkillConfig : SkillConfig
    {
        public int minHealValue;
        public int maxHealValue;

        public override Skill CreateSkill()
        {
            return new HealSkill(minHealValue, maxHealValue, this);
        }
    }
}
