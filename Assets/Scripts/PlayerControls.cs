//using System; //Probably need to get rid of this
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Rigidbody mRb;
    private Camera mCamera;
    private Quaternion mTargetRotation;
    public Transform mArms { get; private set; }
    private readonly float DEFAULT_ARM_ROTATION = 90f;
    private readonly float DEFAULT_ARM_UP = 180f;
    private readonly float DEFAULT_ARM_DOWN = 0f;
    private readonly float ARM_SHIFTING_THRESHOLD = 0.25f;


    private float mTargetShiftAngle = 0f;
    private float mTargetTurnAngle = 0f;
    private float mCurRotation = 0f;
    private float mShiftRotatationSpeed = 10f;
    private float mTurnRotationSpeed = 20f;
    private float mArmRotationSpeed = 20f;

    //private int mGravityVariable; //leaving this here in case you want to make the mistake again, BUT it's the same as (int)mCurGravity
    public int mArmVariable;
    public int mTurnVariable; //probably won't be used, BUT exists to set arm variable back when not holding up or down
    public int mShotOrientation
    {
        get
        {
            return 90 * (((int)mCurGravity + mArmVariable) % 4); //adding +2 just made the opposite true for S+W and E+W
        }
    }

    private float mMovementSpeed = 1.5f;
    private const float mGravShiftDelay = 1.5f; //potentially allow the player to increase this later
    private float mTimer = 0f;
    private bool mCanShift = true;
    private bool mIsGrounded = true;
    private bool mCanDoubleJump = false;
    private float mGravityFactor = 10f;
    public Vector3 mGravNormal { get; private set; }
    private Vector3 mMovementVector;
    public readonly float mJumpforce = 6f;

    public bool mShifting = false;
    public enum Gravity {South = 0, East = 1, North = 2, West = 3}; //Swapping east and west didn't seem to change a damn thing
    //but for fuck's sake don't swap north and south
    public Gravity mCurGravity = Gravity.South;

    private float mDistToGround;

    private Vector3 mDesiredAgnles = Vector3.up * -90;

    //Actual player stats
    private float mHealth = 100f;

    public Weapon[] mWeapons; //TEMPORARY; DO NOT KEEP PUBLIC
    public int mWeaponIndex = 0; //TEMPORARY; DO NOT KEEP PUBLIC
    public int mPreviousWeaponIndex = 0; //TEMPORARY
    public int mWeaponCount; //TEMPORARY; DO NOT KEEP PUBLIC

    public GameObject mWeaponWheelCursor; //consider changing this into a RectTransform
    private WeaponSelector mWeaponWheelRef;
    private float mWheelWidth;

    public enum ControlMode {Gameplay = 0, WeaponWheel = 1, Menu = 2, Map = 3 };
    public ControlMode mCurControls = ControlMode.Gameplay;

    // Use this for initialization
    void Start() {
        mRb = GetComponent<Rigidbody>();
        mCamera = Camera.main;
        mRb.constraints = RigidbodyConstraints.FreezeRotationX;
        mGravNormal = Vector3.down;
        mDistToGround = GetComponent<Collider>().bounds.extents.y;

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);

        mWeapons = GetComponentsInChildren<Weapon>();
        mWeaponCount = mWeapons.Length / 2;

        ClearWeapons(); //check later on if this is still necessary
        mWeaponWheelRef = GameObject.Find("Weapon Wheel").GetComponent<WeaponSelector>();
        mWheelWidth = mWeaponWheelRef.GetComponent<RectTransform>().sizeDelta.x;
        mWeaponWheelCursor = mWeaponWheelRef.transform.Find("Cursor").gameObject;

    }


    // Update is called once per frame
    void Update () {
        float lLx = Input.GetAxis("LStickX");
        float lLy = Input.GetAxis("LStickY");
        float lRx = Input.GetAxis("RStickX");
        float lRy = Input.GetAxis("RStickY");

        //probably shouldn't be called every cycle
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
                mTargetShiftAngle = 90f; //doesn't seem to make a difference
                break;
            case 2: //North
                mGravNormal = Vector3.up;
                mMovementVector = Vector3.left;
                mTargetShiftAngle = 180f;
                break;
            case 3: //East
                mGravNormal = Vector3.left;
                mMovementVector = Vector3.down;
                mTargetShiftAngle = -90f; //doesn't seem to make a difference
                break;
            default:
                break;
        }

        //Left stick controls
        if((int)mCurControls < 2) //Can move with the "Gameplay" and "WeaponWheel" modes
        {
            if (lLx != 0)
            {
                if (lLx < 0)
                {   //turn to the relative left
                    mArmVariable = mTurnVariable = 2;
                    mTargetTurnAngle = -90f;

                    //This is going to be some awful code, but I'm at a loss and need it to at least "work" for now
                    switch ((int)mCurGravity)
                    {
                        case 0: //south
                            mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 0);
                            break;
                        case 1: //West
                            mDesiredAgnles = new Vector3(mTargetTurnAngle, 0, 90);
                            break;
                        case 2: //North
                            mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 180); //
                            break;
                        case 3: //East
                            mDesiredAgnles = new Vector3(-mTargetTurnAngle, 0, -90); //
                            break;
                    }
                }
                else
                { //turn to the relative right
                    mArmVariable = mTurnVariable = 0;
                    mTargetTurnAngle = 90f;

                    //This is going to be some awful code, but I'm at a loss and need it to at least "work" for now
                    switch ((int)mCurGravity)
                    {
                        case 0: //south
                            mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 0); //
                            break;
                        case 1: //West
                            mDesiredAgnles = new Vector3(mTargetTurnAngle, 0, 90); //
                            break;
                        case 2: //North
                            mDesiredAgnles = new Vector3(0, mTargetTurnAngle, 180);
                            break;
                        case 3: //East
                            mDesiredAgnles = new Vector3(-mTargetTurnAngle, 0, -90);
                            break;
                    }
                }

                transform.position += mMovementVector * (lLx * mMovementSpeed);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
            mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);

            //Arm movement section
            if (Mathf.Abs(lLy) >= ARM_SHIFTING_THRESHOLD)
            {
                Vector3 lArmRot;
                if (lLy > 0) //Aiming to the relative up
                {
                    lArmRot = 180 * Vector3.right;
                    mArmVariable = 1;
                }
                else //Aiming to the relative down
                {
                    lArmRot = Vector3.zero;
                    mArmVariable = 3;
                }
                mArms.localRotation = Quaternion.RotateTowards(mArms.localRotation, Quaternion.Euler(lArmRot), mArmRotationSpeed);
            }
            else
            {
                mArmVariable = mTurnVariable;
                mArms.localRotation = Quaternion.RotateTowards(mArms.localRotation, Quaternion.Euler(Vector3.right * 90), mArmRotationSpeed);
            }
        }


        //The problem was definitely below! Still not perfect, but it works for now!

        //Temporary, ugly code
        if ((int)mCurGravity % 2 == 1)
        {
            if (mTargetShiftAngle != transform.rotation.eulerAngles.y || transform.rotation.eulerAngles.x != mTargetTurnAngle)
            {
                //Leaving this here so you don't make the same mistakes when you eventually try to optimize this
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
                //Not sure why targetTurnAngle needed to be negative, but it works now
                mTargetRotation = Quaternion.Euler(mTargetShiftAngle, -mTargetTurnAngle, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mShiftRotatationSpeed);
            }
        }

        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 lTemp = transform.rotation.eulerAngles;

        //Ideally it would look the best if the camera turned with the player, but for now it just needs to work
        mCamera.transform.rotation = Quaternion.Euler(0, 0, mTargetShiftAngle);


        //Right stick controls
        if (mCurControls == 0) //can only shift gravity in gameplay mode
        {
            Vector2 lGravInput = new Vector2(lRx, lRy);
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
                    mCurGravity = ShiftGravity<Gravity>(1);
                }
                else if (lGravAngle <= -45f && lGravAngle >= -135f)
                {
                    //Shift gravity to the relative "left"
                    mCurGravity = ShiftGravity<Gravity>(3);
                }
            }
        }
        else if ((int)mCurControls == 1) //Weapon wheel mode
        {
            //using .position sets its starting point to the bottom of the screen
            //but using .localPosition sets its starting point to the top-right corner of the weapon wheel
            //mWeaponWheelCursor.transform.position = new Vector3(lRx, lRy)*mWheelWidth;
            Vector3 lUpRight = Vector3.right + Vector3.up;
            Vector3 lRStick = new Vector3(lRx, lRy);
            mWeaponWheelCursor.transform.localPosition = lUpRight * (-mWheelWidth / 2) + lRStick * (mWheelWidth / 3f);
            if (lRStick.magnitude > 0.5f) //totally arbitrary number right now
            {
                mWeaponWheelRef.Selector(lRStick);
            }
            
        }
    }

    public void ChangeControlMode(int mMode)
    {
        mCurControls = (ControlMode)mMode;
    }

    public void ClearWeapons()
    {
        for (int i = 0; i < mWeaponCount * 2; i++)
        {
            if (i != mWeaponIndex && i != mWeaponIndex + mWeaponCount) //originally 2 instead of weapon count
            {
                mWeapons[i].gameObject.SetActive(false);
            }
            else mWeapons[i].gameObject.SetActive(true);
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
