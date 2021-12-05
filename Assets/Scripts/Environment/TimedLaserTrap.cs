using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLaserTrap : LaserTrap
{
    [Range(0, 1)]
    public float powerOnPercentage;
    private float powerTimer = 0;
    [SerializeField]
    private float timeOffset;

    // Start is called before the first frame update
    void Start()
    {
        SetUpLaser();
        laser.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        powerTimer += Time.deltaTime;
        if (powerTimer > 2 * Mathf.PI)
        {
            powerTimer -= 2 * Mathf.PI;
        }
        if (IsPoweredOn())
        {
            laser.gameObject.SetActive(true);
        }
        else
        {
            laser.gameObject.SetActive(false);
        }
    }

    public override bool IsPoweredOn()
    {
        return Mathf.Abs(Mathf.Sin(powerTimer - timeOffset)) >= (1 - powerOnPercentage);
    }
}
