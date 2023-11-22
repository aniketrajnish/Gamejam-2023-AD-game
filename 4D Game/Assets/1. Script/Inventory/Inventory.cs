using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemSetting> ItemBag = new List<ItemSetting>();

    public void AddItem(ItemSetting item)
    {
        ItemBag.Add(item);
    }   

    public void RemoveItem(ItemSetting item) 
    {  
        ItemBag.Remove(item); 
    }

    public ItemSetting GetItem(int id)
    {
        return ItemBag.Find(item => item.ID == id);
    }
}
