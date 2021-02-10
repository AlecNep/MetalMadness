using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public enum RelativeDirections {down = 1, right = 2, up = 4, left = 8 };


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
        mMainTarget = GameObject.Find("Player"); //For testing purposes
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

            //TODO
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

    public virtual NodeStates IsTargetInSight()
    {
        RaycastHit lSight;
        LayerMask lMask = ~(1 << 9 | 1 << 12);
        Physics.Raycast(transform.position, mMainTarget.transform.position, out lSight, mMaxTrackingDistance, lMask);

        return ReferenceEquals(lSight.collider.gameObject, mMainTarget) ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    public NodeStates TargetVisibleShift() //Possibly rename
    {
        Collider[] lSurfaces = Physics.OverlapSphere(mMainTarget.transform.position, mAttackDistance, 11); //11 = environment layer
        byte lScanDirections = 15;

        //Scan for suitable surfaces to shift to
        Vector3 lDir = (mMainTarget.transform.position - transform.position).normalized; //TEMP: for testing
        float lAngle = Vector3.SignedAngle(-transform.up, lDir, Vector3.forward);

        int lRoundedAngle = (int)lAngle; //should be a more accurate rounding operations
        lRoundedAngle = (lRoundedAngle + 360) % 360;

        if (((lRoundedAngle / 30) % 3) % 2 == 0) //within 30 degrees of a cardinal direction
        {
            int lRoundedDiv = (int)lAngle / 90;

            lScanDirections ^= (byte)~(1 << lRoundedDiv);

            if (lScanDirections != 1) //This really should never happen since this whole function depends on line of sight with a target
                mGravShifter.ShiftGravity((int)Mathf.Log(2, lScanDirections));
            return NodeStates.SUCCESS;
        }
        else
        {
            //rule out directions to scan
            if (lAngle > 0) //to the right, to some extent
                lScanDirections ^= (byte)RelativeDirections.left;
            else //to the left, to some extent
                lScanDirections ^= (byte)RelativeDirections.right;

            if (Mathf.Abs(lAngle) > 90) //up to some extent
                lScanDirections ^= (byte)RelativeDirections.down;
            else //down to some extent
                lScanDirections ^= (byte)RelativeDirections.up;

            //TODO

            /**
             * Scan directions follow LURD - Left, Up, Right, Down
             * For combinations, only 0011, 0110, 1001, and 1100 are possible
             */

            Vector3[] lPossibleVectors = new Vector3[2];

            //Since all viable combinations use both vectors to some extent, we only need to find which directions of each are being used
            lPossibleVectors[0] = mGravShifter.GetGravityNormal();
            lPossibleVectors[1] = mGravShifter.GetMovementVector();

            if ((lScanDirections & 4) == 4) //Up, opposite of grav vector
                lPossibleVectors[0] *= -1;
            else
                Debug.LogError("Warning: " + name + " at " + transform.position + " is trying to shift gravity downwards");
            if ((lScanDirections & 8) == 8) //Left, opposite of moement vector
                lPossibleVectors[1] *= -1;

            RaycastHit[] lScannedSurfaces = new RaycastHit[2];
            for (int i=0; i<2; ++i)
            {
                Physics.Raycast(transform.position, lPossibleVectors[i], out lScannedSurfaces[i], mMaxTrackingDistance, 11);
            }

            int lValidSurfaces = 0;
            for(int i=0; i < 2; ++i)
            {
               if (lSurfaces.Contains(lScannedSurfaces[i].collider))
                {
                    lValidSurfaces += (i + 1);
                }
            }
            int lVertical = 2 * ((lScanDirections & 4) / 4);
            int lHorizontal = 1 + 2 * ((lScanDirections & 8) / 8);
            switch (lValidSurfaces)
            {
                case 0:
                    return NodeStates.FAILURE;
                case 1: //only the first one
                    mGravShifter.ShiftGravity(lVertical); //Will return 0 if down, or 2 if up
                    return NodeStates.SUCCESS;
                case 2: //only the second
                    mGravShifter.ShiftGravity(lHorizontal); //Will return 1 if right, or 3 if left
                    return NodeStates.SUCCESS;
                case 3: //both
                    if (Vector3.Distance(transform.position, lScannedSurfaces[0].collider.transform.position) 
                        < Vector3.Distance(transform.position, lScannedSurfaces[1].collider.transform.position))
                        mGravShifter.ShiftGravity(lVertical);
                    else
                        mGravShifter.ShiftGravity(lHorizontal);
                    return NodeStates.SUCCESS;
            }
        }

        Debug.LogError("Warning: The \"TargetVisibleShift\" function completed without any viable options");
        return NodeStates.FAILURE; //Here for default
    }

    protected virtual void Attack() { }

}
