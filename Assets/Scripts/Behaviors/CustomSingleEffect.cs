using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSingleEffect : MonoBehaviour
{
    [SerializeField]
    private float lifetime;
    private float curDur;
    private AudioSource impactSound;


    // Update is called once per frame
    void Update()
    {
        curDur += Time.deltaTime;

        if (curDur > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
