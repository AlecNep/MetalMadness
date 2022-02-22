using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackoutCover : Interactive, ISaveable
{
    [Range(0,1)]
    public float fadeRate;
    private MeshRenderer mesh;
    [SerializeField]
    private bool isVisible;
    private bool alreadyActivated = false;

    [Serializable]
    private struct SaveData
    {
        public bool visible;
        public bool activated;
    }

    private void OnValidate()
    {
        if (mesh == null)
            mesh = GetComponent<MeshRenderer>();
        Color col;
        if (isVisible)
        {
            col = Color.black;
        }
        else
        {
            col = Color.clear;
        }
            
        mesh.sharedMaterial.color = col;
    }

    private void Start()
    {
        if (mesh == null)
            mesh = GetComponent<MeshRenderer>();
    }

    public override void Interact()
    {
        if (!alreadyActivated)
        {
            alreadyActivated = true;
            if (isVisible)
            {
                StartCoroutine(FadeOut());
            }
            else
            {
                StartCoroutine(FadeIn());
            }
        }
    }


    private IEnumerator FadeIn()
    {
        print("Fading in");
        while (mesh.material.color.a < 1)
        {
            yield return null;
            mesh.material.color += Color.black * fadeRate * Time.deltaTime;
            if (mesh.material.color.a >= 1) //not sure if this value can even be higher than 1, but I'm adding it just in case
            {
                mesh.material.color = Color.black;
                isVisible = true;
                break;
            }
        }
    }

    private IEnumerator FadeOut()
    {
        print("Fading out");
        while (mesh.material.color.a > 0)
        {
            yield return null;
            mesh.material.color -= Color.black * fadeRate * Time.deltaTime;
            if (mesh.material.color.a <= 0) //not sure if this value can even be lower than 0, but I'm adding it just in case
            {
                mesh.material.color = Color.clear;
                isVisible = false;
                break;
            }
        }
    }

    public object CaptureState()
    {
        return new SaveData
        {
            visible = isVisible,
            activated = alreadyActivated
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        isVisible = saveData.visible;
        alreadyActivated = saveData.activated;
    }
}
