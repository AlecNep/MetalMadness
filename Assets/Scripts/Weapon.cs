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


    public void Awake()
    {
        mFiringLocation = new Vector3(0, -GetComponent<Collider>().bounds.max.y, 0);
        mArmsParent = transform.parent.parent;
        print("mArmsParent: " + mArmsParent.name);
        mPlayer = mArmsParent.parent;
        print("mPlayer: " + mPlayer.name);
    }

    public abstract void Fire();
}
