using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement
{
    private ICreatureControl input;
    private CreatureSettings settings;
    private Transform currentTransform;

    public CreatureMovement(ICreatureControl input, CreatureSettings settings, Transform currentTransform)
    {
        this.input = input;
        this.settings = settings;
        this.currentTransform = currentTransform;
    }

    public void Move()
    {
        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = 
            Vector3.RotateTowards(currentTransform.forward, input.Direction, settings.TurnSpeed*Time.deltaTime, 0.0f);

        currentTransform.rotation = Quaternion.LookRotation(newDirection);
        currentTransform.position += input.Direction * settings.Speed * Time.deltaTime;
    }
}
