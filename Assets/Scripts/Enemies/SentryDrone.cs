using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SentryDrone : Drone
{
    AIDestinationSetter aiDestination;

    [SerializeField]
    float distanceThreshold;
    bool SDTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        aiDestination = GetComponent<AIDestinationSetter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiDestination.target != null && Vector3.Distance(transform.position, aiDestination.target.position) <= distanceThreshold && !SDTriggered)
        {//only call this once
            SDTriggered = true; //this line is probably redundant

            LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1 << 15; //environment, enemies, destructible, and doors
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, aiDestination.target.position - transform.position, out hit, distanceThreshold, layers))
                StartCoroutine(SelfDestruct());
        }
        if (Mathf.Abs(transform.position.z) > 0.1) //reset Z-value to keep it at 0
            transform.position -= Vector3.forward * transform.position.z;
    }

    public override void ResetDrone()
    {
        SetIdleMode();
        SDTriggered = false;
    }

    private IEnumerator SelfDestruct()
    {
        aiDestination.target = null;
        blueLight.SetActive(false);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            aiDestination.target = GameManager.Instance.player.transform;
            SetAlertMode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") 
        {
            if (aiDestination.target != null && aiDestination.target.name == "Player") //could already be chasing the player if theyre shot; no need to check again
            {
                return;
            }
            LayerMask layers = 1 << 11 | 1 << 12 | 1 << 13 | 1 << 15; //environment, enemies, destructible, and doors
            RaycastHit hit;
            Vector3 direction = other.transform.position - transform.position;
            if (!Physics.Raycast(transform.position, direction, out hit, direction.magnitude, layers))
            {
                aiDestination.target = other.transform;
                if (!isAlerted)
                    SetAlertMode();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            aiDestination.target = null;
            SetIdleMode();
        }
    }
}
