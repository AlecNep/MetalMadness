using System;
using System.Collections.Generic;
using UnityEngine;


public class DoorCloserTrigger : Interactive, ISaveable
{
    [SerializeField]
    private TestDoor[] doors;

    public bool isResuseable;
    private bool activatedOnce = false;

    [Serializable]
    private struct SaveData
    {
        public bool reuseable;
        public bool activated;
    }

    protected new void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isResuseable)
            {
                foreach (TestDoor door in doors)
                {
                    door.ForceClose();
                }
            }
            else if (!activatedOnce)
            {
                foreach (TestDoor door in doors)
                {
                    door.ForceClose();
                }
                activatedOnce = true;
            }
        }
    }

    public object CaptureState()
    {
        return new SaveData
        {
            reuseable = isResuseable,
            activated = activatedOnce
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        isResuseable = saveData.reuseable;
        activatedOnce = saveData.activated;
    }
}
