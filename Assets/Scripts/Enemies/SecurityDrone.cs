using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SecurityDrone : Drone
{
    private AIDestinationSetter aiDestination;

    

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
