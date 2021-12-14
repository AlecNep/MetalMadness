using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoor : TestDoor
{
    [SerializeField]
    private float timeOpen;

    public override void Interact(string input = "")
    {
        if (input == "")
        {
            if (!isOpen)
            {
                StartCoroutine(TimedOpen());
            }
        }
    }

    public IEnumerator TimedOpen()
    {
        StartCoroutine(Move(endPos, delayTimer));

        yield return new WaitForSecondsRealtime(timeOpen);

        StartCoroutine(Move(startPos, 0));
    }
}
