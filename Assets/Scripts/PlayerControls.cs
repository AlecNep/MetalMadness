using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Rigidbody mRb;
    private Camera mCamera;
    private Quaternion mTargetRotation;

    private Vector3 mOldRotation;
    private float mTargetShiftAngle = 0f;
    private float mTargetTurnAngle = 0f;
    private float mCurRotation = 0f;
    private float mShiftRotatationSpeed = 10f;
    private float mTurnRotationSpeed = 15f;

    private float mMovementSpeed = 1.5f;
    private const float mGravShiftDelay = 1.5f;
    private float mTimer = 0f;
    private bool mCanShift = true;
    private bool mIsGrounded = true;
    private float mGravityFactor = 10f;
    private Vector3 mGravNormal;
    private float mJumpforce = 4f;

    public bool mShifting = false;
    public enum Gravity {South = 0, West = 1, North = 2, East = 3};
    private Gravity mCurGravity = Gravity.South;

    private float mDistToGround;

	// Use this for initialization
	void Start () {
        mRb = GetComponent<Rigidbody>();
        mCamera = Camera.main;
        mRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        mGravNormal = mOldRotation = Vector3.down;
        mDistToGround = GetComponent<Collider>().bounds.extents.y;
	}

    private void FixedUpdate()
    {
        switch ((int)mCurGravity)
        {
            case 0:
                mGravNormal = Vector3.down;
                mTargetShiftAngle = 0f;
                break;
            case 1:
                mGravNormal = Vector3.left;
                mTargetShiftAngle = -90f;
                break;
            case 2:
                mGravNormal = Vector3.up;
                mTargetShiftAngle = 180f;
                break;
            case 3:
                mGravNormal = Vector3.right;
                mTargetShiftAngle = 90f;
                break;
            default:
                break;
        }
        mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);
        
        if (mTargetShiftAngle != transform.rotation.eulerAngles.z)
        {
            mTargetRotation = Quaternion.Euler(0, 0, mTargetShiftAngle);
            transform.rotation = mCamera.transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mShiftRotatationSpeed);
        }
        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    // Update is called once per frame
    void Update () {
        float lLx = Input.GetAxis("LStickX");
        float lLy = Input.GetAxis("LStickY");

        

        if (lLx != 0)
        {
            if (lLx < 0)
            {
                mTargetTurnAngle = 90f;
            }
            else
            {
                mTargetTurnAngle = -90f;
            }
            transform.position += transform.right * (lLx * mMovementSpeed);
            //mRb.AddForce(transform.right * (lLx * mMovementSpeed), ForceMode.Force);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        if (transform.rotation.eulerAngles.y != mTargetTurnAngle)
        {
            
        }

        Vector2 lGravDir = new Vector2(Input.GetAxis("RStickX"), Input.GetAxis("RStickY"));
        float lGravAngle = 0f;

        
        if (mTimer < mGravShiftDelay && !mCanShift)
        {
            mTimer += Time.deltaTime;
        }
        if (mTimer >= mGravShiftDelay)
        {
            mTimer = 0;
            mCanShift = true;
        }
            

        if (lGravDir.magnitude > 0.1f && mCanShift)
        {
            mCanShift = false;
            
            lGravAngle = Vector2.Angle(Vector2.up, lGravDir);
            Vector3 cross = Vector3.Cross(Vector2.up, lGravDir);

            if (cross.z > 0)
                lGravAngle = -lGravAngle;

            if (lGravAngle > -45f && lGravAngle <= 45f)
            {
                //Shift gravity to the relative "up"
                mCurGravity = ShiftGravity<Gravity>(2);
            }
            else if (lGravAngle > 45f && lGravAngle <= 135f)
            {
                //Shift gravity to the relative "right"
                mCurGravity = ShiftGravity<Gravity>(3);
            }
            else if (lGravAngle <= -45f && lGravAngle >= -135f)
            {
                //Shift gravity to the relative "left"
                mCurGravity = ShiftGravity<Gravity>(1);
            }
        }

        if (Input.GetButtonDown("AButton"))
        {
            if (IsGrounded())
            {
                mRb.AddForce(mJumpforce * transform.up, ForceMode.Impulse);
            }
        }

        /*if (mShifting)
        {
            mRb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            mRb.constraints &= RigidbodyConstraints.FreezeRotationZ;
        }*/
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, mDistToGround + 0.1f);
    }

    public T ShiftGravity<T>(int mNew) where T: struct
    {
        T[] arr = (T[])System.Enum.GetValues(typeof(Gravity));
        int j = (int)(mCurGravity + mNew) % 4;
        return arr[j];
    }
}
