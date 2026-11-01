using UnityEngine;

namespace MyProject
{
    public enum HealTargetMode
    {
        SelectedAlly,
        AdjacentAllies,
        AllAllies
    }

    [CreateAssetMenu]
    public class HealSkillConfig : SkillConfig
    {
        public int minHealValue;
        public int maxHealValue;
        public HealTargetMode targetMode;

        public override Skill CreateSkill()
        {
            return new HealSkill(minHealValue, maxHealValue, this);
        }

        public override string GetDescription()
        {
            string recipients;
            switch (targetMode)
            {
                case HealTargetMode.AdjacentAllies:
                    recipients = "Selected ally and adjacent positions.";
                    break;
                case HealTargetMode.AllAllies:
                    recipients = "All living allies.";
                    break;
                default:
                    recipients = "One living ally, including yourself.";
                    break;
            }

            return tip + "\nRestore " + minHealValue + "-" + maxHealValue + " HP, up to maximum.\n" + recipients;
        }
    }
}
