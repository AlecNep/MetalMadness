using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RivetGun : Weapon {

    [SerializeField]
    public GameObject mChargedShot;

    protected override void Awake()
    {
        base.Awake();

        AudioSource[] sounds = GetComponents<AudioSource>();
        soundFX = sounds[0];
        chargedFX = sounds[1];
    }

    public override void Fire()
    {
        if (mOvercharged)
        {
            if (mChargedDelayTimer == 0)
            {
                mChargedDelayTimer = mChargedFireRate;
                Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
                Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
                GameObject lBullet = Instantiate(mChargedShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon
                GameManager.Instance.MainCamera.ScreenShake(mChargedShake, mChargedShakeTime);
                chargedFX.Play();

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
                GameObject lBullet = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon
                GameManager.Instance.MainCamera.ScreenShake(mScreenShake, mShakeTime);
                soundFX.Play();

                lBullet.GetComponent<Bullet>().SetDirection(lDirection);
            }
        }
    }
}
