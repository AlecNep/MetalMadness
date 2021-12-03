using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKillHazard : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Damageable d))
        {
            d.Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Damageable d))
        {
            d.Die();
        }
    }
}
