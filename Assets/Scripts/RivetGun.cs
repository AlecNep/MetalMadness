using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RivetGun : Weapon {

    private float mShotDelayTimer;
    

	// Use this for initialization
	void Start () {
        mShot = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rivet.prefab", typeof(GameObject));
        mShotRB = mShot.GetComponent<Rigidbody>(); //really needs to be safer
	}
	
	// Update is called once per frame
	void Update () {

        if (mShotDelayTimer > 0)
        {
            mShotDelayTimer -= Time.deltaTime;
        }
        if (mShotDelayTimer < 0)
            mShotDelayTimer = 0;
	}

    public override void Fire()
    {
        //print("Arm Variable: " + mPlayer.mArmVariable);
        print("Gravity: " + (int)mPlayer.mCurGravity);
        //print("Turn variable: " + mPlayer.mTurnVariable);

        //print("orientation number: " + (mPlayer.mShotOrientation / 90));
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
