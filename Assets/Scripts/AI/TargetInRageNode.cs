using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRageNode : Node
{
    public Transform mTarget;
    public Transform mEntity; //The asset using this script
    public float mMaxTrackingDistance;

    public TargetInRageNode(Transform pTarget, Transform pEntity, float pTrackingDistance)
    {
        mTarget = pTarget;
        mEntity = pEntity;
        mMaxTrackingDistance = pTrackingDistance;
    }

    public override NodeStates Evaluate()
    {
        return Vector3.Distance(mEntity.position, mTarget.position) <= mMaxTrackingDistance ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }
}
