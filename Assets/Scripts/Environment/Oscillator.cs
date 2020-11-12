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

	// Use this for initialization
	void Start () {
        gameObject.tag = "MovingPlatform";
        mCol = GetComponent<BoxCollider>();
        mCube = transform.GetChild(0);
        mStartingPoint = transform.position.x;
        mCol.size = mCube.localScale = new Vector3(mLength, 0.2f, 3f);
	}
	
	// Update is called once per frame
	void Update () {
        mCounter += Time.deltaTime;
        transform.position = new Vector3(mStartingPoint + mMovementDistance * Mathf.Sin(mCounter * mMovementSpeed),
            transform.position.y, transform.position.z);
	}
}
