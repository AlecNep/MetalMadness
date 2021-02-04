using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceBot : MonoBehaviour {

    //Personal variables
    protected int mIntendedDirection = 1;
    [SerializeField]
    protected float mMovementSpeed;
    protected readonly float DEFAULT_ARM_ROTATION = 90f;
    protected float mBodyRotationSpeed = 15f;
    protected Quaternion mTargetRotation;

    //Patrol Variables
    protected Vector3 mStartingPoint;
    public float mPatrolDistance;
    protected float mCounter;
    protected float mDistFromStart;
    public enum State {patrolling = 0, seeking = 1, attacking = 2, returning = 3 }
    public State mCurState = State.patrolling;
    protected float mPreviousDist = 0;

    //References
    public Transform mArms { get; private set; } //maybe should just be private

    //Tracking variables
    protected GameObject mTarget; //not sure just yet if this should be a Gameobject or Transform
    protected float mTargetDistance;
    protected float mMaxTrackingDistance;
    protected float mAttackDistance;

    //Gravity shifting functionality
    protected GravityShifter _mGravShifter;
    public GravityShifter mGravShifter
    {
        get
        {
            if (_mGravShifter != null)
                return _mGravShifter;
            else
                return null;
        }
    }

    // Use this for initialization
    void Start () {
        _mGravShifter = GetComponent<GravityShifter>();
        if (_mGravShifter == null)
        {
            System.Console.Error.WriteLine("Maintenance bot " + name + "Was not given a GravityShifter component!");
        }
        mStartingPoint = transform.position;

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);
    }

    // Update is called once per frame
    void Update () {
        if (mCurState == 0) //Patrolling
        {
            mCounter += Time.deltaTime;
            float lSineResult = Mathf.Sin(mCounter * mMovementSpeed);
            mIntendedDirection = (int)Mathf.Sign(lSineResult - mPreviousDist);
            mPreviousDist = lSineResult;
            mDistFromStart = mPatrolDistance * lSineResult;

            transform.position = mStartingPoint + _mGravShifter.GetMovementVector() * mDistFromStart;

            if (Mathf.Abs(transform.position.z) > 0.05f)
            {
                transform.position -= Vector3.forward * transform.position.z;
            }
        }
        else if ((int)mCurState == 1) //Seeking
        {
            //perhaps should be in a child class
        }
        else if ((int)mCurState == 2) //About to attack a target
        {
            //perhaps should be in a child class
        }
        else
        {

        }

        mTargetRotation = Quaternion.LookRotation(_mGravShifter.GetMovementVector() * -mIntendedDirection, -_mGravShifter.GetGravityNormal());
        if (transform.rotation != mTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mBodyRotationSpeed);
        }

        if (mTargetDistance > mMaxTrackingDistance)
        {
            mTarget = null;
            mTargetDistance = 0;
            mCurState = State.returning;
        }
    }

    public void SetPatrolPoint(Vector3 pCenter)
    {
        mStartingPoint = pCenter;
    }

    protected virtual void Attack() { }
}
