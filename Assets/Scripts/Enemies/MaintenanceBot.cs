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
    public float mLength;
    private float mCounter;
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

        switch ((int)mCurState)
        {
            case 0: //Casually patrolling

                break;
            case 1: //Seeking a target
                break;
            case 2: //Returning to patroll path
                break;
        }
    }
}
