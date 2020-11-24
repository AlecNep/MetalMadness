using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTip : MonoBehaviour {

    //private FixedJoint mFj;
    //private Transform mPlayer;

    [SerializeField] FixedJoint mFixedJointTarget; // 1st fixed joint on tip
    [SerializeField] FixedJoint mFixedJointPlayer; // 2nd fixed joint on tip

    [SerializeField] Rigidbody mRB;     // rigidbody of tip
    [SerializeField] Rigidbody mPlayerRB; // player's rigidbody

    [SerializeField] Collider mColl;

    // Use this for initialization
    void Start () {
        //mFj = GetComponent<FixedJoint>();
        //mPlayer = transform.root;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartAnchor(Rigidbody pTargetRB)
    {
        mRB.isKinematic = false;

        mFixedJointTarget.connectedBody = pTargetRB;
        mFixedJointPlayer.connectedBody = mPlayerRB;
    }

    public void ClearAnchors()
    {
        //FixedJoint lAnchor = gameObject.GetComponent<FixedJoint>();
        /*if (mFj != null)
        {
            //Destroy(mFj);  
            mFj.connectedBody = null;
            //mPlayer.SetParent(null, true);
        }*/

        mRB.isKinematic = true;
        mFixedJointTarget.connectedBody = null;
        mFixedJointPlayer.connectedBody = null;

        // re-enable before shooting again, of course
        // may not be necessary depending on what else you do when you stop anchoring.
        //mColl.enabled = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor            
            //mFj.connectedBody = col.rigidbody;
            StartAnchor(col.rigidbody);
        }
        /*else
        {
            //might not be necessary
            //print("hitting " + col.gameObject.name);
        }*/
    }

    private void OnCollisionExit(Collision col)
    {
        ClearAnchors();
    }
}
