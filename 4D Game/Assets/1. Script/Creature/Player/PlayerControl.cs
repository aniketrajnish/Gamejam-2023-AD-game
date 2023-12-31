using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : ICreatureControl
{
    public Vector3 TurnDirection { get; private set; }

    public Vector3 Direction { get; private set; }

    public void ReadInput()
    { 
        Direction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")).normalized;
        TurnDirection = Direction;
    }
}
