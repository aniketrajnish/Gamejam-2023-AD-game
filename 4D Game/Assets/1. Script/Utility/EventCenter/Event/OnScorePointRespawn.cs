using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScorePointRespawn : IEventWithData
{
    public ItemSetting RespawnedItem;

    public OnScorePointRespawn(ItemSetting itemSetting)
    {
        RespawnedItem = itemSetting;
    }
}
