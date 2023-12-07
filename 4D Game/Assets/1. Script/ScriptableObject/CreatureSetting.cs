using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatureType
{
    Player,
    Enemy,
    Elite,
    Boss
}

[System.Serializable]
public class CreatureSetting
{
    [Header("Control")]
    public CreatureType CreatureType;

    [Header("Stat")]
    public int DefaultHealth;
    public float Speed;
    public float TurnSpeed;
    public int Value;

    public CreatureSetting() { }

    public CreatureSetting(CreatureType type, int defaultHP, float speed, float turnSpeed, int value)
    {
        CreatureType = type;
        DefaultHealth = defaultHP;
        Speed = speed;
        TurnSpeed = turnSpeed;
        Value = value;
    }

    public CreatureSetting Clone()
    {
        return new CreatureSetting(CreatureType, DefaultHealth, Speed, TurnSpeed, Value);
    }
    
}
