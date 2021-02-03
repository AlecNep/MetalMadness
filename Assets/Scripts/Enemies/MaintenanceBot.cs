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

    //Patrol Variables
    private Vector3 mStartingPoint;
    public float mPatrolDistance;
    private float mCounter;
    private float mDistFromStart;
    public enum State {patrolling = 0, seeking = 1, returning = 2 }
    public State mCurState = State.patrolling;

    //References
    public Transform mArms { get; private set; }

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
        mStartingPoint = transform.position;

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(0, 0, DEFAULT_ARM_ROTATION);
    }

    // Update is called once per frame
    void Update () {
        if (mCurState == 0) //Patrolling
        {
            mCounter += Time.deltaTime;
            mDistFromStart = mPatrolDistance * Mathf.Sin(mCounter * mMovementSpeed);

            mIntendedDirection = (int)Mathf.Sign(mDistFromStart);
            Vector3 lTemp = mStartingPoint + _mGravShifter.GetMovementVector() * mDistFromStart;

            transform.position = lTemp;
            if (Mathf.Abs(transform.position.z) > 0.05f)
            {
                transform.position -= Vector3.forward * transform.position.z;
            }
        }
        else if ((int)mCurState == 1) //seeking
        {

        }
        else
        {

        }
    }
}
