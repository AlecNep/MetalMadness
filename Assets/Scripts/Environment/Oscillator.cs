using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    public float mMovementSpeed;
    public float mMovementDistance;
    private float mStartingPoint; 
    public float mLength;
    private float mCounter;
    private BoxCollider mCol;
    private Transform mCube;

    public bool mVertical;
    public bool mReversed;
    private float mMovementVar;

	// Use this for initialization
	void OnValidate () {
        //gameObject.tag = "MovingPlatform";
        mCol = GetComponent<BoxCollider>();
        mCube = transform.GetChild(0);
        mCol.size = mCube.localScale = new Vector3(mLength, 0.2f, 3f);
        int lRot;
        if (mVertical)
        {
            mStartingPoint = transform.position.y;
            lRot = 1;
        }
        else
        {
            mStartingPoint = transform.position.x;
            lRot = 0;
        }
        

        transform.rotation = Quaternion.Euler(0, 0, 90 * lRot);
	}

    private void Start()
    {
        if (mReversed)
            mMovementSpeed *= -1;
    }

    // Update is called once per frame
    void Update () {
        mCounter += Time.deltaTime;

        mMovementVar = mStartingPoint + mMovementDistance * Mathf.Sin(mCounter * mMovementSpeed);
        
        //Could probably just be done with localPosition instead
        Vector3 lPos = mVertical ? new Vector3(transform.position.x, mMovementVar, transform.position.z) : 
            new Vector3(mMovementVar, transform.position.y, transform.position.z);
        transform.position = lPos;
	}
}
