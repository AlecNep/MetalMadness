using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveLaserTrap : LaserTrap, ISaveable
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

    [Serializable]
    private struct SaveData
    {
        public bool isOn;
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

    public override void Interact()
    {
        isOn = !isOn;
    }

    public override bool IsPoweredOn()
    {
        return isOn;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            isOn = isOn
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        isOn = saveData.isOn;
    }
}
