using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    public float mMovementSpeed;
    public float mMovementDistance;
    private float mStartingPoint; 
    public float mLength;
    private float mCounter;

	// Use this for initialization
	void Start () {
        gameObject.tag = "MovingPlatform";
        mStartingPoint = transform.position.x;
        transform.localScale = new Vector3(mLength, 0.2f, 3);
	}
	
	// Update is called once per frame
	void Update () {
        mCounter += Time.deltaTime;
        transform.position = new Vector3(mStartingPoint + mMovementDistance * Mathf.Sin(mCounter * mMovementSpeed),
            transform.position.y, transform.position.z);
	}
}
