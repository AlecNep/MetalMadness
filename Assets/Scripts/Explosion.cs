using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private float mDuration;
    private float mMaxDuration = 1;
    [SerializeField]
    protected float camShakeMagnitude;
    [SerializeField]
    protected float camShakeTime;
    private AudioSource explosionSound;

    // Use this for initialization
    void Awake () {
        explosionSound = GetComponent<AudioSource>();
        GameManager.Instance.MainCamera.ScreenShake(camShakeMagnitude, camShakeTime);
        //explosionSound.Play();
    }

	
	// Update is called once per frame
	void Update () {
        if (mDuration >= mMaxDuration)
            Destroy(gameObject);
        else
            mDuration += Time.deltaTime;
	}
}
