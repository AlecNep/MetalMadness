using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNavMesh : MonoBehaviour
{
    public bool isStopped;
    private Vector3 _destination;
    public Vector3 destination { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            if (destination != null)
            {

            }


        }
    }

    public void SetDestination(Vector3 pDestination)
    {
        destination = pDestination;
    }

    public void AdjustOrientation(Vector3 pMovementVector, Vector3 pGravityVector, int pIntendedDirection, float pBodyRotationSpeed)
    {
        Quaternion pTargetRotation = Quaternion.LookRotation(pMovementVector * pIntendedDirection, -pGravityVector);
        if (transform.rotation != pTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, pTargetRotation, pBodyRotationSpeed);
        }
    }
}
