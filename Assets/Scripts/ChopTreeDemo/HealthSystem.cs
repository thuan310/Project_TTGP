using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;
    private int health;
    private int healthMax;

    public HealthSystem (int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health; 
    }

    public int GetHealthPercent()
    {
        return health / healthMax;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <=0)
        {
            health = 0;
            OnDead?.Invoke(this, EventArgs.Empty);
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > healthMax)
        {
            health = healthMax;
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this, EventArgs.Empty);
        }
    }
}
