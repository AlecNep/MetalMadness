using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : Interactive
{
    [SerializeField]
    private float delayTimer;
    [SerializeField]
    private float movementSpeed;
    public enum Orientation { Horizontal = 0, Vertical = 1}
    [SerializeField]
    public Orientation orientation;

    private Vector3 movementAxis;
    private bool isOpen = false;
    private float distance;

    private Transform door1;
    private Transform door2;

    private void OnValidate()
    {
        if (orientation == 0) //Is horizontal
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else //Is Vertical
        {
            transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (orientation == 0) //Is horizontal
        {
            movementAxis = Vector3.right;
        }
        else //Is Vertical
        {
            movementAxis = Vector3.up;
        }

        door1 = transform.Find("Door1");
        door2 = transform.Find("Door2");

        distance = door1.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        
    }

    

    /*private IEnumerator Move()
    {
        float door1XDestination = 
    }*/
}
