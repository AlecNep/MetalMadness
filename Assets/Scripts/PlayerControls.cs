using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Rigidbody mRb;
    private Camera mCamera;
    private Quaternion mTargetRotation;

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
    private Vector3 mMovementVector;
    private float mJumpforce = 5f;

    public bool mShifting = false;
    public enum Gravity {South = 0, West = 1, North = 2, East = 3};
    private Gravity mCurGravity = Gravity.South;

    private float mDistToGround;

    private Vector3 mDesiredAgnles = Vector3.up * -90;

	// Use this for initialization
	void Start () {
        mRb = GetComponent<Rigidbody>();
        mCamera = Camera.main;
        mRb.constraints = RigidbodyConstraints.FreezeRotationX;
        mGravNormal = Vector3.down;
        mDistToGround = GetComponent<Collider>().bounds.extents.y;
	}

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update () {
        float lLx = Input.GetAxis("LStickX");
        float lLy = Input.GetAxis("LStickY");

        

        switch ((int)mCurGravity)
        {
            case 0: //South (normal gravity)
                mGravNormal = Vector3.down;
                mMovementVector = Vector3.right;
                mTargetShiftAngle = 0f;
                break;
            case 1: //West
                mGravNormal = Vector3.left;
                mMovementVector = Vector3.down;
                mTargetShiftAngle = -90f;
                break;
            case 2: //North
                mGravNormal = Vector3.up;
                mMovementVector = Vector3.left;
                mTargetShiftAngle = 180f;
                break;
            case 3: //East
                mGravNormal = Vector3.right;
                mMovementVector = Vector3.up;
                mTargetShiftAngle = 90f;
                break;
            default:
                break;
        }

        if (lLx != 0)
        {
            if (lLx < 0)
            { //turn to the relative left
                
                mTargetTurnAngle = 90f;

                //This is going to be some awful code, but I'm at a loss and need it to at least "work" for now
                switch ((int)mCurGravity)
                {
                    case 0: //south
                        mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 0);
                        break;
                    case 1: //West
                        mDesiredAgnles = new Vector3(90, mTargetTurnAngle, 0);
                        break;
                    case 2: //North
                        mDesiredAgnles = new Vector3(180, mTargetTurnAngle, 0);
                        break;
                    case 3: //East
                        mDesiredAgnles = new Vector3(-90, mTargetTurnAngle, 0);
                        break;
                }
            }
            else
            { //turn to the relative right
                mTargetTurnAngle = -90f;

                //This is going to be some awful code, but I'm at a loss and need it to at least "work" for now
                switch ((int)mCurGravity)
                {
                    case 0: //south
                        mDesiredAgnles = new Vector3(0, -mTargetTurnAngle, 0);
                        break;
                    case 1: //West
                        mDesiredAgnles = new Vector3(-90, -mTargetTurnAngle, 0);
                        break;
                    case 2: //North
                        mDesiredAgnles = new Vector3(180, -mTargetTurnAngle, 0);
                        break;
                    case 3: //East
                        mDesiredAgnles = new Vector3(90, -mTargetTurnAngle, 0);
                        break;
                }
            }
            transform.position += mMovementVector * (lLx * mMovementSpeed);
            //mRb.AddForce(transform.right * (lLx * mMovementSpeed), ForceMode.Force);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);

        if (mTargetShiftAngle != transform.rotation.eulerAngles.x || transform.rotation.eulerAngles.y != mTargetTurnAngle)
        {
            mTargetRotation = Quaternion.Euler(mTargetShiftAngle, mTargetTurnAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mShiftRotatationSpeed);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(mDesiredAgnles), mShiftRotatationSpeed);


            //transform.Rotate(new Vector3(mTargetShiftAngle, mTargetTurnAngle, 0), Space.Self);
            Vector3 lTemp = transform.rotation.eulerAngles;
            //mCamera.transform.rotation = Quaternion.Euler(0, 0, temp.z);
            mCamera.transform.rotation = Quaternion.Euler(0, 0, lTemp.x);
        }
        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        

        Vector2 lGravInput = new Vector2(Input.GetAxis("RStickX"), Input.GetAxis("RStickY"));
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
            

        if (lGravInput.magnitude > 0.1f && mCanShift)
        {
            mCanShift = false;
            
            lGravAngle = Vector2.Angle(Vector2.up, lGravInput);
            Vector3 cross = Vector3.Cross(Vector2.up, lGravInput);

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
                //mRb.AddForce(mJumpforce * transform.up, ForceMode.Impulse); //original
                mRb.AddForce(mJumpforce * (-mGravNormal), ForceMode.Impulse);
            }
        }

    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, mDistToGround + 0.1f);
    }

    public T ShiftGravity<T>(int mNew) where T: struct
    {
        T[] arr = (T[])System.Enum.GetValues(typeof(Gravity));
        int j = (int)(mCurGravity + mNew) % 4;
        print(arr[j]);
        return arr[j];
    }
}
