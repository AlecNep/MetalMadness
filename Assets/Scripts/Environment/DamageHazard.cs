using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHazard : MonoBehaviour
{
    [SerializeField]
    private float damage;

    /// <summary>
    /// Causes damage if the player is inside a damaging zone
    /// Used if the attached collider is a trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            Damageable dmg = other.GetComponent<Damageable>();
            dmg.ChangeHealth(-damage);
        }
    }

    /// <summary>
    /// Causes damage if the player is touching the object
    /// Used if the attached collider is not a trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "Enemy")
        {
            Damageable dmg = collision.collider.GetComponent<Damageable>();
            dmg.ChangeHealth(-damage);
        }
    }
}
