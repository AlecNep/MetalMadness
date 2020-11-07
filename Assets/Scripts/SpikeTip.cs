using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTip : MonoBehaviour {

    //FixedJoint mFj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor

        }
        else
        {
            //might not be necessary
            print("hitting " + col.gameObject.name);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody lRb = other.GetComponent<Rigidbody>();
        if (lRb)
        {
            gameObject.AddComponent<FixedJoint>();
            GetComponent<FixedJoint>().connectedBody = lRb;
        }
    }
}
