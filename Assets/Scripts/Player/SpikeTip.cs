using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTip : MonoBehaviour {

    //private FixedJoint mFj;
    private PlayerControls mPlayer;

    [SerializeField] FixedJoint mFixedJointTarget; // 1st fixed joint on tip
    [SerializeField] FixedJoint mFixedJointPlayer; // 2nd fixed joint on tip

    [SerializeField] Rigidbody mRB;     // rigidbody of tip
    [SerializeField] Rigidbody mPlayerRB; // player's rigidbody

    [SerializeField] Collider mColl;

    /*private Spike mSpikeBase;
    private Vector3 mStartPos;
    private Vector3 mEndPos;*/

    

    // Use this for initialization
    void Start () {
        mPlayer = transform.root.GetComponent<PlayerControls>();
        /*mSpikeBase = GetComponentInParent<Spike>();
        if (mSpikeBase == null)
        {
            print("Shit, couldn't find the spike base");
        }

        mStartPos = transform.localPosition;
        mEndPos = mStartPos + Vector3.up * mSpikeBase.mConeEnd;*/

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartAnchor(Rigidbody pTargetRB, string pTag)
    {
        /*if (!mSpikeBase.mExtended)
        {
            yield return new WaitUntil(() => mSpikeBase.mExtended);
        }*/

        //mRB.isKinematic = false;
        mPlayer.SpikeAttached(pTag);
        if (pTag == "Enemy")
        {
            //TODO: Make the enemy a child of the player if applicable
        }
        mFixedJointTarget.connectedBody = pTargetRB;
        mFixedJointPlayer.connectedBody = mPlayerRB;
    }

    public void ClearAnchors()
    {
        //mRB.isKinematic = true;
        mPlayer.SpikeDetached();
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
            StartAnchor(col.rigidbody, col.gameObject.tag);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        ClearAnchors();
    }
}
