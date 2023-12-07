using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemSetting> ItemBag = new List<ItemSetting>();

    public void AddItem(ItemSetting item)
    {
        if (!ItemBag.Contains(item))
            ItemBag.Add(item);
    }   

    public void RemoveItem(ItemSetting item) 
    {
        if (ItemBag.Contains(item))
            ItemBag.Remove(item); 
    }

    public bool HasItem(ItemSetting item)
    {
        return ItemBag.Contains(item);
    }

    public ItemSetting GetItem(int id)
    {
        return ItemBag.Find(item => item.ID == id);
    }

    public int GetSize()
    {
        return ItemBag.Count;
    }

    public int GetItemCountByID(int id)
    {
        int count = 0;
        foreach (ItemSetting item in ItemBag)
        {
            if(item.ID == id)
                count++;
        }

        return count;
    }
}
