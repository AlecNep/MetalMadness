using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermiteThrower : Weapon {

    private readonly float mConeSpreadMax = 6;
    Quaternion mOrientation;


    new void Awake()
    {
        base.Awake();
        
        mFireType = mFireTypes.auto;
    }

	// Use this for initialization
	new void Start () {
        
        mOrientation = Quaternion.identity;

        print("Thermite shot RB? " + (mShotRB != null));
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
