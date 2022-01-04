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
    protected float damage;


    protected void Explode()
    {
        LayerMask entities = 1 << 8 | 1 << 12 | 1 << 13; //Player, enemy, and destructible layers
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, entities); //something about this line causes a StackOverflowException; address ASAP!

        foreach (Collider col in cols)
        {
            if (col.gameObject == gameObject)
                continue;
            if (col.tag == "Player" || col.tag == "Enemy") //should probably be replaced with TryGetComponent
            {
                LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1 << 15; //environment, enemies, destructible, and doors
                RaycastHit hit;


                Damageable victim = col.GetComponent<Damageable>();
                float value = (explosionRadius - Vector3.Distance(transform.position, col.transform.position)) / explosionRadius;
                victim.ChangeHealth(-damage * (value));
            }
            else
                print("explosion caught " + col.name);
            col.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);

        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public override void Die()
    {
        Explode(); //There's probable a better way to do this, but this should work for now
    }
}
