using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Bullet {

    //origins will almost certainly need to be calculated based on the parent, not hard-coded
    private Transform[] mSpikeSections;
    private float mBaseSpikeStart = 0.644f;
    private float mBaseSpikeEnd = 0.944f;
    private float mCylinderStart = 0f;
    private float mCylinderEnd = -2f; //from 0 to -2
    private float mConeStart = 1.26f;
    private float mConeEnd = -0.66f; //will probably need to be adjusted //from 1.26 to -0.66

    private IEnumerator mExpand;
    private IEnumerator mCollapse;

	// Use this for initialization
	void Start () {
        mMaxLifespan = Mathf.Infinity;
        Transform lS = transform;
        //Hardcoded for now. Maybe make a local list then convert it into an array
        //Maybe make it one-less than the total amount since mSpikeSections[0] doesn't need to move
        mSpikeSections = new Transform[6]; 
        int i = 0;

        while (lS.childCount > 0)
        {
            Transform temp = lS.GetChild(0);
            mSpikeSections[i] = temp;
            lS = temp;

            i++;
        }

        mExpand = ExpandSequence();
        //mCollapse = CollapseSequence();
    }
    
    public IEnumerator ExpandSequence()
    {
        while (transform.position.x > mBaseSpikeEnd && mSpikeSections[1].position.y > mCylinderEnd && mSpikeSections[5].position.y > mConeEnd)
        { //only using the first cylinder section in the condition since they all move the same distance

        }
        yield return null;
    }

    /*public IEnumerator CollapseSequence()
    {

    }*/


    public void Anchor(Rigidbody pRb)
    {

    }

    public new void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor

        }
        else
        {

        }
    }
}
