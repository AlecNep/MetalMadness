using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGun : Weapon {

    private Spike mSpike;

    public new void Awake()
    {
        mFireType = mFireTypes.spike;
        mArmsParent = transform.parent.parent;
        mPlayer = mArmsParent.parent.GetComponent<PlayerControls>();

        mSpike = GetComponentInChildren<Spike>(); //make this safer!!!!!!
        if (mSpike != null)
            print("We good");
    }

    public new void Start()
    {
        //just here to override the original and avoid the shot-based logic
    }

    public override void Fire()
    {
        mSpike.ExpandSequence();
    }

    public override void StopFiring()
    {
        mSpike.CollapseSequence();
    }
}
