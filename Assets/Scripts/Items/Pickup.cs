using UnityEngine;
using System;

public abstract class Pickup : MonoBehaviour, ISaveable
{
    protected Action effect;
    [SerializeField]
    protected bool respawnable;
    [SerializeField]
    protected float respawnRate;
    protected bool grabbed;
    protected float timer;

    [Serializable]
    private struct SaveData
    {
        public bool respawnable;
        public float respawnRate;
        public bool grabbed;
    }

    protected void OnValidate()
    {
        if (respawnable)
        {
            timer = respawnRate;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            grabbed = true;
            if (respawnable)
            {
                timer = 0;
            }
            effect();
            gameObject.SetActive(false);
        }
    }

    protected void Update()
    {
        if (CanRespawn())
        {
            grabbed = false;
            timer = respawnRate;
            gameObject.SetActive(true);
        }
    }

    protected bool CanRespawn()
    {
        return respawnable && (timer >= respawnRate) && grabbed;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            respawnable = respawnable,
            respawnRate = respawnRate,
            grabbed = grabbed
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        respawnable = saveData.respawnable;
        respawnRate = saveData.respawnRate;
        grabbed = saveData.grabbed;
        if (grabbed)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
