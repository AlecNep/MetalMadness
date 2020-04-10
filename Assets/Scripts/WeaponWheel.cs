using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheel : MonoBehaviour {

    List<WheelIcon> mWeapons;
    int mWeaponCount; //NOTE: might not be necessary. Here for placeholder purposes 
    public int mHighlighted { get; private set; }

	// Use this for initialization
	void Start () {
        //Need to figure out when and how to determine the weapon amount
        //mWeapons = new List<Weapon>(7);
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
        if (cross.z > 0)
        {
            lAngle = -lAngle;
        }

        lCurrent = (int)(lAngle / lSlotSize);
        if (lCurrent != mHighlighted)
        {
            mWeapons[mHighlighted].Toggle();
        }
        //mWeapons[mHighlighted].Toggle();
        mHighlighted = lCurrent;
    }
}
