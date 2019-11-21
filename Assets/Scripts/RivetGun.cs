using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RivetGun : Weapon {

    [SerializeField]
    private float mShotVelocity;
    private float mShotDelayTimer;
    [SerializeField]
    private float mLifeSpan;

	// Use this for initialization
	void Start () {
        mShot = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rivet.prefab", typeof(GameObject));
        mShotRB = mShot.GetComponent<Rigidbody>(); //really needs to be safer
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Fire()
    {
        if (mShotDelayTimer == 0)
        {
            mShotDelayTimer = mFireRate;

            Instantiate(mShot, mFiringLocation, transform.localRotation);
            //Add the movement behavior to the shot itself
        }
    }
}
