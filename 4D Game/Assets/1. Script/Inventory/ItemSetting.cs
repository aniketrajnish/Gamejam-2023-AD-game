using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Attack,
    Dimension
}

[System.Serializable]
public class ItemSetting
{
    public int ID;
    public string Name;
    public ItemType Type;
    public int Value;

    public ItemSetting() { }

    public ItemSetting(int id, string name, ItemType type, int value)
    {
        ID = id;
        Name = name;
        Type = type;
        Value = value;
    }

    public ItemSetting Clone()
    {
        return new ItemSetting(ID, Name, Type, Value);
    }
}
