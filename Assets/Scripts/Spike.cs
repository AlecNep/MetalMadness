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
    private float mConeDistance = -2.01f;
    private Vector3 mBaseGoal;
    private Vector3 mConeGoal;

    private IEnumerator mExpand;
    private IEnumerator mCollapse;

    public bool mExpanding { get; private set; }
    public bool mCollapsing { get; private set; }
    private bool _Extended = false;
    public bool mExtended {
        get
        {
            return _Extended;
        }
        set
        {
            _Extended = value;
            print("Extended: " + _Extended);
        }
    }

    public bool mIsBusy
    {
        get
        {
            return mExpanding || mCollapsing;
        }
    }

    private int TCallCount = 0; //delete later; exclusively here for testing


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

        mExpanding = mCollapsing = false;// = mExtended = false;

        //These lines are probably unnecessary now
        mExpand = _ExpandSequence();
        mCollapse = _CollapseSequence();
    }
    
    public void ExpandSequence()
    {
        StartCoroutine(_ExpandSequence());
    }

    private IEnumerator _ExpandSequence()
    {
        mExpanding = true;
        if (mCollapsing)
        {
            yield return new WaitUntil(() => !mCollapsing);
        }

        mBaseGoal = transform.localPosition + mBaseDistance * Vector3.up;
        mConeGoal = mSpikeSections[5].localPosition + mConeDistance * Vector3.up;

        while (transform.localPosition.y > mBaseSpikeEnd || mSpikeSections[1].localPosition.y > mCylinderEnd
            || mSpikeSections[5].localPosition.y > mConeEnd)
        { //only using the first cylinder section in the condition since they all move the same distance
            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, mBaseGoal, mSpeed);

            mSpikeSections[1].localPosition = mSpikeSections[2].localPosition = mSpikeSections[3].localPosition = 
                mSpikeSections[4].localPosition = Vector3.MoveTowards(mSpikeSections[1].localPosition, Vector3.up * mCylinderDistance, mSpeed);

            mSpikeSections[5].localPosition = Vector3.MoveTowards(mSpikeSections[5].localPosition, mConeGoal, mSpeed);

            if (Mathf.Approximately(transform.localPosition.y, mBaseSpikeEnd) &&
                Mathf.Approximately(mSpikeSections[1].localPosition.y, mCylinderEnd) &&
                Mathf.Approximately(mSpikeSections[5].localPosition.y, mConeEnd))
            {
                transform.localPosition = mBaseGoal;
                mSpikeSections[5].localPosition = mConeGoal;
                break;
            }

            yield return null;
        }
        mExpanding = false;
        mExtended = true;
    }

    public void CollapseSequence()
    {
        StartCoroutine(_CollapseSequence());
    }

    private IEnumerator _CollapseSequence()
    {
        mCollapsing = true;
        if (mExpanding)
        {
            yield return new WaitUntil(() => !mExpanding);
        }

        mBaseGoal = transform.localPosition - mBaseDistance * Vector3.up;
        mConeGoal = mSpikeSections[5].localPosition - mConeDistance * Vector3.up;

        while (transform.localPosition.y < mBaseSpikeStart || mSpikeSections[1].localPosition.y < mCylinderStart
            || mSpikeSections[5].localPosition.y < mConeStart)
        { //only using the first cylinder section in the condition since they all move the same distance

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, mBaseGoal, mSpeed);

            mSpikeSections[1].localPosition = mSpikeSections[2].localPosition = mSpikeSections[3].localPosition =
                mSpikeSections[4].localPosition = Vector3.MoveTowards(mSpikeSections[1].localPosition, Vector3.zero, mSpeed);

            mSpikeSections[5].localPosition = Vector3.MoveTowards(mSpikeSections[5].localPosition, mConeGoal, mSpeed);

            if (Mathf.Approximately(transform.localPosition.y, mBaseSpikeStart) && 
                Mathf.Approximately(mSpikeSections[1].localPosition.y, mCylinderStart) && 
                Mathf.Approximately(mSpikeSections[5].localPosition.y, mConeStart))
            {
                transform.localPosition = mBaseGoal;
                mSpikeSections[5].localPosition = mConeGoal;
                break;
            }

            yield return null;
        }
        mCollapsing = mExtended = false;
        print("done collapsing");
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
            //might not be necessary
        }
    }
}
