using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceBot : MonoBehaviour {

    //Personal variables
    public Vector3 mGravNormal { get; private set; }
    private Vector3 mMovementVector;
    private int mIntendedDirection = 1;
    [SerializeField]
    private float mMovementSpeed;
    private readonly float DEFAULT_ARM_ROTATION = 90f;

    //Patrol Variables
    private Vector3 mStartingPoint;
    public float mPatrolDistance;
    private float mCounter;
    private float mDistFromStart;
    public enum State {patrolling = 0, seeking = 1, returning = 2 }
    public State mCurState = State.patrolling;

    //References
    public Transform mArms { get; private set; }
    private Rigidbody mRb;

    //Physics variables
    private float mGravityFactor = 10f;


    // Use this for initialization
    void Start () {
        mGravNormal = Vector3.down;

        mRb = GetComponent<Rigidbody>();

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(0, 0, DEFAULT_ARM_ROTATION);
    }

    // Update is called once per frame
    void Update () {
        mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal); //maybe move somewhere else

        
    }

    /*private void SetGravityVariables()
    {
        switch ((int)mCurGravity)
        {
            case 0: //South (normal gravity)
                mGravNormal = Vector3.down;
                mMovementVector = Vector3.right;
                mTargetShiftAngle = 0f;
                break;
            case 1: //West
                mGravNormal = Vector3.right;
                mMovementVector = Vector3.up;
                mTargetShiftAngle = 90f;
                break;
            case 2: //North
                mGravNormal = Vector3.up;
                mMovementVector = Vector3.left;
                mTargetShiftAngle = 180f;
                break;
            case 3: //East
                mGravNormal = Vector3.left;
                mMovementVector = Vector3.down;
                mTargetShiftAngle = -90f;
                break;
            default:
                break;
        }
    }*/
}
