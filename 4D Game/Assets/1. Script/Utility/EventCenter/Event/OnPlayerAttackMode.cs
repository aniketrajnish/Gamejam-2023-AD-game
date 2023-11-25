using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerAttackMode : IEventWithData
{
    public bool IsAttackMode;

    public OnPlayerAttackMode(bool isAttackMode)
    {
        IsAttackMode = isAttackMode;
    }
}
