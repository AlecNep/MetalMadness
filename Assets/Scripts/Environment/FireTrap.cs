using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            Damageable dmg = other.GetComponent<Damageable>();
            dmg.ChangeHealth(-damage);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "Enemy")
        {
            Damageable dmg = collision.collider.GetComponent<Damageable>();
            dmg.ChangeHealth(-damage);
        }
    }
}
