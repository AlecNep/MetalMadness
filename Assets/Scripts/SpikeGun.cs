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
        
    }

    public new void Start()
    {
        //just here to override the original and avoid the shot-based logic
    }

    public override void Fire()
    {
        /*
         * Only initiate the expand sequence if:
         * 1) enough time has passed
         * 2) it's not already epanding
         * 3) it's back at its start
         */
        if (mShotDelayTimer == 0 && !mSpike.mExpanding && !mSpike.mExtended) //why does this totally break it?
        {
            mSpike.ExpandSequence();
        }
    }

    public override void StopFiring()
    {
        /*
         * Only initiate the collapse sequence if:
         * 1) enough time has passed
         * 2) it's not already collapsing
         * 3) it's fully extended
         */
        if (mShotDelayTimer == 0 && !mSpike.mCollapsing && mSpike.mExtended) //why does this totally break it?
        {
            mShotDelayTimer = mFireRate;
            mSpike.CollapseSequence();
        }
        
    }
}
