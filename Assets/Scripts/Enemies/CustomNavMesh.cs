using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNavMesh : MonoBehaviour
{
    public bool isStopped = true;
    private Vector3 _destination;
    public Vector3 destination { get; private set; }

    [SerializeField]
    protected float mMovementSpeed;
    protected float mBodyRotationSpeed = 15f;
    protected int mIntendedDirection = 1;
    protected float mZDistance = 0;

    //Gravity shifting functionality
    protected GravityShifter _gravShifter;
    public GravityShifter gravShifter
    {
        get
        {
            if (_gravShifter == null)
            {

                _gravShifter = gameObject.AddComponent<GravityShifter>();
            }
            return _gravShifter;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gravShifter = GetComponent<GravityShifter>();
        if (_gravShifter == null)
        {
            System.Console.Error.WriteLine("Warning: " + name + " was not given a GravityShifter component!");
        }
        destination = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStopped)
        {
            if (destination != transform.position)
            {
                Vector3 lVectorToTarget = (transform.position - destination).normalized;
                mIntendedDirection = (int)Mathf.Sign(Vector3.SignedAngle(-gravShifter.GetGravityNormal(), lVectorToTarget, Vector3.forward));

                //Good for now, but this should ideally slow down when close
                transform.position += 0.1f * mIntendedDirection * gravShifter.GetMovementVector() * mMovementSpeed;
            }
        }
        mZDistance = transform.position.z;
        if (Mathf.Abs(mZDistance) > 0.05f)
        {
            transform.position -= Vector3.forward * mZDistance;
        }
        AdjustOrientation(gravShifter.GetMovementVector(), gravShifter.GetGravityNormal(), -mIntendedDirection, mBodyRotationSpeed);
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
