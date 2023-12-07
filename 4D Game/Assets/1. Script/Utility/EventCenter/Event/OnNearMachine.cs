using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNearMachine : IEventWithData
{
    public bool IsNear;
    public bool IsInteractable;
    
    public OnNearMachine(bool isNear, bool isInteractable)
    {
        IsNear = isNear;
        IsInteractable = isInteractable;
    }
}
