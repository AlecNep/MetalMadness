using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Environment")
        {
            //Anchor

        }
        else
        {
            //might not be necessary
            print("hitting " + col.gameObject.name);
        }
    }
}
