using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{
    public float healthAtTime;
    public float rivetAmmoAtTime;
    public float shrapnelAmmoAtTime;
    public float bulletsAtTime;
    public enum Orientation { South = 0, East = 1, North = 2, West = 3 }
    public Orientation orientation;

    private bool alreadyTriggered = false;

    public void SetCheckpointOrientation(int o)
    {
        orientation = (Orientation)o;
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

    public static Checkpoint CreateCheckpoint()
    {
        GameObject obj = new GameObject();
        Checkpoint check = obj.AddComponent<Checkpoint>();
        Debug.Log("Checkpoint.CreateCheckpoint(): could actually create a checkpoint? " + (check != null));
        Debug.Log("Checkpoint.CreateCheckpoint(): has a reference to the player? " + (GameManager.Instance.player != null));
        check.transform.position = GameManager.Instance.player.transform.position;
        try
        {
            check.alreadyTriggered = true;
            PlayerControls.SetCheckpoint(check);
            PlayerControls playerRef = GameManager.Instance.player;
            check.healthAtTime = playerRef.GetHealth();
        }
        catch (Exception e)
        {
            Debug.LogError("REALLY could not create a backup checkpoint");
            Debug.LogException(e);
        }
        

        return check;
    }

    private static Checkpoint CreateCheckpoint(Vector3 pos)
    {
        //Debug.Log("Checkpoint.CreateCheckpoint(Vector3): are we even given a position? " + (pos != null)); //Doesn't seem to be the problem
        Checkpoint check = CreateCheckpoint(); //seems to be the problem
        Debug.Log("Checkpoint.CreateCheckpoint(Vector3): could actually create a checkpoint? " + (check != null));
        check.transform.position = pos;

        return check;
    }

    public override string ToString()
    {
        string output = "Checkpoint.ToString:\n";

        output += "Checkpoint has a reference to the player? " + (GameManager.Instance.player != null) + " \n";
        output += "Player has a reference at all? " + PlayerControls.HasCheckpoint() + "\n";
        output += "Player has a reference to this particular checkpoint?" + (PlayerControls.GetCheckpoint() == this);
        //output += "Player's health at the time of reaching this checkpoint=" + 

        return output;
    }
}
