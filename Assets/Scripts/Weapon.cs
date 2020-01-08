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
    public enum mFireTypes { semi = 0, auto = 1, spike = 2 }; //actually might not be necessary; could do the code class-by-class
    public mFireTypes mFireType { get; protected set; }
    public bool mFiring = false; //only applicable for automatic
    [SerializeField]
    protected float mFireRate;

    protected Transform mArmsParent;
    protected PlayerControls mPlayer;
    protected Transform mBulletSpawn;


    public void Awake()
    {
        mBulletSpawn = transform.Find("BulletSpawn");
        mFiringLocation = mBulletSpawn.position;
        
        mArmsParent = transform.parent.parent;
        mPlayer = mArmsParent.parent.GetComponent<PlayerControls>();
    }

    public void Update()
    {
        if (mShotDelayTimer > 0)
        {
            mShotDelayTimer -= Time.deltaTime;
        }
        if (mShotDelayTimer < 0)
            mShotDelayTimer = 0;
    }

    public abstract void Fire();
}
