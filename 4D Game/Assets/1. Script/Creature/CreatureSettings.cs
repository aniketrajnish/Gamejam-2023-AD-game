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

[CreateAssetMenu(fileName = "Creature", menuName = "ScriptableObjects/CreatureSettings", order = 1)]
public class CreatureSettingsSO : ScriptableObject
{
    public CreatureSettings CreatureSettings;
}

[System.Serializable]
public class CreatureSettings
{
    [Header("Control")]
    public CreatureType CreatureType;

    [Header("Stat")]
    public float DefaultHealth;
    public float Speed;
    public float TurnSpeed;

    public CreatureSettings() { }

    public CreatureSettings(CreatureType type, float defaultHP, float speed, float turnSpeed)
    {
        CreatureType = type;
        DefaultHealth = defaultHP;
        Speed = speed;
        TurnSpeed = turnSpeed;
    }

    public CreatureSettings Clone()
    {
        return new CreatureSettings(CreatureType, DefaultHealth, Speed, TurnSpeed);
    }
    
}
