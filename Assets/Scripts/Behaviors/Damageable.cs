using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth { get; protected set; }
    public float health { get; protected set; }

    public virtual void ChangeHealth(float pChange)
    {
        health += pChange;
        float lHalfMax = maxHealth / 2;
        if (Mathf.Abs(health - lHalfMax) > lHalfMax) //either above 100 or dead af
        {
            if (health > 0)
            { //over 100
                health = maxHealth;
            }
            else
            {
                //Dead
                Die();
            }
        }
    }

    public float GetHealth() { return health; }

    public float GetMaxHealth() { return maxHealth; }

    public virtual void Die() { }
}
