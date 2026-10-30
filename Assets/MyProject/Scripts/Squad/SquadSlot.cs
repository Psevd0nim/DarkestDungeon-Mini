using UnityEngine;

namespace MyProject
{
    public class SquadSlot : MonoBehaviour
    {
        public Character CharacterInSlot { get; private set; }

        public void SetCharacter(Character character)
        {
            CharacterInSlot = character;
        }
    }
}
