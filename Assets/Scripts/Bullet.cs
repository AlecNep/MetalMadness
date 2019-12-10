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
    }

    

    // Update is called once per frame
    void Update () {
        //mRb.velocity = transform.right * mSpeed; //FUCK THIS FUCKING LINE FUCKING ALL MY FUCKING SHIT UP. F U C K
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
        print(collision.collider.name);
        Destroy(gameObject);
    }
}
