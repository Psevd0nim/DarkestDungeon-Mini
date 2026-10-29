using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class CharacterData
    {
        public int health;
        public int speed;
        public int attack;
        public int defense;

        public CharacterData(CharacterData characterData)
        {
            health = characterData.health;
            speed = characterData.speed;
            attack = characterData.attack;
            defense = characterData.defense;
        }
    }
}
