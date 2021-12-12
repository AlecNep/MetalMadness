using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloserTrigger : Interactive
{
    [SerializeField]
    private TestDoor[] doors;

    private bool activatedOnce = false;

    protected new void OnTriggerEnter(Collider other)
    {
        print(name + " detected " + other);
        if (!activatedOnce && other.tag == "Player")
        {
            foreach (TestDoor door in doors)
            {
                door.ForceClose();
            }
        }
        activatedOnce = true;
    }
}
