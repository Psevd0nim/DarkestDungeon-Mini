namespace MyProject
{
    public class HealSkill : Skill
    {
        public HealSkill(HealSkillConfig healSkillConfig)
        {
            Config = healSkillConfig;
        }

        public override void UseSkill(SkillsManager skillsManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
