using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : Bullet {

    bool mDetonation = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public new void SetDirection(Vector3 pDir) //didn't fix the problem
    {
        mRb.AddForce(pDir * mSpeed, ForceMode.Impulse);
    }

    /*public new void OnCollisionEnter(Collision col)
    {

    }*/
}
