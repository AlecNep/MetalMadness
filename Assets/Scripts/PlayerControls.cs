using System; //Probably need to get rid of this
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Rigidbody mRb;
    private Camera mCamera;
    private Quaternion mTargetRotation;
    private Transform mArms;
    private readonly float DEFAULT_ARM_ROTATION = 90f;
    private readonly float DEFAULT_ARM_UP = 180f;
    private readonly float DEFAULT_ARM_DOWN = 0f;
    private readonly float ARM_SHIFTING_THRESHOLD = 0.25f; //(float)(Math.Sqrt(2) / 2f);


    private float mTargetShiftAngle = 0f;
    private float mTargetTurnAngle = 0f;
    private float mCurRotation = 0f;
    private float mShiftRotatationSpeed = 10f;
    private float mTurnRotationSpeed = 15f;
    private float mArmRotationSpeed = 20f;

    private float mMovementSpeed = 1.5f;
    private const float mGravShiftDelay = 1.5f; //potentially allow the player to increase this later
    private float mTimer = 0f;
    private bool mCanShift = true;
    private bool mIsGrounded = true;
    private bool mCanDoubleJump = false;
    private float mGravityFactor = 10f;
    public Vector3 mGravNormal { get; private set; }
    private Vector3 mMovementVector;
    public readonly float mJumpforce = 5f;

    public bool mShifting = false;
    public enum Gravity {South = 0, West = 1, North = 2, East = 3};
    private Gravity mCurGravity = Gravity.South;

    private float mDistToGround;

    private Vector3 mDesiredAgnles = Vector3.up * -90;

    //Actual player stats
    private float mHealth = 100f;

	// Use this for initialization
	void Start () {
        mRb = GetComponent<Rigidbody>();
        mCamera = Camera.main;
        mRb.constraints = RigidbodyConstraints.FreezeRotationX;
        mGravNormal = Vector3.down;
        mDistToGround = GetComponent<Collider>().bounds.extents.y;

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);

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
                //mTargetShiftAngle = 90f; //doesn't seem to make a difference
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
                //mTargetShiftAngle = -90f; //doesn't seem to make a difference
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
                        mDesiredAgnles = new Vector3(mTargetTurnAngle, 180, 90);
                        break;
                    case 2: //North
                        mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 180); //
                        break;
                    case 3: //East
                        mDesiredAgnles = new Vector3(-mTargetTurnAngle, 180, -90); //
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
                        mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 0); //
                        break;
                    case 1: //West
                        mDesiredAgnles = new Vector3(mTargetTurnAngle, 180, 90); //
                        break;
                    case 2: //North
                        mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 180);
                        break;
                    case 3: //East
                        mDesiredAgnles = new Vector3(-mTargetTurnAngle, 180, -90);
                        break;
                }
            }

            transform.position += mMovementVector * (lLx * mMovementSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);

        //Arm movement section
        if (Math.Abs(lLy) >= ARM_SHIFTING_THRESHOLD)
        {
            Vector3 lArmRot = lLy > 0 ? 180 * Vector3.right : Vector3.zero;
            
            //The problem was you were using ".roation" instead of ".localRotation
            mArms.localRotation = Quaternion.RotateTowards(mArms.localRotation, Quaternion.Euler(lArmRot), mArmRotationSpeed);
        }
        else
        {
            //The problem was you were using ".roation" instead of ".localRotation
            mArms.localRotation = Quaternion.RotateTowards(mArms.localRotation, Quaternion.Euler(Vector3.right * 90), mArmRotationSpeed);
        }



        //The problem was definitely below! Still not perfect, but it works for now!

        //Temporary, ugly code
        if ((int)mCurGravity % 2 == 1)
        {
            if (mTargetShiftAngle != transform.rotation.eulerAngles.y || transform.rotation.eulerAngles.x != mTargetTurnAngle)
            {
                //mTargetRotation = Quaternion.Euler(mTargetShiftAngle, mTargetTurnAngle, 0f); //rotates around world-y
                //mTargetRotation = Quaternion.Euler(mTargetTurnAngle, mTargetShiftAngle, 0f); //rotates around world-z
                //mTargetRotation = Quaternion.Euler(0f, mTargetTurnAngle, mTargetShiftAngle); //rotates around world-y but is sideways
                //mTargetRotation = Quaternion.Euler(0f, mTargetShiftAngle, mTargetTurnAngle); //rotates around the right axis, but it sideways
                //mTargetRotation = Quaternion.Euler(mTargetTurnAngle, mTargetShiftAngle, mDesiredAgnles.z);
                mTargetRotation = Quaternion.Euler(mDesiredAgnles); //rotates perfectly when moving, but doesn't automatically switch like north and south
                transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mShiftRotatationSpeed);
            }
        }
        else
        {
            if (mTargetShiftAngle != transform.rotation.eulerAngles.x || transform.rotation.eulerAngles.y != mTargetTurnAngle)
            {
                mTargetRotation = Quaternion.Euler(mTargetShiftAngle, mTargetTurnAngle, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mShiftRotatationSpeed);
            }
        }

        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 lTemp = transform.rotation.eulerAngles;

        //Ideally it would look the best if the camera turned with the player, but for now it just needs to work
        mCamera.transform.rotation = Quaternion.Euler(0, 0, mTargetShiftAngle);


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

    }

    public bool IsGrounded()
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
