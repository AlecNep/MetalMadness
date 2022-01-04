using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon {

    private float mCurDelay; //Fires the same bullet if charged, just faster
    private float mConeSpreadMax = 4;
    Quaternion mOrientation;

    new void Awake()
    {
        base.Awake();

        mFireType = mFireTypes.auto;
        mOrientation = Quaternion.identity;

        soundFX = GetComponent<AudioSource>(); //There should also be a higher sound for the charged attack, like the minigun from the original UT
    }

    new void Update()
    {
        if (mCurDelay > 0)
        {
            mCurDelay -= Time.deltaTime;
        }
        if (mCurDelay < 0)
        {
            mCurDelay = 0;
        }

        if (mFiring == true && mShotDelayTimer == 0)
        {
            Firing();
        }
    }

    public override void Firing()
    {
        if (mOvercharged)
        {
            mCurDelay = mChargedFireRate;
            mConeSpreadMax = 6;
            soundFX.pitch = 1.15f;
        }
        else
        {
            mCurDelay = mFireRate;
            mConeSpreadMax = 2;
            soundFX.pitch = 1f;
        }

        /*mCurDelay = mOvercharged ? mChargedFireRate : mFireRate;
        mConeSpreadMax = mOvercharged ? 6 : 2;
        */
        Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
        Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;

        mOrientation = Random.rotation;
        GameManager.Instance.MainCamera.ScreenShake(mScreenShake, mShakeTime);
        if (!soundFX.isPlaying)
            soundFX.Play();

        GameObject lBlob = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation));
        lBlob.transform.rotation = Quaternion.RotateTowards(lBlob.transform.rotation, mOrientation, mConeSpreadMax);
        lBlob.GetComponent<Bullet>().SetDirection(lBlob.transform.right); //probably super expensive to call this on a full-auto weapon
    }

    public void OnDisable()
    {
        mFiring = false;
    }
}
