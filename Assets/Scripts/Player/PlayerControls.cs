//using System; //Probably need to get rid of this
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : Damageable {

    public Rigidbody mRb { get; private set; }
    private Camera mCamera;
    private Quaternion mTargetRotation;
    public Transform mArms { get; private set; }
    private readonly float DEFAULT_ARM_ROTATION = 90f;
    private readonly float ARM_SHIFTING_THRESHOLD = 0.25f;

    private float mBodyRotationSpeed = 20f;
    private float mArmRotationSpeed = 20f;

    public int mArmVariable;
    public int mTurnVariable;
    public Interactive interactableObject { get; set; }

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
            if (_mGravShifter == null)
            {

                _mGravShifter = gameObject.AddComponent<GravityShifter>();
            }
            return _mGravShifter;
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

        //Need to slowly move this to a better class
        mWeaponWheelRef = GameManager.Instance.UI.transform.Find("Weapon Wheel").GetComponent<WeaponSelector>();
        mWheelWidth = mWeaponWheelRef.GetComponent<RectTransform>().sizeDelta.x;
        mWeaponWheelCursor = mWeaponWheelRef.transform.Find("Cursor").gameObject;
    }


    private void Update() 
    {
        mTargetRotation = Quaternion.LookRotation(mGravShifter.GetMovementVector() * -mIntendedDirection, -mGravShifter.GetGravityNormal());

        if (transform.rotation != mTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mBodyRotationSpeed);
        }

        mCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 3, -20);
        mCamera.transform.rotation = Quaternion.Euler(0, 0, _mGravShifter.GetShiftAngle());
    }

    
    void FixedUpdate () {

        float lLx = Input.GetAxis("LStickX");
        float lLy = Input.GetAxis("LStickY");
        float lRx = Input.GetAxis("RStickX");
        float lRy = Input.GetAxis("RStickY");


        //Left stick controls
        if((int)GameManager.currentGameMode < 2) //Can move with the "Gameplay" and "WeaponWheel" modes
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

                LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1<< 15; //environment, enemies, destructible, and doors
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, mGravShifter.GetMovementVector() * mIntendedDirection, out hit, 0.7f, layers))
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


                mZDistance = transform.position.z;
                if (Mathf.Abs(mZDistance) > 0.05f)
                {
                    transform.position -= Vector3.forward * mZDistance;
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
        if (GameManager.currentGameMode == 0) //can only shift gravity in gameplay mode;
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
                lGravAngle = Vector2.Angle(Vector2.up, lGravInput);
                Vector3 cross = Vector3.Cross(Vector2.up, lGravInput);

                if (cross.z > 0)
                    lGravAngle = -lGravAngle;
                
                if (Mathf.Abs(lGravAngle) < 135f) //not downwards
                {
                    mCanShift = false;
                    shiftTimer = mGravShiftDelay;

                    if (lGravAngle > -45f && lGravAngle <= 45f)
                    {
                        //Shift gravity to the relative "up"
                        mGravShifter.ShiftGravity(2);
                    }
                    else if (lGravAngle > 45f && lGravAngle <= 135f)
                    {
                        //Shift gravity to the relative "right"
                        mGravShifter.ShiftGravity(1);
                    }
                    else if (lGravAngle <= -45f && lGravAngle >= -135f)
                    {
                        //Shift gravity to the relative "left"
                        mGravShifter.ShiftGravity(3);
                    }
                }
                
            }
        }
        else if ((int)GameManager.currentGameMode == 1) //Weapon wheel mode
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

    public void SetWeapons(int newWeaponIndex)
    {
        mPreviousWeaponIndex = mWeaponIndex;
        mWeaponIndex = newWeaponIndex;
        ClearWeapons();
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
        return !mGravShifter.IsLocked() && shiftTimer == 0;
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

    public void RefillGravity()
    {
        shiftTimer = 0;
    }

    public void RefillDash()
    {
        mDashTimer = 0;
    }

    public override void Die()
    {
        //All temporary code!
        mGravShifter.ShiftGravity(4 - (int)mGravShifter.mCurGravity);
        transform.position = Vector3.zero;
        mRb.velocity = Vector3.zero;
        health = maxHealth;
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
