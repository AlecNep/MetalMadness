using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlatform : Interactive
{
    public float mMovementSpeed;
    public float mMovementDistance;
    private float mStartingPoint;
    public float mLength;
    private BoxCollider mCol;
    private Transform mCube;
    public bool mVertical;


    // Start is called before the first frame update
    void OnValidate()
    {
        mCol = GetComponent<BoxCollider>();
        mCube = transform.GetChild(0);
        mCol.size = mCube.localScale = new Vector3(mLength, 0.2f, 3f);

        int lRot;
        if (mVertical)
        {
            //mStartingPoint = transform.position.y;
            lRot = 1;
        }
        else
        {
            //mStartingPoint = transform.position.x;
            lRot = 0;
        }

        transform.rotation = Quaternion.Euler(0, 0, 90 * lRot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(string input = "")
    {
        if (input != "")
        {
            if (input == "Left")
            {

            }
        }
    }
}
