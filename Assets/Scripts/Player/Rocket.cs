using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet {

    public GameObject mExplosion;

    public new void OnCollisionEnter(Collision col)
    {
        Instantiate(mExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
