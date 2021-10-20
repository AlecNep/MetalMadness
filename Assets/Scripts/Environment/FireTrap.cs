using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField]
    float damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            Damageable dmg = other.GetComponent<Damageable>();
            dmg.ChangeHealth(-damage);
        }
    }
}
