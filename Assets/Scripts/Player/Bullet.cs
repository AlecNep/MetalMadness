using UnityEngine;

public class Bullet : MonoBehaviour {

    protected Rigidbody mRb;

    [SerializeField]
    protected float mSpeed;
    float mLifespan;
    [SerializeField]
    protected float mMaxLifespan;
    [SerializeField]
    protected float damage;


    // Use this for initialization
    void Awake () {
		mRb = GetComponent<Rigidbody>();
        mLifespan = 0;
    }

    

    // Update is called once per frame
    void Update () {
        mLifespan += Time.deltaTime;
        if (mLifespan >= mMaxLifespan || Mathf.Abs(transform.position.z) >= 2)
            Destroy(gameObject);
	}

    public void SetDirection(Vector3 pDir)
    {
        mRb.velocity = pDir * mSpeed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "PlayerBullet")
        {
            Damageable victim = collision.collider.GetComponent<Damageable>();
            if (victim != null)
            {
                victim.ChangeHealth(-damage);
            }
            Destroy(gameObject);
        }
        
    }

}
