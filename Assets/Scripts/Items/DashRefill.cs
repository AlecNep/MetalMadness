using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefill : Pickup
{
    void Awake()
    {
        effect = () =>
        {
            //if (!GameManager.Instance.player.CanDash())
            GameManager.Instance.player.RefillDash();
        };
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !GameManager.Instance.player.CanDash())
        {
            effect();
            Destroy(gameObject);
        }
    }
}
