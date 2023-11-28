using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUsingMachine : IEventWithData
{
    public float amount;

    public OnUsingMachine(float amount)
    {
        this.amount = amount;
    }
}
