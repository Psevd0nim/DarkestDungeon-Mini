using System;
using System.Collections.Generic;

[Serializable]
public class EntityData
{
    public float health;
    public float speed;
    public float attack;
    public float defense;
    public List<Skill> skillList;

    public EntityData(float health, float speed, float attack, float defense, List<Skill> skillList)
    {
        this.health = health;
        this.speed = speed;
        this.attack = attack;
        this.defense = defense;
        this.skillList = skillList;
    }
}