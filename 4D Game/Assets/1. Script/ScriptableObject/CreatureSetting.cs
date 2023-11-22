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
    public float DefaultHealth;
    public float Speed;
    public float TurnSpeed;

    public CreatureSetting() { }

    public CreatureSetting(CreatureType type, float defaultHP, float speed, float turnSpeed)
    {
        CreatureType = type;
        DefaultHealth = defaultHP;
        Speed = speed;
        TurnSpeed = turnSpeed;
    }

    public CreatureSetting Clone()
    {
        return new CreatureSetting(CreatureType, DefaultHealth, Speed, TurnSpeed);
    }
    
}
