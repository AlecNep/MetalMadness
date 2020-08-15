using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Bullet {

    //origins will almost certainly need to be calculated based on the parent, not hard-coded
    private Transform[] mSpikeSections;
    private float mBaseSpikeStart = -0.24f;
    private float mBaseSpikeEnd = -0.8f;
    private float mCylinderStart = 0f;
    private float mCylinderEnd = -2f;
    private float mConeStart = 1.255f;
    private float mConeEnd = -0.755f;

    //might need these after all
    private float mBaseDistance = -0.56f;
    private float mCylinderDistance = -2f;
    private float mConeDistance = -2f;
    private Vector3 mBaseGoal;
    private Vector3 mConeGoal;

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

        mExpand = _ExpandSequence();
        mCollapse = _CollapseSequence();

        mBaseGoal = transform.localPosition + mBaseDistance * Vector3.up;
        mConeGoal = mSpikeSections[5].localPosition + mConeDistance * Vector3.up;
    }
    
    public void ExpandSequence()
    {
        StartCoroutine(mExpand);
    }

    private IEnumerator _ExpandSequence()
    {
        mBaseGoal = transform.localPosition + mBaseDistance * Vector3.up;
        mConeGoal = mSpikeSections[5].localPosition + mConeDistance * Vector3.up;

        while (transform.localPosition.x > mBaseSpikeEnd && mSpikeSections[1].localPosition.y > mCylinderEnd
            && mSpikeSections[5].localPosition.y > mConeEnd)
        { //only using the first cylinder section in the condition since they all move the same distance
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, mBaseGoal, mSpeed);

            //This is a fukkn hail-mary of a line haha let's hope this works
            mSpikeSections[1].localPosition = mSpikeSections[2].localPosition = mSpikeSections[3].localPosition = 
                mSpikeSections[4].localPosition = Vector3.MoveTowards(mSpikeSections[1].localPosition, Vector3.up * mCylinderDistance, mSpeed);

            mSpikeSections[5].localPosition = Vector3.MoveTowards(transform.localPosition, mConeGoal, mSpeed);

            yield return null;
        }
        
    }

    public void CollapseSequence()
    {
        StartCoroutine(mCollapse);
    }

    private IEnumerator _CollapseSequence()
    {
        mBaseGoal = transform.localPosition - mBaseDistance * Vector3.up;
        mConeGoal = mSpikeSections[5].localPosition - mConeDistance * Vector3.up;

        while (transform.localPosition.x < mBaseSpikeStart && mSpikeSections[1].localPosition.y < mCylinderStart
            && mSpikeSections[5].localPosition.y < mConeStart)
        { //only using the first cylinder section in the condition since they all move the same distance
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, mBaseGoal, mSpeed);

            //This is also a fukkn hail-mary of a line. Fingers crossed
            mSpikeSections[1].localPosition = mSpikeSections[2].localPosition = mSpikeSections[3].localPosition =
                mSpikeSections[4].localPosition = Vector3.MoveTowards(mSpikeSections[1].localPosition, Vector3.zero, mSpeed);

            mSpikeSections[5].localPosition = Vector3.MoveTowards(transform.localPosition, mConeGoal, mSpeed);

            yield return null;
        }
    }


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
