namespace MyProject
{
    public class SkipTurnSkill : Skill
    {
        public SkipTurnSkill(SkipTurnSkillConfig skipTurnSkillConfig)
        {
            Config = skipTurnSkillConfig;
        }

        public override void UseSkill(SkillsManager skillsManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
