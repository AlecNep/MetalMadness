using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermite : Bullet {

    private IEnumerator mSticking;
    

    // Use this for initialization
    void Start () {
        mSticking = StickingSequence();
	}

    private new void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody)
        {
            gameObject.AddComponent<FixedJoint>();
            GetComponent<FixedJoint>().connectedBody = col.rigidbody;

            StartCoroutine(mSticking);
        }
    }

    private IEnumerator StickingSequence()
    {
        //will probably need to disable their colliders or turn them into triggers
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
