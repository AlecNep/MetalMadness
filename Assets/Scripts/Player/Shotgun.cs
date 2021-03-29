using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Shotgun : Weapon {

    public float mConeSpreadMax;
    public int mPieceCount;
    List<Quaternion> mPieces;
    [SerializeField]
    public GameObject mChargedShot;

    // Use this for initialization
    new void Start () {
        mPieces = new List<Quaternion>(mPieceCount);
        for (int i = 0; i < mPieceCount; i++)
        {
            mPieces.Add(Quaternion.Euler(Vector3.zero)); //Maybe Quaternion.identity instead
        }
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

                for (int i = 0; i < mPieceCount; i++)
                {
                    mPieces[i] = Random.rotation;
                    GameObject lShrapnel = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject;
                    lShrapnel.transform.rotation = Quaternion.RotateTowards(lShrapnel.transform.rotation, mPieces[i], mConeSpreadMax);
                    lShrapnel.GetComponent<Bullet>().SetDirection(lShrapnel.transform.right); //?
                }
            }
        }
    }
}
