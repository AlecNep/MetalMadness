using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    protected GameObject bounds;
    protected GameObject interactableObject;

    //public virtual void Interact() { }
    public virtual void Interact(string input="") {}
    protected string input;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.player.interactableObject = this;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.player.interactableObject = null;
        }
    }
}
