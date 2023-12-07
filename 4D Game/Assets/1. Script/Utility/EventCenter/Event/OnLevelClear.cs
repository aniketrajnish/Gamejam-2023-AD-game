using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLevelClear : IEventWithData
{
    public int level;

    public OnLevelClear(int level)
    {
        this.level = level;
    }
}
