using System.Collections;
using System;
using UnityEngine;

public class TestDoor : Interactive, ISaveable
{
    protected Vector3 startPos;
    protected Vector3 endPos;
    [SerializeField]
    protected float delayTimer;
    [SerializeField]
    protected float speed;
    protected bool isMoving = false;
    protected bool isOpen = false;

    public enum Orientation { right = 0, up = 1, left = 2, down = 3 }
    [SerializeField]
    public Orientation opensTo;

    protected Transform door;
    protected AudioSource sound;

    [Serializable]
    private struct SaveData
    {
        public bool isOpen;
    }

    protected void OnValidate()
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
        sound = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        if (isOpen)
        {
            //Close
            StartCoroutine(Move(startPos, delayTimer));
        }
        else
        {
            //Open
            StartCoroutine(Move(endPos, delayTimer));
        }
        isOpen = !isOpen;
    }

    protected IEnumerator Move(Vector3 destination, float timer)
    {
        isMoving = true;
        if (timer > 0)
        {
            float delay = 0;
            while (delay < timer)
            {
                yield return null;
                delay += Time.deltaTime;
            }
        }
        sound.Play();
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
        isMoving = false;
    }

    public void ForceOpen()
    {
        StartCoroutine(_ForceOpen());
    }

    private IEnumerator _ForceOpen()
    {
        while (isMoving)
            yield return null;
        StartCoroutine(Move(endPos, delayTimer));
        isOpen = true;
    }

    public void ForceClose()
    {
        StartCoroutine(_ForceClose());
    }

    private IEnumerator _ForceClose()
    {
        while (isMoving)
            yield return null;
        StartCoroutine(Move(startPos, delayTimer));
        isOpen = false;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            isOpen = isOpen
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        isOpen = saveData.isOpen;
        if (isOpen)
        {
            door.localPosition = endPos;
        }
        else
        {
            door.localPosition = startPos;
        }
    }
}
