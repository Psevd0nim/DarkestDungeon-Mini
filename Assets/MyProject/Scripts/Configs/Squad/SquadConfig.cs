using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class SquadConfig : ScriptableObject
    {
        public CharacterConfig firstSquadSlot;
        public CharacterConfig secondSquadSlot;
        public CharacterConfig thirdSquadSlot;
        public CharacterConfig fourthSquadSlot;

        public CharacterConfig[] GetSquadFormation()
        {
            CharacterConfig[] squadFormation = new CharacterConfig[]
            {
                firstSquadSlot, secondSquadSlot, thirdSquadSlot, fourthSquadSlot,
            };
            return squadFormation;
        }
    }
}
