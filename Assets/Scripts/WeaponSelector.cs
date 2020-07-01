using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour {

    static List<WheelIcon> mWeapons;
    int mWeaponCount = 5; //NOTE: might not be necessary. Here for placeholder purposes  //hardcoded
    static PlayerControls mPlayerRef;
    public static int mHighlighted { get; private set; }

	// Use this for initialization
	void Start () {
        //Need to figure out when and how to determine the weapon amount
        //mWeapons = new List<WheelIcon>(mWeaponCount);\
        mWeapons = new List<WheelIcon>(GetComponentsInChildren<WheelIcon>());
        mPlayerRef = GameObject.Find("Player").GetComponent<PlayerControls>();
        mHighlighted = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Selector(Vector2 pAngle)
    {
        float lSlotSize = 360f / mWeaponCount;
        float lAngle = Vector2.Angle(Vector2.up, pAngle);
        //int lHighlighted = 0;
        int lSelected;

        Vector3 cross = Vector3.Cross(Vector2.up, pAngle);

        if (cross.z > 0)
        {
            //lAngle = -lAngle;
            lAngle = 360f - lAngle;
        }
        //print("angle after: " + lAngle);
        //lSelected = lAngle >= 0 ? (int)(lAngle / lSlotSize) : mWeaponCount + (int)(lAngle / lSlotSize) - 1;
        lSelected = (int)(lAngle / lSlotSize);
        //OR have a "cyclic" index by combnining it wit the max. e.g. if current < 0, current = count + current
        //print("Angle = " + lAngle + ", Current slot = " + lSelected + ", highlighted = " + lHighlighted);
        if (mHighlighted == -1) //fresh start; nothing has been highlighted since the wheel was opened
        {
            mWeapons[lSelected].Toggle();
            mHighlighted = lSelected;
        }
        else if (lSelected != mHighlighted)
        {
            mWeapons[mHighlighted].Toggle();
            mWeapons[lSelected].Toggle();
            mHighlighted = lSelected;
        }
        
    }

    public static void Reset()
    {
        if(mHighlighted > -1)
        {
            mPlayerRef.mPreviousWeaponIndex = mPlayerRef.mWeaponIndex;
            mPlayerRef.mWeaponIndex = mHighlighted;
            mPlayerRef.ClearWeapons();
            mWeapons[mHighlighted].Toggle();
            mHighlighted = -1;
        }
    }
}
