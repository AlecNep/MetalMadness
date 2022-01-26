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
    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void Explode()
    {
        rb.velocity = Vector3.zero;
        //Should probably put all this into the actual explosion script
        LayerMask entities = 1 << 8 | 1 << 12 | 1 << 13; //Player, enemy, and destructible layers
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, entities); //something about this line causes a StackOverflowException; address ASAP!

        foreach (Collider col in cols)
        {
            if (col.gameObject == gameObject)
                continue;
            if (col.gameObject.TryGetComponent(out Damageable victim)) //should probably be replaced with TryGetComponent
            {
                LayerMask environmentLayers = 1 << LayerMask.NameToLayer("Environment") | 1 << LayerMask.NameToLayer("Doors"); //environment, enemies, destructible, and doors
                RaycastHit hit;
                Vector3 direction = col.transform.position - transform.position;
                if (!Physics.Raycast(transform.position, direction, out hit, direction.magnitude, environmentLayers))
                {
                    float value = (explosionRadius - Vector3.Distance(transform.position, col.transform.position)) / explosionRadius;
                    victim.ChangeHealth(-damage * value);
                    col.GetComponent<Rigidbody>().AddExplosionForce(explosionForce * value, transform.position, explosionRadius, 0f, ForceMode.Impulse);
                }

                //Damageable victim = col.GetComponent<Damageable>();
                
            }
            else
                print("explosion caught " + col.name);
            

        }
        health = 0;
        gameObject.SetActive(false);
        Instantiate(explosion, transform.position, transform.rotation);
    }

    public override void Die()
    {
        Explode(); //There's probable a better way to do this, but this should work for now
    }
}
