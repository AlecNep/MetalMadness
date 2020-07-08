using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private float mDuration;
    private float mMaxDuration = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mDuration >= mMaxDuration)
            Destroy(gameObject);
        else
            mDuration += Time.deltaTime;
	}
}
