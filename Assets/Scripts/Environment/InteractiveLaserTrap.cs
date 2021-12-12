using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveLaserTrap : LaserTrap
{
    private bool _isOn;
    private bool isOn
    {
        get
        {
            return _isOn;
        }
        set
        {
            _isOn = value;
            laser.gameObject.SetActive(_isOn);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(string input="")
    {
        isOn = !isOn;
    }

    public override bool IsPoweredOn()
    {
        return isOn;
    }
}
