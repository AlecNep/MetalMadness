using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : Bullet {

    //bool mDetonation = false;
    private IEnumerator mDetonation;
    private GameObject mLight;
    public GameObject mExplosion;

	// Use this for initialization
	void Start () {
        mRb.centerOfMass = new Vector3(0, 0.15f, 0); //doesn't seem to work
        mDetonation = DetonationSequence();
        mLight = transform.GetChild(transform.childCount - 1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public new void SetDirection(Vector3 pDir) //didn't fix the problem
    {
        mRb.AddForce(pDir * mSpeed, ForceMode.Impulse);
    }

    public new void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody)
        {
            gameObject.AddComponent<HingeJoint>();
            GetComponent<HingeJoint>().connectedBody = col.rigidbody;
            StartCoroutine(mDetonation);
        }
    }

    private IEnumerator DetonationSequence()
    {
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 2; i++)
        {
            mLight.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            mLight.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.1f);

        //TODO: Explosion
        Instantiate(mExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
