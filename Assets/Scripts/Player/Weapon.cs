using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField]
    protected GameObject mShot;
    protected Rigidbody mShotRB;
    protected GameObject mVFX;
    protected GameObject mModel; //really not sure if this is necessary
    protected Vector3 mFiringLocation;
    protected float mShotDelayTimer;
    protected float mChargedDelayTimer;
    public enum mFireTypes { semi = 0, auto = 1, spike = 2 }; //definitely necessary for the command pattern class
    public mFireTypes mFireType { get; protected set; }
    public bool mFiring = false; //only applicable for automatic
    [SerializeField]
    protected float mFireRate;
    [SerializeField]
    protected float mChargedFireRate;
    [SerializeField]
    protected float mScreenShake;
    [SerializeField]
    protected float mChargedShake;
    [SerializeField]
    protected float mShakeTime;
    [SerializeField]
    protected float mChargedShakeTime;

    protected Transform mArmsParent;
    protected PlayerControls mPlayer;
    protected Transform mBulletSpawn;

    public bool mOvercharged { get; private set; }

    protected int ammo;
    [SerializeField]
    protected int maxAmmo;

    public void Awake()
    {
        mBulletSpawn = transform.Find("BulletSpawn");
        mFiringLocation = mBulletSpawn.position;

        mFireType = mFireTypes.semi; //default
        mArmsParent = transform.parent.parent;
        mPlayer = mArmsParent.parent.GetComponent<PlayerControls>();
    }

    public void Start()
    {
        mShotRB = mShot.GetComponent<Rigidbody>(); //really needs to be safer; also might not be necessary
    }

    public void Update()
    {
        if (mShotDelayTimer > 0)
        {
            mShotDelayTimer -= Time.deltaTime;
        }
        if (mShotDelayTimer < 0)
        {
            mShotDelayTimer = 0;
        }

        if (mChargedDelayTimer > 0)
        {
            mChargedDelayTimer -= Time.deltaTime;
        }
        if (mChargedDelayTimer < 0)
        {
            mChargedDelayTimer = 0;
        }

        if ((int)mFireType == 1)
        {
            if (mFiring == true && mShotDelayTimer == 0)
            {
                Firing();
            }
        }
    }

    public virtual void Overcharged(bool pCharged)
    {
        mOvercharged = pCharged;
    }

    public virtual void Fire()
    {

        if ((int)mFireType == 1)
        {
            mFiring = true;
        }
    }
    public virtual void Firing() {}
    public virtual void StopFiring()
    {
        mFiring = false;
    }
}
