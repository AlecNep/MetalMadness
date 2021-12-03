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
}
