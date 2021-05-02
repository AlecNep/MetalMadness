using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    PlayerControls player;
    Slider health, shift, dash;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: all of this should be more secure
        health = transform.Find("Health").GetComponent<Slider>();
        shift = transform.Find("Shift").GetComponent<Slider>();
        dash = transform.Find("Dash").GetComponent<Slider>();
        GameObject lPlayer = GameObject.Find("Player");
        if (lPlayer == null)
        {
            print("HUD somehow didn't find the player. Panic");
        }
        player = lPlayer.GetComponent<PlayerControls>();
        health.maxValue = player.GetMaxHealth();
        shift.maxValue = player.GetShiftDelay();
        dash.maxValue = player.GetDashDelay();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //TODO: MAJORLY temporary code here! Change ASAP!
        UpdateSliders();
    }

    private void UpdateSliders()
    {
        health.value = player.GetHealth();
        shift.value = player.GetShiftTimer();
        dash.value = player.GetDashTimer();
    }
}
