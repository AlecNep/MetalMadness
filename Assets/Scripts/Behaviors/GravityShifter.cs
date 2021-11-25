using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShifter : MonoBehaviour {

    private Rigidbody mRb;
    public enum Gravity { South = 0, East = 1, North = 2, West = 3 };
    private Gravity _mCurGravity = Gravity.South;
    public Gravity mCurGravity 
    {
        get
        {
            return _mCurGravity;
        }
        set
        {
            _mCurGravity = value;
            SetGravityVariables();
        }
    }

    [SerializeField]
    private int defaultGravity;

    [SerializeField]
    private float mGravityFactor = 10f;
    public Vector3 mGravNormal { get; private set; }
    private Vector3 mMovementVector;
    private float mTargetShiftAngle = 0f; //Only here until the camera gets its own script
    private bool mGravityActive;
    private bool mGravLocked = false;


    private void OnValidate()
    {
        if (defaultGravity < 0 || defaultGravity > 3)
        {
            defaultGravity = 0;
        }
    }

    // Use this for initialization
    void Start () {
        mRb = GetComponent<Rigidbody>(); //secure this later
        mGravityActive = true;
        mCurGravity = (Gravity)defaultGravity;
        SetGravityVariables();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (mGravityActive)
        {
            mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);
        }
	}

    public void ForceGravity(int pDirection)
    {
        if ((int)mCurGravity != pDirection)
            mRb.velocity = Vector3.zero;
        mCurGravity = (Gravity)pDirection;
        Lock(true);
    }

    //Might not need to be public; but I'll leave it that way for now
    public void Lock(bool locked)
    {
        mGravLocked = locked;
    }

    public bool IsLocked()
    {
        return mGravLocked;
    }

    public void GravityIsActive(bool pActive)
    {
        mGravityActive = pActive;
    }

    public Vector3 GetMovementVector()
    {
        return mMovementVector;
    }

    public Vector3 GetGravityNormal()
    {
        return mGravNormal;
    }

    public float GetShiftAngle()
    {
        return mTargetShiftAngle;
    }

    public void ShiftGravity(int pNew)
    {
        mRb.velocity = Vector3.zero;
        mCurGravity = (Gravity)((int)(mCurGravity + pNew) % 4);
        SetGravityVariables();
    }

    private void SetGravityVariables()
    {
        if (mGravLocked)
        {
            Debug.Log(name + " cannot shift gravity due to being grav-locked");
        }
        switch ((int)mCurGravity)
        {
            case 0: //South (normal gravity)
                mGravNormal = Vector3.down;
                mMovementVector = Vector3.right;
                mTargetShiftAngle = 0f;
                break;
            case 1: //East
                mGravNormal = Vector3.right;
                mMovementVector = Vector3.up;
                mTargetShiftAngle = 90f;
                break;
            case 2: //North
                mGravNormal = Vector3.up;
                mMovementVector = Vector3.left;
                mTargetShiftAngle = 180f;
                break;
            case 3: //West
                mGravNormal = Vector3.left;
                mMovementVector = Vector3.down;
                mTargetShiftAngle = -90f;
                break;
            default:
                break;
        }
    }
}
