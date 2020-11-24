using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTip : MonoBehaviour {

    private FixedJoint mFj;
    private Transform mPlayer;

	// Use this for initialization
	void Start () {
        mFj = GetComponent<FixedJoint>();
        //mPlayer = transform.root;
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
            //mPlayer.SetParent(null, true);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor            
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
            //gameObject.AddComponent<FixedJoint>();
            //mFj.connectedBody = lRb;
            //mFj.enableCollision = true;
        }
    }
}
