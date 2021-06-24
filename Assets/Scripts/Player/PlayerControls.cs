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

    private float mBodyRotationSpeed = 20f;
    private float mArmRotationSpeed = 20f;

    public int mArmVariable;
    public int mTurnVariable;


    //Movement and speed
    [SerializeField]
    private float mMovementSpeed = 2f;
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
    private float mChargeEnergy;

    //Gravity stuff
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

    public int mShotOrientation
    {
        get
        {
            return 90 * (((int)_mGravShifter.mCurGravity + mArmVariable) % 4);
        }
    }

    private const float mGravShiftDelay = 1.5f; //potentially allow the player to reduce this later
    private float shiftTimer = 0f;
    private bool mCanShift = true;
    

    private int mIntendedDirection = 1; 
    [SerializeField]
    public float mJumpForce;
    [SerializeField]
    public float mChargedJumpForce;
    private bool mOnMovingObject; //used for when the player is on top of another moving object
    private float mZDistance = 0f;

    public bool mShifting = false;
    private float mDistToGround;

    //Actual player stats
    private const float DEFAULT_HEALTH = 100f;
    public float maxHealth { get; private set; }
    public float health { get; private set; }

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
        health = maxHealth = DEFAULT_HEALTH; //TODO: make this more secure later!
        mRb = GetComponent<Rigidbody>(); //secure this later
        _mGravShifter = GetComponent<GravityShifter>(); //secure this later
        mCamera = Camera.main;
        mDistToGround = GetComponent<Collider>().bounds.extents.y; //secure this later

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);

        mWeapons = GetComponentsInChildren<Weapon>();
        mWeaponCount = mWeapons.Length / 2;

        ClearWeapons(); //check later on if this is still necessary
        mWeaponWheelRef = GameObject.Find("Weapon Wheel").GetComponent<WeaponSelector>();
        mWheelWidth = mWeaponWheelRef.GetComponent<RectTransform>().sizeDelta.x;
        mWeaponWheelCursor = mWeaponWheelRef.transform.Find("Cursor").gameObject;
    }


    private void Update() //doesn't seem to matter if it's regular or Late
    {
        mTargetRotation = Quaternion.LookRotation(_mGravShifter.GetMovementVector() * -mIntendedDirection, -_mGravShifter.GetGravityNormal());

        if (transform.rotation != mTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mBodyRotationSpeed);
        }

        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        mCamera.transform.rotation = Quaternion.Euler(0, 0, _mGravShifter.GetShiftAngle());
    }

    
    void FixedUpdate () {

        float lLx = Input.GetAxis("LStickX");
        float lLy = Input.GetAxis("LStickY");
        float lRx = Input.GetAxis("RStickX");
        float lRy = Input.GetAxis("RStickY");


        //Left stick controls
        if((int)mCurControls < 2) //Can move with the "Gameplay" and "WeaponWheel" modes
        {
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
                    {   //turn to the relative right
                        mIntendedDirection = 1;
                        mArmVariable = mTurnVariable = 0;
                    }
                }

                LayerMask lIgnoreLayers = ~(1 << 8 & 1 << 9);
                if (!Physics.Raycast(transform.position, mGravShifter.GetMovementVector() * mIntendedDirection, 0.7f, lIgnoreLayers))
                {
                    if (IsDashing())
                    {
                        //following lines are reduced by 1/10th because of the left stick sensitivity
                        float lDashSpeed = CommandPattern.OverCharge.mCharged ? mChargedDashSpeed : mDashSpeed;
                        transform.position += 0.1f * mIntendedDirection * mGravShifter.GetMovementVector() * lDashSpeed;
                    }
                    else
                    {
                        transform.position += mGravShifter.GetMovementVector() * (lLx * mMovementSpeed);
                    }
                }
                else
                {
                    print("woah mane, goofy shit happening");
                }

                mZDistance = transform.position.z;
                if (Mathf.Abs(mZDistance) > 0.05f)
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


        //Right stick controls
        if (mCurControls == 0) //can only shift gravity in gameplay mode; cannot shift if attached to anything
        {
            Vector2 lGravInput = new Vector2(lRx, lRy);
            float lGravAngle = 0f;


            if (shiftTimer > 0)
            {
                shiftTimer -= Time.deltaTime;
                if (shiftTimer < 0)
                {
                    shiftTimer = 0;
                }
            }
            

            if (lGravInput.magnitude > 0.1f && CanShift() && !mAttached)
            {
                mCanShift = false;
                shiftTimer = mGravShiftDelay;

                lGravAngle = Vector2.Angle(Vector2.up, lGravInput);
                Vector3 cross = Vector3.Cross(Vector2.up, lGravInput);

                if (cross.z > 0)
                    lGravAngle = -lGravAngle;

                if (lGravAngle > -45f && lGravAngle <= 45f)
                {
                    //Shift gravity to the relative "up"
                    _mGravShifter.ShiftGravity(2);
                }
                else if (lGravAngle > 45f && lGravAngle <= 135f)
                {
                    //Shift gravity to the relative "right"
                    _mGravShifter.ShiftGravity(1);
                }
                else if (lGravAngle <= -45f && lGravAngle >= -135f)
                {
                    //Shift gravity to the relative "left"
                    _mGravShifter.ShiftGravity(3);
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

    public void ChangeControlMode(int mMode) //consider overridding this method for more than just int input
    {
        mCurControls = (ControlMode)mMode;
        mGravShifter.GravityIsActive((int)mCurControls < 2);
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

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetShiftDelay()
    {
        return mGravShiftDelay;
    }

    public float GetShiftTimer()
    {
        return mGravShiftDelay - shiftTimer;
    }

    public float GetDashDelay()
    {
        return mDashDelay;
    }

    public float GetDashTimer()
    {
        return mDashDelay - mDashTimer;
    }

    public bool CanShift()
    {
        return shiftTimer == 0;
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
