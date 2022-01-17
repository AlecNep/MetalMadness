using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ISaveable
{
    [Serializable]
    private struct SaveData
    {
        public float health;
        public float xPos;
        public float yPos;
        public float zPos;
    }

    public object CaptureState()
    {
        SaveData data = new SaveData();
        if (TryGetComponent(out Damageable d)){
            data.health = d.GetHealth();
        }
        else
        {
            data.health = 50; //Should never happen
            Debug.LogError("Enemy.CaptureState: " + name + " was not able to detect Damageable component");
        }

        data.xPos = transform.position.x;
        data.yPos = transform.position.y;
        data.zPos = transform.position.z;
        return data;
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        if (TryGetComponent(out Damageable d))
        {
            if (d.GetHealth() <= 0)
                gameObject.SetActive(false);
            else
                d.SetHealth(saveData.health);
        }
        else
            Debug.LogError("Enemy.LoadState: " + name + " was not able to detect Damageable component");
        transform.position = new Vector3(saveData.xPos, saveData.yPos, saveData.zPos);
    }
}
