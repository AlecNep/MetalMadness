using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInVerticalReach : Node
{
    public Transform mTarget;
    public Transform mEntity; //The asset using this script
    public float mMaxTrackingDistance;


    public TargetInVerticalReach(Transform pTarget, Transform pEntity, float pTrackingDistance)
    {
        mTarget = pTarget;
        mEntity = pEntity;
        mMaxTrackingDistance = pTrackingDistance;
    }

    //Need a way to pass the grav variable on in real-time
    public override NodeStates Evaluate()
    {
        /*if ((int)mGravShifter.mCurGravity % 2 == 1)
        {
            return Mathf.Abs((mMainTarget.transform.position - transform.position).x) <= mAttackDistance ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            return Mathf.Abs((mMainTarget.transform.position - transform.position).y) <= mAttackDistance ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }*/
        return NodeStates.FAILURE;
    }
}
