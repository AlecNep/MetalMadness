using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermiteThrower : AutomaticWeapon {

    private readonly float mConeSpreadMax = 6;
    Quaternion mOrientation;

	// Use this for initialization
	new void Start () {
        //mShot = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Thermite.prefab", typeof(GameObject));
        mFireType = mFireTypes.auto;
        mOrientation = Quaternion.identity;
	}

    public override void Firing()
    {
        mShotDelayTimer = mFireRate;
        Vector3 lDirection = Vector3.Normalize(mBulletSpawn.position - transform.position);
        Vector3 lOrientation = Vector3.forward * mPlayer.mShotOrientation;

        mOrientation = Random.rotation;
        GameObject lBlob = Instantiate(mShot, mBulletSpawn.position, Quaternion.Euler(lOrientation)) as GameObject;
        lBlob.transform.rotation = Quaternion.RotateTowards(lBlob.transform.rotation, mOrientation, mConeSpreadMax);
        lBlob.GetComponent<Bullet>().SetDirection(lBlob.transform.right);
    }
}
