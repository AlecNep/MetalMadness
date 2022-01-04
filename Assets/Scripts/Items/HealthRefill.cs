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
            //if (GameManager.Instance.player.GetHealth() < GameManager.Instance.player.GetMaxHealth())
            GameManager.Instance.player.ChangeHealth(amount);
        };
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.player.GetHealth() < GameManager.Instance.player.GetMaxHealth())
        {
            effect();
            Destroy(gameObject);
        }
    }
}
