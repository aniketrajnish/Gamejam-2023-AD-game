using System;

public class HealthChangeEvent : EventArgs
{
    private float currentHealth;
    private float currentMaxHealth;
    private float currentHealthPercent;

    public float CurrentHealth { get { return currentHealth; } }
    public float CurrentMaxHealth { get { return currentMaxHealth; } }
    public float CurrentHealthPercent { get { return currentHealthPercent; } }

    public HealthChangeEvent(float currentHealth, float currentMaxHealth)
    {
        this.currentHealth = currentHealth;
        this.currentMaxHealth = currentMaxHealth;
        currentHealthPercent = currentHealth / currentMaxHealth;
    }

}