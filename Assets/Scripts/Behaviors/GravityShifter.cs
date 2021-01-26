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
    private float mGravityFactor = 10f;
    public Vector3 mGravNormal { get; private set; }
    private Vector3 mMovementVector;
    private int mIntendedDirection = 1; //might not be necessary to have in this script
    private float mTargetShiftAngle = 0f; //Only here until the camera gets its own script
    private bool mGravityActive;


    // Use this for initialization
    void Start () {
        mRb = GetComponent<Rigidbody>(); //secure this later
        mGravNormal = Vector3.down;
        mCurGravity = Gravity.South;
        mGravityActive = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (mGravityActive)
        {
            mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);
        }
	}

    /*public T ShiftGravity<T>(int pNew) where T : struct
    {
        mRb.velocity = Vector3.zero;
        T[] lArr = (T[])System.Enum.GetValues(typeof(Gravity));
        int j = (int)(mCurGravity + pNew) % 4;

        return lArr[j];
    }*/

    public void ShiftGravity(int pNew)
    {
        mRb.velocity = Vector3.zero;
        mCurGravity = (Gravity)((int)(mCurGravity + pNew) % 4);
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

    private void SetGravityVariables()
    {
        print("Gravity shifter setting variables");
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
    }
}
