using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceBot : MonoBehaviour {

    //Personal variables
    private int mIntendedDirection = 1;
    [SerializeField]
    private float mMovementSpeed;
    private readonly float DEFAULT_ARM_ROTATION = 90f;
    private float mBodyRotationSpeed = 15f;
    private Quaternion mTargetRotation;

    //Patrol Variables
    private Vector3 mStartingPoint;
    public float mPatrolDistance;
    private float mCounter;
    private float mDistFromStart;
    public enum State {patrolling = 0, seeking = 1, attacking = 2, returning = 3 }
    public State mCurState = State.patrolling;
    private float mPreviousDist = 0;

    //References
    public Transform mArms { get; private set; } //maybe should just be private
    private Transform mTarget;
    private float mMaxTrackingDistance;

    //Gravity shifting functionality
    private GravityShifter _mGravShifter;
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

        }
        else if ((int)mCurState == 2) //About to attack a target
        {

        }
        else
        {

        }

        mTargetRotation = Quaternion.LookRotation(_mGravShifter.GetMovementVector() * -mIntendedDirection, -_mGravShifter.GetGravityNormal());
        if (transform.rotation != mTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mBodyRotationSpeed);
        }
    }

    public void SetPatrolPoint(Vector3 pCenter)
    {
        mStartingPoint = pCenter;
    }
}
