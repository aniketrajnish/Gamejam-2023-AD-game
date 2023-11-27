using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerAttackMode : IEventWithData
{
    public int TimeLeft;

    public OnPlayerAttackMode(int timeLeft)
    {
        TimeLeft = timeLeft;
    }
}
