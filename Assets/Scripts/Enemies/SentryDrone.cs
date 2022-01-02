using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SentryDrone : Explodable
{
    AIDestinationSetter aiDestination;
    Rigidbody rb;

    [SerializeField]
    float distanceThreshold;
    bool isInRange = false;

    GameObject redLight;
    GameObject yellowLight;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        aiDestination = GetComponent<AIDestinationSetter>();

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
            isInRange = true; //this line is probably redundant

            LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1 << 15; //environment, enemies, destructible, and doors
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, aiDestination.target.position - transform.position, out hit, distanceThreshold, layers))
                StartCoroutine(SelfDestruct());
        }
        if (Mathf.Abs(transform.position.z) > 0.1) //reset Z-value to keep it at 0
            transform.position -= Vector3.forward * transform.position.z;
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

        Explode();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1 << 15; //environment, enemies, destructible, and doors
            RaycastHit hit;
            Vector3 direction = other.transform.position - transform.position;
            if (!Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(other.transform.position, transform.position), layers))
            {
                aiDestination.target = other.transform;
            }
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
