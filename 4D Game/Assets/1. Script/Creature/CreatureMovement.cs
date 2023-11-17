using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement
{
    private ICreatureControl input;
    private CreatureSettings settings;
    private Transform target;

    public CreatureMovement(ICreatureControl input, CreatureSettings settings, Transform target)
    {
        this.input = input;
        this.settings = settings;
        this.target = target;
    }

    public void Move()
    {
        target.Rotate(Vector3.up * input.Rotation * settings.TurnSpeed * Time.deltaTime);
        target.position += input.Direction * settings.Speed * Time.deltaTime;
    }
}
