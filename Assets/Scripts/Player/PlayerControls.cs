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
    private readonly float ARM_SHIFTING_THRESHOLD = 0.25f;

    private float mTargetShiftAngle = 0f; //Only here until the camera gets its own script
    private float mBodyRotationSpeed = 20f; //Seems to be in charge of shifting and turning
    private float mArmRotationSpeed = 20f;

    public int mArmVariable;
    public int mTurnVariable;

    public int mShotOrientation
    {
        get
        {
            return 90 * (((int)mCurGravity + mArmVariable) % 4);
        }
    }

    //Movement and speed
    private float mMovementSpeed = 1.5f;
    public float mDashSpeed;
    public float mChargedDashSpeed;
    public float mDashTimer = 0;
    public float mDashDuration;
    public float mDashDelayBuffer;
    public float mDashDelay
    {
        get
        {
            return mDashDuration + mDashDelayBuffer;
        }
    }
    private bool mAttachedToWall = false;
    private bool mAttachedToEnemy = false;
    public bool mAttached
    {
        get
        {
            return mAttachedToEnemy || mAttachedToWall;
        }
    }

    //Overcharge
    private bool mChargeActive = false; //might not be neccessary, already in CommandPattern
    private float mChargeEnergy;

    //Gravity stuff
    public GravityShifter mGravShifter;
    private const float mGravShiftDelay = 1.5f; //potentially allow the player to reduce this later
    private float mTimer = 0f;
    private bool mCanShift = true;
    private bool mCanDoubleJump = false; //might be useless if overcharged jump is implemented
    private float mGravityFactor = 10f; //not sure why this is 10; investigate later
    public Vector3 mGravNormal;
    /*public Vector3 mGravNormal
    {
        get
        {
            if (mGravShifter != null)
            {
                return mGravShifter.GetGravityNormal();
            }
            else return Vector3.down;
        }
    }*/
    private Vector3 mMovementVector;
    private int mIntendedDirection = 1; 
    public readonly float mJumpforce = 6f;
    public readonly float mChargedJumpForce = 10f;
    private bool mOnMovingObject; //used for when the player is on top of another moving object
    private float mZDistance = 0f;

    public bool mShifting = false;
    public enum Gravity {South = 0, East = 1, North = 2, West = 3};
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




    private float mDistToGround;

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
        mRb = GetComponent<Rigidbody>(); //secure this later
        mGravShifter = GetComponent<GravityShifter>(); //secure this later
        mCamera = Camera.main;
        mGravNormal = Vector3.down;
        mDistToGround = GetComponent<Collider>().bounds.extents.y; //secure this later
        mCurGravity = Gravity.South;

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


        //Left stick controls
        if((int)mCurControls < 2) //Can move with the "Gameplay" and "WeaponWheel" modes
        {
            //Find a way to move this code into the gravity shifter script without needing to access the PlayerControls script
            //mRb.AddForce(mGravityFactor * mRb.mass * mGravNormal);

            //Main movement section
            if (!mAttachedToWall) //cannot move if attached to a wall
            {
                if (lLx != 0)
                {
                    if (lLx < 0)
                    {   //turn to the relative left
                        mIntendedDirection = -1;
                        mArmVariable = mTurnVariable = 2;
                    }
                    else
                    { //turn to the relative right
                        mIntendedDirection = 1;
                        mArmVariable = mTurnVariable = 0;
                    }
                }

                if (IsDashing())
                {
                    //following lines are reduced by 1/10th because of the left stick sensitivity
                    /*if (CommandPattern.OverCharge.mCharged)// && mChargeEnergy > amountNeeded)
                    {
                        transform.position += 0.1f * mIntendedDirection * mMovementVector * mChargedDashSpeed;
                    }
                    else
                    {
                        transform.position += 0.1f * mIntendedDirection * mMovementVector * mDashSpeed;
                    }*/
                    float lDashSpeed = CommandPattern.OverCharge.mCharged ? mChargedDashSpeed : mDashSpeed;
                    transform.position += 0.1f * mIntendedDirection * mGravShifter.GetMovementVector() * lDashSpeed;
                }
                else
                {
                    //transform.position += mMovementVector * (lLx * mMovementSpeed);
                    transform.position += mGravShifter.GetMovementVector() * (lLx * mMovementSpeed);
                }

                mZDistance = transform.position.z;
                if (mZDistance > 0.05f)
                {
                    //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    transform.position -= Vector3.forward * mZDistance;
                    //mZDistance = 0;
                }
                //End main movement section

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
                //end arm movement section
            }
            

            if (mDashTimer > 0)
            {
                mDashTimer -= Time.deltaTime;
                if (mDashTimer < 0)
                {
                    mDashTimer = 0;
                }
            }
        }

        mTargetRotation = Quaternion.LookRotation(mGravShifter.GetMovementVector() * -mIntendedDirection, -mGravShifter.GetGravityNormal());

        if (transform.rotation != mTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mBodyRotationSpeed);
        }

        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        //Ideally it would look the best if the camera turned with the player, but for now it just needs to work
        mCamera.transform.rotation = Quaternion.Euler(0, 0, mGravShifter.GetShiftAngle());


        //Right stick controls
        if (mCurControls == 0) //can only shift gravity in gameplay mode; cannot shift if attached to anything
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

            if (lGravInput.magnitude > 0.1f && mCanShift && !mAttached)
            {
                mCanShift = false;

                lGravAngle = Vector2.Angle(Vector2.up, lGravInput);
                Vector3 cross = Vector3.Cross(Vector2.up, lGravInput);

                if (cross.z > 0)
                    lGravAngle = -lGravAngle;

                if (lGravAngle > -45f && lGravAngle <= 45f)
                {
                    //Shift gravity to the relative "up"
                    //mCurGravity = ShiftGravity<Gravity>(2);
                    mGravShifter.ShiftGravity(2);
                }
                else if (lGravAngle > 45f && lGravAngle <= 135f)
                {
                    //Shift gravity to the relative "right"
                    //mCurGravity = ShiftGravity<Gravity>(1);
                    mGravShifter.ShiftGravity(1);
                }
                else if (lGravAngle <= -45f && lGravAngle >= -135f)
                {
                    //Shift gravity to the relative "left"
                    //mCurGravity = ShiftGravity<Gravity>(3);
                    mGravShifter.ShiftGravity(3);
                }
            }
        }
        else if ((int)mCurControls == 1) //Weapon wheel mode
        {
            Vector3 lUpRight = Vector3.right + Vector3.up;
            Vector3 lRStick = new Vector3(lRx, lRy);
            mWeaponWheelCursor.transform.localPosition = lUpRight * (-mWheelWidth / 2) + lRStick * (mWheelWidth / 3f);
            if (lRStick.magnitude > 0.5f) //totally arbitrary number right now
            {
                mWeaponWheelRef.Selector(lRStick);
            }
            
        }
    } //~~~~~~end Update~~~~~~

    public void ChangeControlMode(int mMode)
    {
        mCurControls = (ControlMode)mMode;
        mGravShifter.GravityIsActive(mCurControls == ControlMode.Gameplay);
    }

    public void ClearWeapons()
    {
        for (int i = 0; i < mWeaponCount * 2; i++)
        {
            if (i != mWeaponIndex && i != mWeaponIndex + mWeaponCount)
            {
                mWeapons[i].gameObject.SetActive(false);
            }
            else mWeapons[i].gameObject.SetActive(true);
        }
    }

    public bool CanDash()
    {
        return mDashTimer == 0;
    }

    public bool IsDashing()
    {
        return mDashDelay - mDashTimer <= mDashDuration;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, mDistToGround + 0.1f);
    }

    public T ShiftGravity<T>(int pNew) where T: struct
    {
        mRb.velocity = Vector3.zero;
        T[] lArr = (T[])System.Enum.GetValues(typeof(Gravity));
        int j = (int)(mCurGravity + pNew) % 4;
        
        return lArr[j];
    }

    private void SetGravityVariables()
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
    }

    private void DetachFromMovingObject()
    {
        if (mOnMovingObject)
        {
            mOnMovingObject = false;
            transform.SetParent(null, true);
        }
        
    }

    public void SpikeAttached(string pTag)
    {
        if (pTag == "Enemy")
        {
            mAttachedToEnemy = true;
        }
        else if (pTag == "Environment")
        {
            mAttachedToWall = true;
        }
        else
        {
            throw new System.Exception("Error: Player attached to invalid object");
        }
    }

    public void SpikeDetached()
    {
        mAttachedToWall = mAttachedToEnemy = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        GameObject lGO = col.gameObject;
        if (lGO.tag == "MovingPlatform")
        {
            if (!mOnMovingObject)
            {
                mOnMovingObject = true;
                transform.SetParent(lGO.transform, true);
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        GameObject lGO = col.gameObject;
        if (lGO.tag == "MovingPlatform")
        {
            DetachFromMovingObject();
        }
    }
}
