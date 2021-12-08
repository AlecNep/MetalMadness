using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorResetter : Interactive
{
    private void Awake()
    {
               
    }

    protected new void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {

        }
    }
}
