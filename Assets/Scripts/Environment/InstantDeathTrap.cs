using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            Damageable dmg = other.GetComponent<Damageable>();
            dmg.SetHealth(0);
        }
    }
}
