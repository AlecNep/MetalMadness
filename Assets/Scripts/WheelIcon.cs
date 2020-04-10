using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelIcon : MonoBehaviour {

    bool highlighted;
    GameObject mGlow;

	// Use this for initialization
	void Start () {
        mGlow = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Toggle()
    {
        highlighted = !highlighted; //probably unnecessary
        gameObject.SetActive(highlighted);
    }
}
