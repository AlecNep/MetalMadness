using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Explodable
{
    protected GameObject yellowLight;
    protected GameObject blueLight;
    protected GameObject redLight; //just in case we need it
    protected bool isAlerted = false;

    // Start is called before the first frame update
    void Awake()
    {
        Transform lLights = transform.Find("Lights");
        yellowLight = lLights.Find("Yellow light").gameObject;
        blueLight = lLights.Find("Blue light").gameObject;
        redLight = lLights.Find("Red light").gameObject;

        yellowLight.SetActive(false);
        redLight.SetActive(false);
        blueLight.SetActive(true);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public virtual void ResetDrone() { }

    public void SetIdleMode()
    {
        yellowLight.SetActive(false);
        blueLight.SetActive(true);
        redLight.SetActive(false);
        isAlerted = false;
    }

    public void SetAlertMode()
    {
        blueLight.SetActive(false);
        yellowLight.SetActive(true);
        isAlerted = true;
    }
}
