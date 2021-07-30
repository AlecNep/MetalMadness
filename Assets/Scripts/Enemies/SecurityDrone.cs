using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SecurityDrone : Explodable
{
    AIDestinationSetter aiDestination;
    Rigidbody rb;

    GameObject yellowLight;
    GameObject blueLight;
    GameObject redLight;

    [SerializeField]
    float fireRate;
    float fireDelay = 0;

    [SerializeField]
    GameObject laser;

    private bool hasTarget = false;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        aiDestination = GetComponent<AIDestinationSetter>();

        Transform lLights = transform.Find("Lights");
        yellowLight = lLights.Find("Yellow light").gameObject;
        blueLight = lLights.Find("Blue light").gameObject;

        yellowLight.SetActive(false);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.z) > 0.1)
            transform.position -= Vector3.forward * transform.position.z;

        if (fireDelay > 0)
        {
            fireDelay -= Time.deltaTime;
        }
        if (fireDelay < 0)
        {
            fireDelay = 0;
        }

        if (aiDestination.target != null && fireDelay == 0)
        {
            Fire();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            aiDestination.target = other.transform;
            blueLight.SetActive(false);
            yellowLight.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            aiDestination.target = null;
            yellowLight.SetActive(false);
            blueLight.SetActive(true);
        }
    }

    private void Fire()
    {
        fireDelay = fireRate;
        
        Vector3 lDirection = Vector3.Normalize(aiDestination.target.position - transform.position);
        float angle = Vector3.Angle(Vector3.right, lDirection);
        Vector3 cross = Vector3.Cross(Vector3.right, lDirection);
        if (cross.z < 0)
            angle *= -1;
        Vector3 lOrientation = angle * Vector3.forward;
        GameObject lBullet = Instantiate(laser, transform.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

        lBullet.GetComponent<Bullet>().SetDirection(lDirection);
    }
}
