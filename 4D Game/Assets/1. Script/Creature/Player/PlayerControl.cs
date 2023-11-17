using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : ICreatureControl
{
    public float Rotation { get; private set; }

    public Vector3 Direction { get; private set; }

    public void ReadInput()
    {
        Rotation = Input.GetAxis("Mouse X");
        Direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}
