using System;
using System.Collections.Generic;
using UnityEngine;


public class DoorCloserTrigger : Interactive, ISaveable
{
    [SerializeField]
    private TestDoor[] doors;
    [SerializeField]
    private InteractivePanel[] panels;

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
            //There is probably a cleaner way to do this. Maybe with an XOR
            if (isResuseable)
            {
                foreach (TestDoor door in doors)
                {
                    door.ForceClose();
                }
                foreach (InteractivePanel panel in panels)
                {
                    panel.Interact(2);
                }
            }
            else if (!activatedOnce)
            {
                foreach (TestDoor door in doors)
                {
                    door.ForceClose();
                }
                foreach (InteractivePanel panel in panels)
                {
                    panel.Interact(2);
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
