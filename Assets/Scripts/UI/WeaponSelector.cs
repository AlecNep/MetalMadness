using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour {

    static List<WheelIcon> mWeapons;
    static PlayerControls mPlayerRef;
    public static int mHighlighted { get; private set; }

	// Use this for initialization
	void Start () {
        mWeapons = new List<WheelIcon>(GetComponentsInChildren<WheelIcon>());
        mPlayerRef = GameObject.Find("Player").GetComponent<PlayerControls>();
        mHighlighted = -1;
	}

    public void Selector(Vector2 pAngle)
    {
        int weaponCount = mPlayerRef.mWeaponCount; //not sure why this had to be moved here, but okay

        float lSlotSize = 360f / weaponCount;
        float lAngle = Vector2.Angle(Vector2.up, pAngle);
        int lSelected;

        Vector3 cross = Vector3.Cross(Vector2.up, pAngle);

        if (cross.z > 0)
        {
            lAngle = 360f - lAngle;
        }
        lSelected = (int)(lAngle / lSlotSize);
        if (mHighlighted == -1) //fresh start; nothing has been highlighted since the wheel was opened
        { //Edit 13/09/2021: I have no idea why I did this or why it works, but it works (for now), so I'm not touching it!!!
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
            mPlayerRef.SetWeapons(mHighlighted);
            mWeapons[mHighlighted].Toggle();
            mHighlighted = -1;
        }
    }
}
