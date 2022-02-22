using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    protected float maxHealth;
    protected float health;
    public bool isAlive = true;

    public virtual void ChangeHealth(float pChange)
    {
        health += pChange;
        if (health > maxHealth)
            health = maxHealth;
        else if (health <= 0)
            Die();
    }

    public float GetHealth() { return health; }

    public void SetHealth(float h) { health = h; }

    public float GetMaxHealth() { return maxHealth; }

    public virtual void Die() { Destroy(gameObject); }
}
