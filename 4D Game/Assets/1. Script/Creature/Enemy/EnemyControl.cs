using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : ICreatureControl
{
    public Vector3 Rotation { get; private set; }

    public Vector3 Direction { get; private set; }

    public void ReadInput()
    {
        //TBD
    }
}
