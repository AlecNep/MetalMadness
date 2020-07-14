using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Weapon {

	// Use this for initialization
	new void Start () {
        mFireType = mFireTypes.spike;
	}

    public override void Fire()
    {
        throw new System.NotImplementedException();
    }
}
