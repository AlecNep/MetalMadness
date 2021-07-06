using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SentryDrone : MonoBehaviour
{
    Transform playerRef;
    AIDestinationSetter aiDestination;
    [SerializeField]
    float distanceThreshold;
    bool isInRange = false;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    float explosionForce;
    [SerializeField]
    float explosionRadius;

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
    }

    private IEnumerator SelfDestruct()
    {
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

        Instantiate(explosion, transform.position, transform.rotation);
        GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 3f, ForceMode.Impulse);
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
