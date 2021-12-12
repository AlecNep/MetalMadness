using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePanel : Interactive
{
    public Interactive[] interactables;

    public override void Interact(string input = "")
    {
        //there is probably a better way to do this
        if (input == "")
        {
            foreach (Interactive i in interactables)
            {
                i.Interact();
            }
        }
        else
        {
            foreach (Interactive i in interactables)
            {
                i.Interact(input);
            }
        }
    }
}
