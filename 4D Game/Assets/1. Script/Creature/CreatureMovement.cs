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
        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = 
            Vector3.RotateTowards(target.forward, input.Direction, settings.TurnSpeed*Time.deltaTime, 0.0f);

        target.rotation = Quaternion.LookRotation(newDirection);
        target.position += input.Direction * settings.Speed * Time.deltaTime;
    }
}
