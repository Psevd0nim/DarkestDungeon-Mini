using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float health;
    public float speed;
    public float attack;
    public float defense;

    public List<Skill> skillList;

    [SerializeField] private TextMeshPro _squadPosNum;

    public void Init(EntityData entityData, int squadPosNum)
    {
        health = entityData.health;
        speed = entityData.speed;
        attack = entityData.attack;
        defense = entityData.defense;
        skillList = entityData.skillList;
        _squadPosNum.text = squadPosNum.ToString();
    }

    public void TakeDamage(float damageValue)
    {
        health -= damageValue;
    }
}
