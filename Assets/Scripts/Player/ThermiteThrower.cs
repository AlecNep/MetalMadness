using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermiteThrower : Weapon {

    private readonly float mConeSpreadMax = 6;
    Quaternion mOrientation;


    new void Awake()
    {
        mBulletSpawn = transform.Find("BulletSpawn");
        mFiringLocation = mBulletSpawn.position;

        mFireType = mFireTypes.auto;
        mArmsParent = transform.parent.parent;
        mPlayer = mArmsParent.parent.GetComponent<PlayerControls>();
    }

	// Use this for initialization
	new void Start () {
        
        mOrientation = Quaternion.identity;
	}

    public override void Firing()
    {
        mShotDelayTimer = mFireRate;
        Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
        Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;

        mOrientation = Random.rotation;
        GameObject lBlob = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject;
        lBlob.transform.rotation = Quaternion.RotateTowards(lBlob.transform.rotation, mOrientation, mConeSpreadMax);
        lBlob.GetComponent<Bullet>().SetDirection(lBlob.transform.right);
    }
}
