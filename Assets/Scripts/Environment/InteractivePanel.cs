using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePanel : Interactive
{
    public Interactive[] interactables;
    [SerializeField]
    private string data;

    public override void Interact()
    {
        if (data != "")
            _Interact(data);
        else
            _Interact();
    }

    public void _Interact(string input)
    {
        foreach (Interactive i in interactables)
        {
            i.Interact(input);
        }
    }

    public void _Interact()
    {
        foreach (Interactive i in interactables)
        {
            i.Interact();
        }
    }
}
