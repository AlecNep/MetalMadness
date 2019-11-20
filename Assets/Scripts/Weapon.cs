using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    protected GameObject mShot;
    protected GameObject mVFX;
    protected GameObject mModel; //really not sure if this is necessary
    protected Vector3 mFiringLocation;
    protected float mFireRate;

    public void Awake()
    {
        mFiringLocation = new Vector3(0, GetComponent<Collider>().bounds.max.y, 0);
    }

    public abstract void Fire();
}
