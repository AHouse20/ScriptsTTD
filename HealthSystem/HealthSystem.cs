using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler<int> OnDamaged;

    [SerializeField] private int health = 20;
    private int maxHealth;

    private void Awake()
    {
        maxHealth = health;
    }
    public void Damage(int damageToTake)
    {
        health -= damageToTake;

        if(health < 0) health = 0;

        OnDamaged?.Invoke(this, damageToTake);

        if (health == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }


}
