using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            gameObject.SetActive(true);
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            AudioListener.pause = false;
        }
    }
}
