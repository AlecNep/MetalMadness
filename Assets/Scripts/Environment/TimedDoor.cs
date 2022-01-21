using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoor : TestDoor
{
    [SerializeField]
    private float timeOpen;
    [SerializeField]
    private InteractivePanel panel; //There's probably a better way to do this than giving the panel a door reference and giving the door a panel reference, but for now it will have to do

    public override void Interact()
    {
        if (!isOpen)
        {
            StartCoroutine(TimedOpen());
        }
    }

    public IEnumerator TimedOpen()
    {
        //Probably a better way to do this too instead of having 2 identical if-statements
        bool hasPanel = panel != null;
        if (hasPanel)
            panel.Interact(0);
        StartCoroutine(Move(endPos, delayTimer));

        yield return new WaitForSecondsRealtime(timeOpen);
        
        StartCoroutine(Move(startPos, 0));
        if (hasPanel)
        {
            while (isMoving)
                yield return null;
            panel.Interact(1);
        }
    }
}
