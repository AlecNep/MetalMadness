using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : Interactive
{
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField]
    private float delayTimer;
    [SerializeField]
    private float speed;
    private bool isOpen = false;

    public enum Orientation { left = 0, up = 1, right = 2, down = 3 }
    [SerializeField]
    public Orientation opensTo;

    private Transform door;

    private void OnValidate()
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * (90 * (int)opensTo));
    }

    protected void Awake()
    {
        door = transform.Find("Door");
        if (door == null)
        {
            Debug.LogError("Object " + name + " at " + transform.position + " does not have a door!");
        }

        startPos = door.localPosition;
        endPos = startPos + Vector3.right * door.localScale.x;
    }

    public override void Interact()
    {
        if (isOpen)
        {
            //Close
            StartCoroutine(Move(startPos));
        }
        else
        {
            //Open
            StartCoroutine(Move(endPos));
        }
        isOpen = !isOpen;
    }

    //while loop somehow isn't finishing
    private IEnumerator Move(Vector3 destination)
    {
        float delay = 0;
        while (delay < delayTimer)
        {
            yield return null;
            delay += Time.deltaTime;
        }

        while (Vector3.Distance(door.localPosition, destination) > 0)
        {
            door.localPosition = Vector3.MoveTowards(door.localPosition, destination, speed * Time.deltaTime);

            if (Mathf.Approximately(door.localPosition.x, destination.x))
            {
                door.localPosition = destination;
                break;
            }

            yield return null;
        }        
    }

    public void ForceOpen()
    {
        StartCoroutine(Move(endPos));
        isOpen = true;
    }

    public void ForceClose()
    {
        StartCoroutine(Move(startPos));
        isOpen = false;
    }
}
