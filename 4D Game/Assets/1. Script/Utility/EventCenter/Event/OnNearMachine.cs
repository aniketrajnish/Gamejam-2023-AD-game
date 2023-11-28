using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNearMachine : IEventWithData
{
    public bool IsNear;
    
    public OnNearMachine(bool isNear)
    {
        IsNear = isNear;
    }
}
