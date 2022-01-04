using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SecurityDrone : Explodable
{
    private AIDestinationSetter aiDestination;
    private Rigidbody rb;

    private GameObject yellowLight;
    private GameObject blueLight;
    private GameObject redLight;

    [SerializeField]
    private float fireRate;
    private float fireDelay = 0;

    [SerializeField]
    private float camShakeMagnitude;
    [SerializeField]
    private float camShakeTime;

    [SerializeField]
    private GameObject laser;

    private bool hasTarget = false;
    private AudioSource laserSound;


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
        laserSound = GetComponent<AudioSource>();
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
                blueLight.SetActive(false);
                yellowLight.SetActive(true);
            }
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
        laserSound.Play();
        GameManager.Instance.MainCamera.ScreenShake(camShakeMagnitude, camShakeTime);
        GameObject lBullet = Instantiate(laser, transform.position, Quaternion.Euler(lOrientation)) as GameObject; //update soon

        lBullet.GetComponent<Bullet>().SetDirection(lDirection);
    }
}
