using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermiteThrower : AutomaticWeapon {

    public float mConeSpread;

	// Use this for initialization
	void Start () {
		
	}

    public override void Firing()
    {
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;
            Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);

        }
    }
}
