using UnityEngine;

public class Bullet : MonoBehaviour {

    Rigidbody mRb;

    [SerializeField]
    float mSpeed;
    float mLifespan;
    [SerializeField]
    float mMaxLifespan;

	// Use this for initialization
	void Awake () {
		mRb = GetComponent<Rigidbody>();
        mLifespan = 0;

        //mRb.velocity = transform.forward * mSpeed;
    }

    

    // Update is called once per frame
    void Update () {
        mRb.velocity = transform.right * mSpeed;
        mLifespan += Time.deltaTime;
        if (mLifespan >= mMaxLifespan)
            Destroy(gameObject);
	}

    public void SetDirection(Vector3 pDir)
    {
        mRb.velocity = pDir * mSpeed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
