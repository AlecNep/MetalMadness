using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRefill : Pickup
{
    [SerializeField]
    private int amount;

    void Awake()
    {
        effect = () =>
        {
            GameManager.Instance.player.ChangeHealth(amount);
        };
    }
}
