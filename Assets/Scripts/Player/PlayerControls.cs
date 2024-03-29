﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerControls : Damageable, ISaveable {

    public Rigidbody mRb { get; private set; }
    private Camera mCamera;
    private Quaternion mTargetRotation;
    public Transform mArms { get; private set; }
    private readonly float DEFAULT_ARM_ROTATION = 90f;
    private readonly float DEFAULT_AIMING_THRESHOLD = 0.25f;
    private static bool _aimThreshSet = false;
    private static float _aimThresh;
    public static float armAimThreshold
    {
        get
        {
            return _aimThresh;
        }
        set
        {
            _aimThresh = value;
            if (!_aimThreshSet)
                _aimThreshSet = true;
        }
    }

    private float mBodyRotationSpeed = 20f;
    private float mArmRotationSpeed = 20f;

    public int mArmVariable;
    public int mTurnVariable;
    
    public Interactive interactableObject;

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
    private int mIntendedDirection = 1;
    [SerializeField]
    public float mJumpForce;
    [SerializeField]
    public float mChargedJumpForce;
    [Range(0, 1)]
    public float mCutJumpHeight;
    public bool isGrounded;
    private bool alreadyLanded;
    private bool mOnMovingObject; //used for when the player is on top of another moving object
    private float mZDistance = 0f;
    [SerializeField]
    private float dashDamage;
    [SerializeField]
    private float chargedDashDamage;

    public bool mShifting = false;

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
    private bool _isCharged;
    public bool isCharged
    {
        get
        {
            return _isCharged;
        }
        set
        {
            _isCharged = value;
            if (mWeapons != null)
            {
                mWeapons[mWeaponIndex].Overcharged(_isCharged);
                mWeapons[mWeaponIndex + mWeaponCount].Overcharged(_isCharged);
            }
        }
    }
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
    private static bool _gravThreshSet = false;
    private static float _gravThresh;
    private static float DEFAULT_SHIFT_THRESHOLD = 0.25f;
    public static float gravShiftThreshold
    {
        get
        {
            return _gravThresh;
        }
        set
        {
            _gravThresh = value;
            if (!_gravThreshSet)
                _gravThreshSet = true;
        }
    }

    public event OnVarChangeDel OnVarChange;
    public delegate void OnVarChangeDel(int newVal);

    //FX stuff
    private Transform feetZone;
    private int objectsInFeetZone;
    private TrailRenderer dashTrail;
    private ParticleSystem landingDust;
    private ParticleSystem jumpBlast;
    private Vector3 lastJumpPos;
    private Quaternion lastJumpRot;
    private Vector3 lastLandingPos;
    private Quaternion lastLandingRot;

    public AudioSource fxAudio;
    [SerializeField]
    public AudioClip dashSound;
    [SerializeField]
    public AudioClip chargedDashSound;
    [SerializeField]
    public AudioClip gravShiftSound;
    [SerializeField]
    public AudioClip landingSound;
    [SerializeField]
    public AudioClip jumpSound;
    [SerializeField]
    public AudioClip chargeJumpSound;
    [SerializeField]
    public GameObject explosion;
    public bool canMove = true;

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


    //Gameplay stats
    private const float DEFAULT_HEALTH = 100f;
    [SerializeField]
    private Vector3 spawnPoint;
    private static Checkpoint lastCheckpoint;
    [Serializable]
    private struct SaveData
    {
        public float health;
        public float spawnX;
        public float spawnY;
        public float spawnZ;
        public GravityShifter.Gravity orientation;
    }

    //Weapon stuff
    public Weapon[] mWeapons; //TEMPORARY; DO NOT KEEP PUBLIC
    public int mWeaponIndex = 0; //TEMPORARY; DO NOT KEEP PUBLIC
    public int mPreviousWeaponIndex = 0; //TEMPORARY
    public int mWeaponCount; //TEMPORARY; DO NOT KEEP PUBLIC

    public GameObject mWeaponWheelCursor; //consider changing this into a RectTransform
    private WeaponSelector mWeaponWheelRef;
    private float mWheelWidth;

    void Awake()
    {
        if (!_gravThreshSet)
            gravShiftThreshold = DEFAULT_SHIFT_THRESHOLD;
        if (!_aimThreshSet)
            armAimThreshold = DEFAULT_AIMING_THRESHOLD;
    }

    // Use this for initialization
    void Start() {
        health = maxHealth = DEFAULT_HEALTH; //TODO: make this more secure later!
        mRb = GetComponent<Rigidbody>(); //secure this later
        _mGravShifter = GetComponent<GravityShifter>(); //secure this later
        //mDistToGround = GetComponent<Collider>().bounds.extents.y; //secure this later //UPDATE: might not need this anymore with the new foot detection trigger
        fxAudio = GetComponent<AudioSource>();

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);

        feetZone = transform.Find("FeetDetection");

        mWeapons = GetComponentsInChildren<Weapon>();
        mWeaponCount = mWeapons.Length / 2;

        ClearWeapons(); //check later on if this is still necessary

        dashTrail = GetComponent<TrailRenderer>();
        dashTrail.enabled = false;

        Transform tempDust = transform.Find("DustEffect");
        landingDust = tempDust.GetComponent<ParticleSystem>();

        Transform tempJump = transform.Find("JumpEffect");
        jumpBlast = tempJump.GetComponent<ParticleSystem>();

        /*
         * Adds a backup checkpoint in case one was never set; SHOULD be purely for testing/development purposes. 
         * In other words, THIS PROBABLY SHOULD NOT BE HERE FOR THE DEMO VERSION, AND DEFINITELY SHOULD NOT BE HERE FOR THE FULL VERSION
         */
        if (lastCheckpoint == null)
        {
            //Debug.Log("PlayerControls: Needed to create a backup checkpoint (for testing). If this causes any future errors, then there is a problem with the way the backup checkpoint is being created");
            SetCheckpoint(Checkpoint.CreateCheckpoint());
            //lastCheckpoint.ToString();
            if (lastCheckpoint == null) //Seriously, if this block of code is reached, something went wrong
            {
                Debug.Log("PlayerControls: lastCheckpoint is STILL null; some serious shit must have gone down to reach this point");
            }
            else
                GameManager.Instance.DataUtil.Save();
            //lastCheckpoint.healthAtTime = health;
            //lastCheckpoint.SetCheckpointOrientation((int)mGravShifter.mCurGravity);
        }

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

        if (jumpBlast.isPlaying)
        {
            jumpBlast.transform.position = lastJumpPos;
            jumpBlast.transform.rotation = lastJumpRot;
        }
        if (landingDust.isPlaying)
        {
            landingDust.transform.position = lastLandingPos;
            landingDust.transform.rotation = lastLandingRot;
        }
    }


    void FixedUpdate() {

        float lLx = Input.GetAxis("LStickX");
        float lLy = Input.GetAxis("LStickY");
        float lRx = Input.GetAxis("RStickX");
        float lRy = Input.GetAxis("RStickY");

        if (canMove)
        {
            //Left stick controls
            if ((int)GameManager.currentGameMode < 2) //Can move with the "Gameplay" and "WeaponWheel" modes
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

                    LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1 << 15; //environment, enemies, destructible, and doors
                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, mGravShifter.GetMovementVector() * mIntendedDirection, out hit, 0.7f, layers))
                    {
                        if (IsDashing())
                        {
                            if (!dashTrail.enabled)
                                dashTrail.enabled = true;
                            //following lines are reduced by 1/10th because of the left stick sensitivity
                            float lDashSpeed = CommandPattern.OverCharge.mCharged ? mChargedDashSpeed : mDashSpeed;
                            transform.position += 0.1f * mIntendedDirection * mGravShifter.GetMovementVector() * lDashSpeed;
                        }
                        else
                        {
                            transform.position += 0.1f * mGravShifter.GetMovementVector() * (lLx * mMovementSpeed);
                        }
                    }


                    mZDistance = transform.position.z;
                    if (Mathf.Abs(mZDistance) > 0.05f)
                    {
                        transform.position -= Vector3.forward * mZDistance;
                    }
                    //End main movement section

                    //Arm movement section
                    if (Mathf.Abs(lLy) >= armAimThreshold)
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


                if (lGravInput.magnitude > gravShiftThreshold && CanShift() && !mAttached)
                {
                    lGravAngle = Vector2.Angle(Vector2.up, lGravInput);
                    Vector3 cross = Vector3.Cross(Vector2.up, lGravInput);

                    if (cross.z > 0)
                        lGravAngle = -lGravAngle;

                    if (Mathf.Abs(lGravAngle) < 135f) //not downwards
                    {
                        mCanShift = false;
                        shiftTimer = mGravShiftDelay;
                        alreadyLanded = false;
                        fxAudio.PlayOneShot(gravShiftSound, 0.25f);

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
        bool dashing = mDashDelay - mDashTimer <= mDashDuration;
        dashTrail.enabled = dashing;
        return dashing;
    }

    private void OnTriggerEnter(Collider other)
    {
        ++objectsInFeetZone;
        if (!alreadyLanded && LayerMask.LayerToName(other.gameObject.layer) == "Environment")
        {
            alreadyLanded = true;
            lastLandingPos = feetZone.position;
            lastLandingRot = Quaternion.Euler(transform.rotation.eulerAngles + (-90 * Vector3.right));
            fxAudio.PlayOneShot(landingSound, 0.5f);
            landingDust.Play();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (IsDashing() && col.gameObject.TryGetComponent(out Damageable damageable))
        {
            print("Should be dash damaging");
            float lDamage = isCharged ? dashDamage : chargedDashDamage;
            damageable.ChangeHealth(-lDamage);
        }

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

    private void OnTriggerStay(Collider other)
    {
        int[] standables = { 11, 12, 13, 15 };
        if (standables.Contains(other.gameObject.layer)) {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        --objectsInFeetZone;
        if (objectsInFeetZone == 0)
        {
            alreadyLanded = false;
        }
        isGrounded = false;
    }

    private Vector3 GetRelativeVelocity()
    {
        return transform.InverseTransformDirection(mRb.velocity);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            Vector3 lRelVel = GetRelativeVelocity();
            float particleSize;
            alreadyLanded = false;
            //AudioClip jumpSFX;

            if (isCharged)
            {
                lRelVel.y = mChargedJumpForce;
                particleSize = 4;
                fxAudio.PlayOneShot(chargeJumpSound, 0.3f);
            }
            else
            {
                lRelVel.y = mJumpForce;
                particleSize = 2;
                fxAudio.PlayOneShot(jumpSound, 0.4f);
            }
            var main = jumpBlast.main;
            main.startSize = particleSize;
            lastJumpPos = feetZone.position;
            lastJumpRot = transform.rotation;
            jumpBlast.Play();

            mRb.velocity = transform.TransformDirection(lRelVel);
        }
    }

    public void JumpSlowdown()
    {
        Vector3 lRelVel = GetRelativeVelocity();

        if (lRelVel.y > 0)
        {
            lRelVel.y *= mCutJumpHeight;
            mRb.velocity = transform.TransformDirection(lRelVel);
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

    public void RefillGravity()
    {
        shiftTimer = 0;
    }

    public void RefillDash()
    {
        mDashTimer = 0;
    }

    private void SetDefaultSpawnPoint()
    {
        spawnPoint = transform.position;
    }

    public static void SetCheckpoint(Checkpoint c)
    {
        lastCheckpoint = c;
    }

    public static Checkpoint GetCheckpoint()
    {
        return lastCheckpoint;
    }

    public static bool HasCheckpoint()
    {
        return lastCheckpoint != null;
    }

    public void SetSpawnPoint(Vector3 respawn)
    {
        spawnPoint = respawn;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }

    //Save data utilities
    public object CaptureState()
    {
        return new SaveData
        {
            health = health,
            spawnX = lastCheckpoint.transform.position.x,
            spawnY = lastCheckpoint.transform.position.y,
            spawnZ = lastCheckpoint.transform.position.z,
            orientation = (GravityShifter.Gravity)lastCheckpoint.orientation
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        health = saveData.health;
        spawnPoint = new Vector3(saveData.spawnX, saveData.spawnY, saveData.spawnZ);
        mGravShifter.mCurGravity = saveData.orientation;
        transform.position = spawnPoint;
        mDashTimer = 0;
        mRb.velocity = Vector3.zero;
    }

    //end save data utilities

    public override void Die()
    {
        GameManager.Instance.PlayerDeathSequence();
    }
}
