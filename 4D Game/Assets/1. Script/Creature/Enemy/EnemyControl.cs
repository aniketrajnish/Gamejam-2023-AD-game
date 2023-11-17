using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : ICreatureControl
{
    public Vector3 TurnDirection { get; private set; }

    public Vector3 Direction { get; private set; }

    private Vector3 currentDirection = new Vector3(-1, 0, -1);
    private float xModifier = 0.5f;
    private float zModifier = 1;

    public void ReadInput()
    {
        if (currentDirection.x < -1) xModifier = 0.5f;
        if (currentDirection.z < -1) zModifier = 1;
        if (currentDirection.x > 1) xModifier = -0.5f;
        if (currentDirection.z > 1) zModifier = -1;

        currentDirection.x += Time.deltaTime * xModifier;
        currentDirection.z += Time.deltaTime * zModifier;

        //TEST
        Direction = currentDirection;
        TurnDirection = Direction;
    }
}
