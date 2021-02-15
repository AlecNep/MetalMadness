using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInSightNode : Node
{
    public Transform mTarget;
    public Transform mEntity; //The asset using this script
    public float mMaxTrackingDistance;

    public TargetInSightNode(Transform pTarget, Transform pEntity, float pTrackingDistance)
    {
        mTarget = pTarget;
        mEntity = pEntity;
        mMaxTrackingDistance = pTrackingDistance;
    }

    public override NodeStates Evaluate()
    {
        RaycastHit lSight;
        LayerMask lMask = ~(1 << 9 | 1 << 12);
        Physics.Raycast(mEntity.position, mTarget.position, out lSight, mMaxTrackingDistance, lMask);

        return ReferenceEquals(lSight.collider.gameObject, mTarget.gameObject) ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }
}
