namespace MyProject
{
    public class RangedAttackSkill : Skill
    {
        public RangedAttackSkill(RangedSkillConfig rangedSkillConfig)
        {
            Config = rangedSkillConfig;
        }

        public override void UseSkill(SkillsManager skillsManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
