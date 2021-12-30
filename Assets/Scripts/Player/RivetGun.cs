using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RivetGun : Weapon {

    [SerializeField]
    public GameObject mChargedShot;

    public override void Fire()
    {
        if (mOvercharged)
        {
            if (mChargedDelayTimer == 0)
            {
                mChargedDelayTimer = mChargedFireRate;
                Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
                Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
                GameManager.Instance.MainCamera.ScreenShake(mChargedShake, mChargedShakeTime);
                GameObject lBullet = Instantiate(mChargedShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

                lBullet.GetComponent<Bullet>().SetDirection(lDirection);
            }
        }
        else
        {
            if (mShotDelayTimer == 0)
            {
                mShotDelayTimer = mFireRate;
                Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
                Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
                GameManager.Instance.MainCamera.ScreenShake(mScreenShake, mShakeTime);
                GameObject lBullet = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

                lBullet.GetComponent<Bullet>().SetDirection(lDirection);
            }
        }
    }
}
