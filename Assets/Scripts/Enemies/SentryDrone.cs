using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SentryDrone : MonoBehaviour
{
    Transform playerRef;
    AIDestinationSetter aiDestination;
    Rigidbody rb;

    [SerializeField]
    float distanceThreshold;
    bool isInRange = false;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    float explosionForce;
    [SerializeField]
    float explosionRadius;

    [SerializeField]
    float damage;
    [SerializeField]
    float health;

    GameObject redLight;
    GameObject yellowLight;

    // Start is called before the first frame update
    void Start()
    {
        aiDestination = GetComponent<AIDestinationSetter>();
        playerRef = GameObject.Find("Player").transform;
        if (playerRef == null)
            Debug.LogError("Drone was unable to find the player");

        Transform lLights = transform.Find("Lights");
        redLight = lLights.Find("Red light").gameObject;
        yellowLight = lLights.Find("Yellow light").gameObject;

        redLight.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (aiDestination.target != null && Vector3.Distance(transform.position, aiDestination.target.position) <= distanceThreshold && !isInRange)
        {//only call this once
            isInRange = true;
            //TODO
            StartCoroutine(SelfDestruct());
        }
        if (Mathf.Abs(transform.position.z) > 0.1)
            transform.position -= Vector3.forward * transform.position.z;
    }

    private IEnumerator SelfDestruct()
    {
        //Rigidbody targetRB = aiDestination.target.GetComponent<Rigidbody>();
        aiDestination.target = null;
        yellowLight.SetActive(false);
        redLight.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < 3; i++)
        {
            redLight.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            redLight.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.1f);

        LayerMask entities = ~(1 << 8 & 1 << 12);
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, entities);

        foreach (Collider col in cols)
        {
            if (col.transform.root == transform)
                continue;
            if (col.tag == "Player")
            {
                PlayerControls pc = col.GetComponent<PlayerControls>();
                pc.ChangeHealth(-damage * (Vector3.Distance(transform.position, col.transform.position) / explosionRadius));
            }
            else if (col.tag == "Enemy")
            {
                //TODO
            }
            col.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);

        }

        Instantiate(explosion, transform.position, transform.rotation);
        //targetRB.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            aiDestination.target = playerRef;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            aiDestination.target = null;
        }
    }
}
