using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePanel : Interactive
{
    public Interactive[] interactables;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.player.interactableObject = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.player.interactableObject = null;
        }
    }

    public override void Interact()
    {
        foreach(Interactive i in interactables)
        {
            i.Interact();
        }
    }
}
