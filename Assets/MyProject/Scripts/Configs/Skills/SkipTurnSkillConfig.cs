using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class SkipTurnSkillConfig : SkillConfig
    {
        public override Skill CreateSkill()
        {
            return new SkipTurnSkill(this);
        }
    }
}
