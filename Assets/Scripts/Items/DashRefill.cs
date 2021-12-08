using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefill : Pickup
{
    void Awake()
    {
        effect = () =>
        {
            GameManager.Instance.player.RefillDash();
        };
    }
}
