using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDimensionChanging : IEventWithData
{
    public bool isChanging;

    public OnDimensionChanging(bool isChanging)
    {
        this.isChanging = isChanging;
    }
}
