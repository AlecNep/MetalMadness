using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float healthAtTime;
    public float rivetAmmoAtTime;
    public float shrapnelAmmoAtTime;
    public float bulletsAtTime;
    public enum Orientation { South = 0, East = 1, North = 2, West = 3 }
    public Orientation orientation;

    private bool alreadyTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !alreadyTriggered)
        {
            alreadyTriggered = true;
            PlayerControls.SetCheckpoint(this);
            PlayerControls playRef = GameManager.Instance.player;
            healthAtTime = playRef.GetHealth();
            //TODO: code for ammo
        }
    }
}
