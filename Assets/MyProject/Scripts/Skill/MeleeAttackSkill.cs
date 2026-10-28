namespace MyProject
{
    public class MeleeAttackSkill : Skill
    {
        private float _minDamage;
        private float _maxDamage;

        public MeleeAttackSkill(float minDamage, float maxDamage, MeleeSkillConfig meleeSkillConfig)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            Config = meleeSkillConfig;
        }

        public override void UseSkill(SkillsManager skillsManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
