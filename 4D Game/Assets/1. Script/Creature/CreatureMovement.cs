using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMovement
{
    private ICreatureControl input;
    private CreatureSetting setting;
    private Transform currentTransform;

    public CreatureMovement(ICreatureControl input, CreatureSetting setting, Transform currentTransform)
    {
        this.input = input;
        this.setting = setting;
        this.currentTransform = currentTransform;
    }

    public void Move()
    {
        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = 
            Vector3.RotateTowards(currentTransform.forward, input.Direction, setting.TurnSpeed*Time.deltaTime, 0.0f);

        currentTransform.rotation = Quaternion.LookRotation(newDirection);
        currentTransform.position += input.Direction * setting.Speed * Time.deltaTime;

        Debug.Log(currentTransform.position);
    }
}
