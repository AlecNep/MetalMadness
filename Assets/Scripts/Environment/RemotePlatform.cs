using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlatform : MonoBehaviour
{
    public float mMovementSpeed;
    public float mMovementDistance;
    private float mStartingPoint;
    public float mLength;
    private BoxCollider mCol;
    private Transform mCube;

    // Start is called before the first frame update
    void OnValidate()
    {
        mCol = GetComponent<BoxCollider>();
        mCube = transform.GetChild(0);
        mCol.size = mCube.localScale = new Vector3(mLength, 0.2f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
