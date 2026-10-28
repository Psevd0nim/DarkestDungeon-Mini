using System;
using System.Collections.Generic;

namespace MyProject
{
    [Serializable]
    public class EntityData
    {
        public float health;
        public float speed;
        public float attack;
        public float defense;

        public EntityData(EntityData entityData)
        {
            health = entityData.health;
            speed = entityData.speed;
            attack = entityData.attack;
            defense = entityData.defense;
        }
    }
}
