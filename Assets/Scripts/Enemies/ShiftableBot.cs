using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShiftableBot : MonoBehaviour
{
    //Personal variables    
    protected readonly float DEFAULT_ARM_ROTATION = 90f;
    protected Quaternion mTargetRotation;
    protected Collider mCol;

    //Patrol Variables
    protected Vector3 mStartingPoint;
    public float mPatrolDistance;
    protected float mCounter;
    protected float mDistFromStart;
    public enum State { patrolling = 0, seeking = 1, attacking = 2, returning = 3 }
    public State mCurState = State.patrolling;
    protected float mPreviousDist = 0;

    //References
    public Transform mArms { get; private set; } //maybe should just be private
    

    //Tracking variables
    protected List<GameObject> mTargets;
    protected GameObject mMainTarget; //not sure just yet if this should be a Gameobject or Transform
    protected float mTargetDistance; //Seems completely useless
    [SerializeField]
    protected float mMaxTrackingDistance;
    [SerializeField]
    protected float mAttackDistance;

    [System.Flags]
    public enum RelativeDirections { down = 1, right = 2, up = 4, left = 8 };


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

    //NavMesh functionality
    protected CustomNavMesh _mNavMesh;
    public CustomNavMesh mNavMesh
    {
        get
        {
            if (_mNavMesh != null)
                return _mNavMesh;
            else
                return null;
        }
    }

    private Selector TestTreeRoot; //probably needs a repeater
    private Sequence Chasing;
    private Repeater ChasingLoop;
    private ActionNode HasTargetNode;
    private ActionNode TargetInRangeNode;
    private ActionNode TargetInSightNode;
    private Selector Approaching;
    private Sequence Horizontal;
    private Sequence Vertical;
    private ActionNode HorizontalReachNode;
    private ActionNode VerticalReachNode;
    private ActionNode HorizontalApproachNode;
    private ActionNode VerticalApproachNode;
    private Selector Shift;
    private ActionNode AxisAllignedShiftNode;
    private ActionNode ScanNode;
    private ActionNode AttackNode;
    private Selector Patrolling;
    private ActionNode PatrolNode;
    private ActionNode ReturnToPatrolNode;

    // Use this for initialization
    void Start()
    {
        _mGravShifter = GetComponent<GravityShifter>();
        if (_mGravShifter == null)
        {
            System.Console.Error.WriteLine("Warning: " + name + " was not given a GravityShifter component!");
        }

        _mNavMesh = GetComponent<CustomNavMesh>();
        if (_mNavMesh == null)
        {
            System.Console.Error.WriteLine("Warning: " + name + " was not given a CustomNavMesh component!");
        }

        SetPatrolPoint(transform.position);

        mArms = transform.Find("Arms");
        mArms.localEulerAngles = new Vector3(DEFAULT_ARM_ROTATION, 0, 0);

        mTargets = new List<GameObject>();
        //mMainTarget = GameObject.Find("Player"); //For testing purposes

        mCol = GetComponent<Collider>();
        if (mCol == null)
            System.Console.Error.WriteLine("Warning: " + name + " was not given a Collider!");

        //All this should probably be in its own function so it's not completely cluttering the start function
        HasTargetNode = new ActionNode(HasTarget);
        TargetInRangeNode = new ActionNode(TargetInRange);
        TargetInSightNode = new ActionNode(TargetInSight);
        HorizontalReachNode = new ActionNode(TargetInHorizontalReach);
        VerticalReachNode = new ActionNode(TargetInVerticalReach);
        HorizontalApproachNode = new ActionNode(HorizontalApproach);
        VerticalApproachNode = new ActionNode(VerticalApproach);

        AxisAllignedShiftNode = new ActionNode(AxisAllignedShift);
        ScanNode = new ActionNode(Scan);
        Shift = new Selector(new List<Node> { AxisAllignedShiftNode, ScanNode });

        AttackNode = new ActionNode(Attack);

        Horizontal = new Sequence(new List<Node> {HorizontalReachNode, HorizontalApproachNode });
        Vertical = new Sequence(new List<Node> { VerticalReachNode, VerticalApproachNode }); 

        Approaching = new Selector(new List<Node> { Horizontal, Vertical, Shift }); //Will eventually need a node for shifting here

        Chasing = new Sequence(new List<Node> { HasTargetNode, TargetInRangeNode, TargetInSightNode, Approaching, AttackNode });
        //ChasingLoop = new Repeater(Chasing);

        PatrolNode = new ActionNode(Patrol);
        ReturnToPatrolNode = new ActionNode(ReturnToPatrol);

        Patrolling = new Selector(new List<Node> { PatrolNode, ReturnToPatrolNode });

        TestTreeRoot = new Selector(new List<Node> { Chasing, Patrolling });
    }

    // Update is called once per frame
    void Update()
    {
        TestTreeRoot.Evaluate();

        //mNavMesh.AdjustOrientation(mGravShifter.GetMovementVector(), mGravShifter.GetGravityNormal(), mIntendedDirection, mBodyRotationSpeed);

        
    }

    public void SetPatrolPoint(Vector3 pCenter)
    {
        mStartingPoint = pCenter;
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
                    //The player takes the highest precedence
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

    public NodeStates HasTarget()
    {
        return mMainTarget != null ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    public NodeStates TargetInRange()
    {
        if (Vector3.Distance(transform.position, mMainTarget.transform.position) > mMaxTrackingDistance)
        {
            print("Lost its target");
            mTargets.Remove(mMainTarget);
            mMainTarget = null;
            mTargetDistance = 0;
            return NodeStates.FAILURE;
        }
        print("target in range");
        return NodeStates.SUCCESS;
    }

    /**
     * Might be able to replace with its own class
     */
    public NodeStates TargetInSight()
    {
        LayerMask lMask = ~(1 << 9 | 1 << 12); //9 = PlayerBullet, 12 = Enemy
        if (Physics.Raycast(transform.position, mMainTarget.transform.position - transform.position, out RaycastHit lSight, mMaxTrackingDistance, lMask))
        {
            return ReferenceEquals(lSight.collider.gameObject, mMainTarget.transform.gameObject) ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }

    public NodeStates TargetInHorizontalReach()
    {
        print("TargetInHorizontalReach called");
        float lHeight = mCol.bounds.extents.y + 0.05f; //added a slight increase
        if ((int)mGravShifter.mCurGravity % 2 == 1) //East or West gravity
        {
            return Mathf.Abs((mMainTarget.transform.position - transform.position).x) < lHeight ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            return Mathf.Abs((mMainTarget.transform.position - transform.position).y) < lHeight ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
    }

    /**
     * Calculates if the target can be reached without the need for shifting
     */
    public virtual NodeStates TargetInVerticalReach()
    {
        print("TargetInVerticalReach called");
        if ((int)mGravShifter.mCurGravity % 2 == 1)
        {
            //return Mathf.Abs((mMainTarget.transform.position - transform.position).x) <= mMaxTrackingDistance ? NodeStates.SUCCESS : NodeStates.FAILURE; //Purely here for testing purposes
            return Mathf.Abs((mMainTarget.transform.position - transform.position).x) <= mAttackDistance ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
        else
        {
            //return Mathf.Abs((mMainTarget.transform.position - transform.position).y) <= mMaxTrackingDistance ? NodeStates.SUCCESS : NodeStates.FAILURE; //Purely here for testing purposes
            return Mathf.Abs((mMainTarget.transform.position - transform.position).y) <= mAttackDistance ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }
    }

    public NodeStates HorizontalApproach()
    {
        print("HorizontalApproach being called");
        
        if (Vector3.Distance(transform.position, mMainTarget.transform.position) > mAttackDistance)
        {
            mNavMesh.isStopped = false;
            mNavMesh.SetDestination(mMainTarget.transform.position);
            return NodeStates.RUNNING;
        }
        else
        {
            mNavMesh.isStopped = true;
            return NodeStates.SUCCESS;
        }
    }

    public NodeStates VerticalApproach()
    {
        print("VerticalApproach being called");
        //Aim if not already
        Vector3 lTargetLocalPosition = transform.InverseTransformPoint(mMainTarget.transform.position);

        lTargetLocalPosition.y = 0;

        if (Vector3.Distance(transform.position, lTargetLocalPosition) > 0.5f) //CAUTION: 0.5f is here for testing; should be more precise in the future
        {
            mNavMesh.isStopped = false;
            mNavMesh.SetDestination(transform.TransformPoint(lTargetLocalPosition));
            return NodeStates.RUNNING;
        }
        else
        {
            mNavMesh.isStopped = true;
            return NodeStates.SUCCESS;
        }
    }

    /// <summary>
    /// Checks if any of the surfaces close to the target are directly lined up with a cardinal direction
    /// </summary>
    /// <returns>SUCCESS if true, else FAILURE</returns>
    public NodeStates AxisAllignedShift() //Name pending
    {
        print("AxisAllignedShift being called");

        int lLayerMask = 1 << 11;
        Collider[] lSurfaces = Physics.OverlapSphere(mMainTarget.transform.position, mAttackDistance, lLayerMask); //11 = environment layer
        byte lScanDirections = 15; //1111

        //Scan for suitable surfaces to shift to
        Vector3 lDir = (mMainTarget.transform.position - transform.position).normalized;
        float lAngle = Vector3.SignedAngle(-transform.up, lDir, Vector3.forward);

        int lRoundedAngle = (int)lAngle; //should be a more accurate rounding operations
        lRoundedAngle = (lRoundedAngle + 360) % 360; //Gets the angle from [0, 360) instead of [-180, 180]

        if (((lRoundedAngle / 30) % 3) % 2 == 0) //within 30 degrees of a cardinal direction
        {
            int lRoundedDiv = lRoundedAngle / 90;

            lScanDirections &= (byte)(1 << lRoundedDiv);

            if (lScanDirections != 1) //This really should never be 1 since this whole function depends on line of sight with a target
            {
                int lLog = (int)Mathf.Log(2, lScanDirections);
                mGravShifter.ShiftGravity(lLog);
            }
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

            /**
             * Scan directions follow LURD - Left, Up, Right, Down
             * Only possible combinations are 0011, 0110, 1001, and 1100
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
            for (int i = 0; i < 2; ++i)
            {
                Physics.Raycast(transform.position, lPossibleVectors[i], out lScannedSurfaces[i], mMaxTrackingDistance, 11);
            }

            int lValidSurfaces = 0;
            for (int i = 0; i < 2; ++i)
            {
                if (lSurfaces.Contains(lScannedSurfaces[i].collider))
                {
                    lValidSurfaces += (i + 1);
                }
            }
            int lVertical = 2 * ((lScanDirections & 4) / 4); //Will return 0 if down, or 2 if up
            int lHorizontal = 1 + 2 * ((lScanDirections & 8) / 8); //Will return 1 if right, or 3 if left
            switch (lValidSurfaces)
            {
                case 0:
                    return NodeStates.FAILURE;
                case 1: //only the first one
                    mGravShifter.ShiftGravity(lVertical); 
                    return NodeStates.SUCCESS;
                case 2: //only the second
                    mGravShifter.ShiftGravity(lHorizontal);
                    return NodeStates.SUCCESS;
                case 3: //both
                    if (Vector3.Distance(transform.position, lScannedSurfaces[0].collider.transform.position)
                        < Vector3.Distance(transform.position, lScannedSurfaces[1].collider.transform.position))
                    {
                        mGravShifter.ShiftGravity(lVertical);
                    }
                    else
                    {
                        mGravShifter.ShiftGravity(lHorizontal);
                    }
                    return NodeStates.SUCCESS;
            }
        }

        Debug.LogError("Warning: The \"TargetVisibleShift\" function completed without any viable options");
        return NodeStates.FAILURE; //Here for default
    }

    protected virtual NodeStates Scan()
    {
        print("Scan being called");

        return NodeStates.FAILURE;
    }

    protected virtual NodeStates Attack()
    {
        //TODO
        print("Attacking");
        return NodeStates.SUCCESS;
    }

    protected virtual NodeStates Patrol()
    {
        //TODO

        return NodeStates.SUCCESS;
    }

    protected virtual NodeStates ReturnToPatrol()
    {
        //TODO

        return NodeStates.SUCCESS;
    }

    private void OnTriggerEnter(Collider pCol)
    {
        if (pCol.tag == "Player" || pCol.tag == "Serviceable")
        {
            mTargets.Add(pCol.gameObject);
            PrioritizeTargets();
        }
    }
}
