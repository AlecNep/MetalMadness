using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGun : Weapon {

    new void Start()
    {
        mFireType = mFireTypes.spike;
    }

    public override void Fire()
    {
        throw new System.NotImplementedException();
    }
}
