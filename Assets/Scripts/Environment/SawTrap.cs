using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{

    Transform blade;
    [SerializeField]
    float bladeSpeed;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float movementDistance;
    public AnimationCurve lerpCurve;
    float maxTime = Mathf.PI * 2;
    float timer = 0;
    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        blade = transform.Find("SawBlade");
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        blade.Rotate(0, 0, bladeSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= maxTime)
        {
            timer = 0;
        }

        float lValue = movementDistance * lerpCurve.Evaluate(timer * movementSpeed);

        transform.position = startingPosition + (-transform.up * lValue);
    }

    private void OnValidate()
    {
        if (lerpCurve.keys.Length <= 1)
        {
            if (lerpCurve.keys.Length == 1)
                lerpCurve.RemoveKey(0);
            lerpCurve.postWrapMode = WrapMode.Loop;
            lerpCurve.AddKey(new Keyframe(0, 0, 1, 1)); // 0, sin(0), sin'(0) = cos(0), sin'(0) = cos(0)
            lerpCurve.AddKey(new Keyframe(0.5f * Mathf.PI, 1, 0, 0));
            lerpCurve.AddKey(new Keyframe(Mathf.PI, 0, -1, -1));
            lerpCurve.AddKey(new Keyframe(1.5f * Mathf.PI, -1, 0, 0));
            lerpCurve.AddKey(new Keyframe(2f * Mathf.PI, 0, 1, 1));
        }
    }
}
