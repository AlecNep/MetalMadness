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
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;

            GameObject lBullet = Instantiate(mShot, mBulletSpawn.transform.position, Quaternion.Euler(-transform.up)) as GameObject; //update soon
            //Rigidbody lBulletRb = lBullet.GetComponent<Rigidbody>();
            //lBulletRb.velocity = -transform.up;
            lBullet.GetComponent<Bullet>().SetDirection(transform.InverseTransformDirection(-mBulletSpawn.up));
        }
        else
            print("Can't shoot yet");
    }
}
