using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StickyBombLauncher : Weapon {

    // Use this for initialization
    new void Start () {
        mShot = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Sticky bomb.prefab", typeof(GameObject));
        mFireType = mFireTypes.semi;
    }

    public override void Fire()
    {
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;
            Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
            Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
            GameObject lBullet = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

            lBullet.GetComponent<StickyBomb>().SetDirection(lDirection);

            /*mShotDelayTimer = mFireRate;
            Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
            Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;
            GameObject lBullet = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject;*/


        }
    }
}
