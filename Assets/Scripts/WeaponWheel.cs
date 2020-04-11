using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheel : MonoBehaviour {

    List<WheelIcon> mWeapons;
    int mWeaponCount = 5; //NOTE: might not be necessary. Here for placeholder purposes  //hardcoded
    public int mHighlighted { get; private set; }

	// Use this for initialization
	void Start () {
        //Need to figure out when and how to determine the weapon amount
        //mWeapons = new List<WheelIcon>(mWeaponCount);\
        mWeapons = new List<WheelIcon>(GetComponentsInChildren<WheelIcon>());
        print(mWeapons.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Selector(Vector2 pAngle)
    {
        float lSlotSize = 360f / mWeaponCount;
        float lAngle = Vector2.Angle(Vector2.up, pAngle);
        int lCurrent;

        Vector3 cross = Vector3.Cross(Vector2.up, pAngle);
        //print("angle before: " + lAngle);
        if (cross.z > 0)
        {
            lAngle = -lAngle;
            //lAngle = 360f + lAngle;
        }
        //print("angle after: " + lAngle);
        lCurrent = lAngle >= 0 ? (int)(lAngle / lSlotSize) : mWeaponCount + (int)(lAngle / lSlotSize) - 1;
        //OR have a "cyclic" index by combnining it wit the max. e.g. if current < 0, current = count + current
        print("Slot size = " + lSlotSize + ", Angle = " + lAngle + ", Current slot = " + lCurrent);
        if (lCurrent != mHighlighted)
        {
            mWeapons[mHighlighted].Toggle();
            mWeapons[lCurrent].Toggle();
        }
        //mWeapons[mHighlighted].Toggle();
        mHighlighted = lCurrent;
    }
}
