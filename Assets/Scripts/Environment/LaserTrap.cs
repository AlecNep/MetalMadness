using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : Interactive
{
    protected Transform base1;
    protected Transform base2;
    protected Transform laser;

    [SerializeField]
    protected float length;
    [SerializeField]
    protected float damage;
    //private bool isOn = true;

    private void OnValidate()
    {
        SetUpLaser();
    }

    void Awake()
    {
        SetUpLaser();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SetUpLaser()
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

    public virtual bool IsPoweredOn()
    {
        return false;
        //return isOn;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPoweredOn() && other.TryGetComponent(out Damageable d))
        {
            d.ChangeHealth(-damage);
        }
    }
}
