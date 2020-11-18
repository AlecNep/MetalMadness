using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTip : MonoBehaviour {

    private FixedJoint mFj;

	// Use this for initialization
	void Start () {
        mFj = GetComponent<FixedJoint>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClearAnchors()
    {
        //FixedJoint lAnchor = gameObject.GetComponent<FixedJoint>();
        if (mFj != null)
        {
            //Destroy(mFj);  
            mFj.connectedBody = null;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor
            print("OnCollisionEnter: Attempting to anchor");
            //gameObject.AddComponent<FixedJoint>();
            mFj.connectedBody = col.rigidbody;
        }
        else
        {
            //might not be necessary
            //print("hitting " + col.gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody lRb = other.GetComponent<Rigidbody>();
        if (lRb)
        {
            print("OnTriggerEnter: Attempting to anchor");
            //gameObject.AddComponent<FixedJoint>();
            mFj.connectedBody = lRb;
            //mFj.enableCollision = true;
        }
    }
}
