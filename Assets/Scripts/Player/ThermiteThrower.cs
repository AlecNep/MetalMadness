using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermiteThrower : Weapon {

    private readonly float mConeSpreadMax = 6;
    Quaternion mOrientation;
    [SerializeField]
    public GameObject mChargedShot;

    new void Awake()
    {
        base.Awake();
        
        mFireType = mFireTypes.auto;
    }

	// Use this for initialization
	new void Start () {
        
        mOrientation = Quaternion.identity;
	}

    public override void Overcharged(bool pCharged)
    {
        base.Overcharged(pCharged);
        mFireType = pCharged ? mFireTypes.semi : mFireTypes.auto;
        //Change ammo type
    }

    public override void Fire()
    {
        if ((int)mFireType == 1)
        {
            mFiring = true;
        }
        else
        {
            if (mChargedDelayTimer == 0)
            {
                mChargedDelayTimer = mChargedFireRate;
                Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
                Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
                GameObject lBullet = Instantiate(mChargedShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

                lBullet.GetComponent<Bullet>().SetDirection(lDirection);
            }
        }
    }

    public override void Firing()
    {
        mShotDelayTimer = mFireRate;
        Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
        Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;

        mOrientation = Random.rotation;
        GameObject lBlob = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject;
        lBlob.transform.rotation = Quaternion.RotateTowards(lBlob.transform.rotation, mOrientation, mConeSpreadMax);
        lBlob.GetComponent<Bullet>().SetDirection(lBlob.transform.right); //probably super expensive to call this on a full-auto weapon
    }
}
