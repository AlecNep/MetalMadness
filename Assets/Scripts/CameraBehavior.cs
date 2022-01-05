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
    private Transform audioListener;

    // Start is called before the first frame update
    void Start()
    {
        customCam = transform.Find("Camera");
        audioListener = transform.Find("AudioListener");
        
        if (customCam == null)
            Debug.LogError("Somehow the custom camera doesn't have a camera child object");
        playerRef = GameManager.Instance.player;
        audioListener.localPosition = Vector3.back * defaultDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerRef.transform.position + Vector3.forward * defaultDistance;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.forward * playerRef.mGravShifter.GetShiftAngle()), rotationSpeed);
        customCam.localPosition = Vector3.up * heightAbovePlayer;
        customCam.localRotation = Quaternion.Euler(Vector3.right * angle);
    }

    public void ScreenShake(float duration, float magnitude)
    {
        StartCoroutine(_ScreenShake(duration, magnitude));
    }

    private IEnumerator _ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = customCam.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            customCam.localPosition = Vector3.MoveTowards(customCam.localPosition, new Vector3(customCam.localPosition.x + x, customCam.localPosition.y + y, originalPos.z), 0.1f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        customCam.localPosition = originalPos;
    }
}
