using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    protected GameObject bounds;
    protected GameObject interactableObject;


    protected void Awake()
    {
        bounds = transform.Find("Bounds").gameObject;
        if (bounds.TryGetComponent(out BoxCollider col))
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogError("Interactive object " + name + "Does not have collider in bounds object!");
        }

        interactableObject = transform.Find("Object").gameObject;
    }

    public virtual void Interact() { }

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
}
