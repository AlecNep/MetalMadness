using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : Damageable
{
    [SerializeField]
    protected GameObject explosion;
    [SerializeField]
    protected float explosionForce;
    [SerializeField]
    protected float explosionRadius;
    [SerializeField]
    protected float camShakeMagnitude;
    [SerializeField]
    protected float camShakeTime;
    private AudioSource explosionSound;

    [SerializeField]
    protected float damage;

    private void Awake()
    {
        explosionSound = GetComponent<AudioSource>();
        if (explosionSound == null)
            print("shitfuck");
    }

    protected void Explode()
    {
        LayerMask entities = 1 << 8 | 1 << 12 | 1 << 13; //Player, enemy, and destructible layers
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, entities);

        foreach (Collider col in cols)
        {
            if (col.gameObject == gameObject)
                continue;
            if (col.tag == "Player" || col.tag == "Enemy") //this if-statement might be completely useless
            {
                Damageable victim = col.GetComponent<Damageable>();
                float value = (explosionRadius - Vector3.Distance(transform.position, col.transform.position)) / explosionRadius;
                victim.ChangeHealth(-damage * (value));
            }
            else
                print("explosion caught " + col.name);
            col.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);

        }

        GameManager.Instance.MainCamera.ScreenShake(camShakeMagnitude, camShakeTime);
        explosionSound.Play();
        if (!explosionSound.isPlaying)
            print("shitfuck2");

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public override void Die()
    {
        Explode(); //There's probable a better way to do this, but this should work for now
    }
}
