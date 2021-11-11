using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : Interactive
{
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float speed;
    private bool isOpen = false;

    private Vector3 movementAxis;

    private new void Awake()
    {
        base.Awake();
        startPos = interactableObject.transform.position; //get world position
        endPos = startPos + (distance * interactableObject.transform.up);
        movementAxis = Vector3.zero; //probably unnecessary

        Vector3 diff = endPos - startPos;
        int nonZero = 0;
        for (int i = 0; i < 2; ++i) //Setting to 2 because we don't need the Z-axis
        {
            if (diff[i] != 0)
            {
                movementAxis[i] = diff[i] / Mathf.Abs(diff[i]); //Will give 1 if positive or -1 if negative, otherwise 0
                ++nonZero;
            }
        }
        if (nonZero > 1)
        {
            Debug.LogError("ERROR: Object " + name + " is not alligned with an axis! Current axis=" + movementAxis);
        }
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

    private IEnumerator Move(Vector3 destination)
    {
        

        while (Vector3.Distance(interactableObject.transform.position, destination) > 0)
        {
            interactableObject.transform.position += interactableObject.transform.TransformDirection(destination - interactableObject.transform.position) * speed;

            yield return null;
        }
        interactableObject.transform.position = destination;
    }
}
