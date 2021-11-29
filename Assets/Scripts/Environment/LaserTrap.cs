using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    private Transform base1;
    private Transform base2;
    private Transform laser;

    [SerializeField]
    private float length;
    [SerializeField]
    private float damage;

    [Range(0,1)]
    public float powerOnPercentage;
    private float powerTimer = 0;


    private void OnValidate()
    {
        SetUpLaser();
        
    }

    void Awake()
    {
        SetUpLaser();
    }

    private void Start()
    {
        laser.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        powerTimer += Time.deltaTime;
        if (IsPoweredOn())
        {
            laser.gameObject.SetActive(true);
        }
        else
        {
            laser.gameObject.SetActive(false);
        }
    }

    public bool IsPoweredOn()
    {
        return Mathf.Abs(Mathf.Sin(powerTimer)) >= (1 - powerOnPercentage);
    }

    private void SetUpLaser()
    {
        if (base1 == null || base2 == null || laser == null)
        {
            base1 = transform.Find("Base 1");
            base2 = transform.Find("Base 2");
            laser = transform.Find("Laser");
        }

        float lBaseHeight = base1.localScale.y;
        base1.localPosition = Vector3.up * length;
        base2.localPosition = Vector3.down * length;
        laser.localScale = new Vector3(2.5f, length - lBaseHeight, 2.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPoweredOn() && other.TryGetComponent(out Damageable d))
        {
            print("should be damaging " + other.name);
            d.ChangeHealth(-damage);
        }
    }
}
