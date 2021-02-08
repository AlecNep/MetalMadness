using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceBot : MonoBehaviour {

    //Personal variables
    protected int mIntendedDirection = 1;
    [SerializeField]
    protected float mMovementSpeed;
    protected readonly float DEFAULT_ARM_ROTATION = 90f;
    protected float mBodyRotationSpeed = 15f;
    protected Quaternion mTargetRotation;

    //Patrol Variables
    protected Vector3 mStartingPoint;
    public float mPatrolDistance;
    protected float mCounter;
    protected float mDistFromStart;
    public enum State {patrolling = 0, seeking = 1, attacking = 2, returning = 3 }
    public State mCurState = State.patrolling;
    protected float mPreviousDist = 0;

    //References
    public Transform mArms { get; private set; } //maybe should just be private

    //Tracking variables
    protected List<GameObject> mTargets;
    protected GameObject mMainTarget; //not sure just yet if this should be a Gameobject or Transform
    protected float mTargetDistance;
    protected float mMaxTrackingDistance;
    protected float mAttackDistance;

    [System.Flags]
    public enum RelativeDirections {up = 1, right = 2, down = 4, left = 8 };


    //Gravity shifting functionality
    protected GravityShifter _mGravShifter;
    public GravityShifter mGravShifter
    {
        get
        {
            if (_mGravShifter != null)
                return _mGravShifter;
            else
                return null;
        }
    }

    // Use this for initialization
    void Start () {
        _mGravShifter = GetComponent<GravityShifter>();
        if (_mGravShifter == null)
        {
            System.Console.Error.WriteLine("Maintenance bot " + name + "Was not given a GravityShifter component!");
        }
        mStartingPoint = transform.position;

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);

        mTargets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update () {
        if (mCurState == 0) //Patrolling
        {
            mCounter += Time.deltaTime;
            float lSineResult = Mathf.Sin(mCounter * mMovementSpeed);
            mIntendedDirection = (int)Mathf.Sign(lSineResult - mPreviousDist);
            mPreviousDist = lSineResult;
            mDistFromStart = mPatrolDistance * lSineResult;

            transform.position = mStartingPoint + _mGravShifter.GetMovementVector() * mDistFromStart;

            if (Mathf.Abs(transform.position.z) > 0.05f)
            {
                transform.position -= Vector3.forward * transform.position.z;
            }
        }
        else if ((int)mCurState == 1) //Seeking
        {
            //perhaps should be in a child class
        }
        else if ((int)mCurState == 2) //About to attack a target
        {
            //perhaps should be in a child class
        }
        else
        {

        }

        mTargetRotation = Quaternion.LookRotation(_mGravShifter.GetMovementVector() * -mIntendedDirection, -_mGravShifter.GetGravityNormal());
        if (transform.rotation != mTargetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mTargetRotation, mBodyRotationSpeed);
        }

        /*if (mTargetDistance > mMaxTrackingDistance)
        {
            mTarget = null;
            mTargetDistance = 0;
            mCurState = State.returning;
        }*/
    }

    public void SetPatrolPoint(Vector3 pCenter)
    {
        mStartingPoint = pCenter;
    }

    /*public NodeStates ReturnToPatrol()
    {

    }*/

    private void OnTriggerEnter(Collider pCol)
    {
        mTargets.Add(pCol.gameObject);
        PrioritizeTargets();
    }

    public void PrioritizeTargets()
    {
        if (mTargets.Count == 0)
        {
            mMainTarget = null;
        }
        else
        {
            GameObject lClosest = mTargets[0];
            float lClosestDist = Vector3.Distance(transform.position, lClosest.transform.position);

            foreach (GameObject g in mTargets)
            {
                if (g.tag == "Player")
                {
                    mMainTarget = g;
                    break;
                }
                else if (ReferenceEquals(g, lClosest))
                {
                    continue;
                }
                else
                {
                    float lDist = Vector3.Distance(transform.position, g.transform.position);
                    if (lDist < lClosestDist)
                    {
                        lClosest = g;
                        lClosestDist = lDist;
                    }
                }
            }
        }
    }

    public virtual NodeStates TargetInSight()
    {
        RaycastHit lSight;
        LayerMask lMask = ~(1 << 9 | 1 << 12);
        Physics.Raycast(transform.position, mMainTarget.transform.position, out lSight, mMaxTrackingDistance, lMask);

        return ReferenceEquals(lSight.collider.gameObject, mMainTarget) ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    public NodeStates SurfaceInSight()
    {
        Collider[] lSurfaces = Physics.OverlapSphere(mMainTarget.transform.position, mAttackDistance, 11); //11 = environment layer
        byte lScanDirections = 15;

        //float lAngle = Vector3.Angle(transform.up, mMainTarget.transform.position);
        float lAngle = Vector3.SignedAngle(transform.up, mMainTarget.transform.position, Vector3.forward); //might need Vector3.back
        float lUAngle = Mathf.Abs(lAngle); //Useful for some calculations

        int lRoundedAngle = (int)lAngle; //should be a more accurate rounding operations
        if (lRoundedAngle % 90 == 0) //basically in direct sight of a cardinal direction
        {
            int lRoundedDiv = (int)lAngle / 90;
        }

        //rule out directions to scan
        if (lAngle > 0) //to the right, to some extent
            lScanDirections ^= (byte)RelativeDirections.left;
        else //to the left, to some extent
            lScanDirections ^= (byte)RelativeDirections.right;

        if (Mathf.Abs(lAngle) > 90) //down to some extent
            lScanDirections ^= (byte)RelativeDirections.up;
        else //up, to some extent
            lScanDirections ^= (byte)RelativeDirections.down;

        return NodeStates.FAILURE; //Here for default
    }

    protected virtual void Attack() { }

}
