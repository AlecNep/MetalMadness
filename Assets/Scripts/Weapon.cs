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
    [SerializeField]
    protected float mFireRate;

    protected Transform mArmsParent;
    protected Transform mPlayer;
    protected Transform mBulletSpawn;


    public void Awake()
    {
        mBulletSpawn = transform.Find("BulletSpawn");
        mFiringLocation = mBulletSpawn.position;
        
        mArmsParent = transform.parent.parent;
        mPlayer = mArmsParent.parent;
    }

    public abstract void Fire();
}
