using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

    private PlayerControls mPlayer;
    private RectTransform mArrow;

	// Use this for initialization
	void Start () {
        mPlayer = GameObject.Find("Player").GetComponent<PlayerControls>();
        mArrow = transform.GetChild(0).GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        mArrow.rotation = Quaternion.Euler(Vector3.forward * -90 * (int)mPlayer.mCurGravity);
        //negative because the rotation for the rect transform is backwards
        //why in the ever-loving Christ is it backwards?????
	}
}
