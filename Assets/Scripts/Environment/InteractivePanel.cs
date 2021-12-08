using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePanel : Interactive
{
    public Interactive[] interactables;

    public override void Interact()
    {
        foreach(Interactive i in interactables)
        {
            i.Interact();
        }
    }
}
