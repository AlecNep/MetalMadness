using UnityEngine;
using System;

public abstract class Pickup : MonoBehaviour
{
    protected Action effect;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            effect();
            Destroy(gameObject);
        }
        
    }
}