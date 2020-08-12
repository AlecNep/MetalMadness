using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Bullet {

    private Transform[] mSpikeSections;

	// Use this for initialization
	void Start () {
        mMaxLifespan = Mathf.Infinity;
        Transform lS = transform;
        mSpikeSections = new Transform[6]; //Hardcoded for now
        int i = 0;

        while (lS.childCount > 0)
        {
            Transform temp = lS.GetChild(0);
            mSpikeSections[i] = temp;
            lS = temp;

            i++;
        }
        print(mSpikeSections[5].name);
    }

    public new void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor

        }
        else
        {

        }
    }
}
