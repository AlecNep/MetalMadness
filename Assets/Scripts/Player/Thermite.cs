using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermite : Bullet {

    //private IEnumerator mSticking; //probably completely useless
    [SerializeField]
    float counterIncrement;

    // Use this for initialization
    void Start () {
        //mSticking = StickingSequence();
	}

    private new void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody)
        {
            FixedJoint fj = gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = col.rigidbody;
            if (col.collider.tag == "Enemy")
            {
                Damageable dmg = col.collider.GetComponent<Damageable>();
                StartCoroutine(StickingSequence(dmg));
            }
            else
            {
                StartCoroutine(StickingSequence());
            }
        }
    }

    private IEnumerator StickingSequence()
    {
        print("entering sticking sequence 1");
        //will probably need to disable their colliders or turn them into triggers
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private IEnumerator StickingSequence(Damageable dmg)
    {
        print("entering sticking sequence 2");
        //will probably need to disable their colliders or turn them into triggers
        float counter = 0;
        while (counter < 2 && dmg != null)
        {
            dmg.ChangeHealth(-damage);
            yield return new WaitForSeconds(counterIncrement);
            counter += counterIncrement;
        }
        
        Destroy(gameObject);
    }
}
