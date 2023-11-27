using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureStat
{
    public int Health { get { return health; } }
    public float Speed { get { return speed; } }
    public event EventHandler<HealthChangeEvent> OnHealthChanged;

    private int maxHealth;
    private float maxSpeed;
    private int health;
    private float speed;

    public CreatureStat(int maxHealth, float maxSpeed)
    {
        this.maxHealth = maxHealth;
        this.maxSpeed = maxSpeed;
        health = maxHealth;
        speed = maxSpeed;
    }

    public void Reset()
    {
        health = maxHealth;
        speed = maxSpeed;
    }

    public void ModifyHealth(int num)
    {
        health += num;

        if(health <=0)
        {
            health = 0;
        }
        else if(health >= maxHealth)
        {
            health = maxHealth;
        }

        if(OnHealthChanged != null)
            OnHealthChanged(this, new HealthChangeEvent(health, maxHealth));
    }

    public void ModifySpeed(float num)
    {
        speed += num;

        if (speed <= 0)
        {
            speed = 0;
        }
        else if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
    }

}
