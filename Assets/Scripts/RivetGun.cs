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
        print(transform.name);
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;
            Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
            
            GameObject lBullet = Instantiate(mShot, mBulletSpawn.position, mArmsParent.rotation) as GameObject; //update soon
            
            lBullet.GetComponent<Bullet>().SetDirection(lDirection);
        }
        else
            print("Can't shoot yet");
    }
}
