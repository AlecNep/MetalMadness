using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutomaticWeapon : Weapon {

	// Use this for initialization
	/*new void Start () {
        mFireType = mFireTypes.auto;
	}*/

    public new void Update()
    {
        if (mShotDelayTimer > 0)
        {
            mShotDelayTimer -= Time.deltaTime;
        }
        if (mShotDelayTimer < 0)
        {
            mShotDelayTimer = 0;
        }
            
        if (mFiring == true && mShotDelayTimer == 0)
        {
            print("firing: " + mFiring + ", Delay timer: " + mShotDelayTimer);
            Firing();
        }
    }

    public override void Fire()
    {
        mFiring = true;
    }

    public abstract void Firing();

    public override void StopFiring()
    {
        mFiring = false;
        print(mFiring);
    }
}
