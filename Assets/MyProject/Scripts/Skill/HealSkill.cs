using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class HealSkill : Skill
    {
        private int _minHealValue;
        private int _maxHealValue;
        private SquadController _ownSquad;
        private HealSkillConfig _config;

        public HealSkill(int minHealValue, int maxHealValue, HealSkillConfig config) : base(config)
        {
            _minHealValue = minHealValue;
            _maxHealValue = maxHealValue;
            _config = config;
        }

        public void Prepare(SquadController ownSquad)
        {
            _ownSquad = ownSquad;
        }

        public List<Character> GetAvailableTargets()
        {
            return _ownSquad.GetAliveCharacters();
        }

        public override void Execute(Action onCompleted)
        {
            /*List<Character> recipients = new List<Character>();
            switch (_config.targetMode)
            {
                case HealTargetMode.SelectedAlly:
                    recipients.Add(target);
                    break;
                case HealTargetMode.AllAllies:
                    recipients = _ownSquad.GetAliveCharacters();
                    break;
                case HealTargetMode.AdjacentAllies:
                    foreach (Character ally in _ownSquad.GetAliveCharacters())
                    {
                        if (Math.Abs(ally.SquadPosNum - target.SquadPosNum) <= 1)
                        {
                            recipients.Add(ally);
                        }
                    }

                    break;
            }

            _skillExecutionManager.Heal(Owner, recipients, UnityEngine.Random.Range(_minHealValue, _maxHealValue + 1), Config, onCompleted);*/
            onCompleted?.Invoke();
        }
    }
}
