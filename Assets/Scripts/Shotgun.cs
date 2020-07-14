using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Shotgun : Weapon {

    public float mConeSpreadMax;
    public int mPieceCount;
    List<Quaternion> mPieces;

	// Use this for initialization
	new void Start () {
        mShot = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Shrapnel.prefab", typeof(GameObject));
        //mShotRB = mShot.GetComponent<Rigidbody>(); //really needs to be safer
        mFireType = mFireTypes.semi;

        mPieces = new List<Quaternion>(mPieceCount);
        for (int i = 0; i < mPieceCount; i++)
        {
            mPieces.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    public override void Fire()
    {
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;
            Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
            Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;

            for(int i = 0; i < mPieceCount; i++)
            {
                mPieces[i] = Random.rotation;
                GameObject s = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject;
                s.transform.rotation = Quaternion.RotateTowards(s.transform.rotation, mPieces[i], mConeSpreadMax);
                s.GetComponent<Bullet>().SetDirection(s.transform.right); //?
            }
            /*GameObject lBullet1 = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

            lBullet1.GetComponent<Bullet>().SetDirection(lDirection);*/
        }
    }
}
