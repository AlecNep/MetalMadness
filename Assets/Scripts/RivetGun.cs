using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RivetGun : Weapon {

    public override void Fire()
    {
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;
            Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
            Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
            GameObject lBullet = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon
            
            lBullet.GetComponent<Bullet>().SetDirection(lDirection);
        }
    }
}
