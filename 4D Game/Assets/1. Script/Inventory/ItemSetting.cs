using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Attack,
    Dimension,
    Score
}

[System.Serializable]
public class ItemSetting
{
    public int ID;
    public string Name;
    public ItemType Type;
    public int Value;
    public float RespawnTime;

    public ItemSetting() { }

    public ItemSetting(int id, string name, ItemType type, int value, float respawnTime)
    {
        ID = id;
        Name = name;
        Type = type;
        Value = value;
        RespawnTime = respawnTime;
    }

    public ItemSetting Clone()
    {
        return new ItemSetting(ID, Name, Type, Value, RespawnTime);
    }
}
