using System;
using UnityEngine;

public class GravShiftRefill : Pickup
{
    void Awake()
    {
        effect = () => 
        {
            //if (!GameManager.Instance.player.CanShift())
            GameManager.Instance.player.RefillGravity();
        };
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !GameManager.Instance.player.CanShift())
        {
            effect();
            Destroy(gameObject);
        }
    }
}
