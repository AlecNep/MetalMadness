using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloserTrigger : Interactive
{
    [SerializeField]
    private TestDoor[] doors;

    private bool activatedOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        
    }

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

    protected new void OnTriggerExit(Collider other)
    {

    }
}
