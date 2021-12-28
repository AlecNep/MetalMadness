using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField]
    private float defaultDistance;
    [SerializeField]
    private float heightAbovePlayer;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float rotationSpeed;

    private Transform customCam;
    private PlayerControls playerRef;
    private 

    // Start is called before the first frame update
    void Start()
    {
        customCam = transform.Find("Camera");
        if (customCam == null)
            Debug.LogError("Somehow the custom camera doesn't have a camera child object");
        playerRef = GameManager.Instance.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerRef.transform.position + Vector3.forward * defaultDistance;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.forward * playerRef.mGravShifter.GetShiftAngle()), rotationSpeed);
        customCam.localPosition = Vector3.up * heightAbovePlayer;
        customCam.localRotation = Quaternion.Euler(Vector3.right * angle);
    }
}
