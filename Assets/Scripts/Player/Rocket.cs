using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet {

    public GameObject mExplosion;

    // Use this for initialization
    /*void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/

    public new void OnCollisionEnter(Collision col)
    {
        Instantiate(mExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
