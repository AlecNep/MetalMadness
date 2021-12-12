using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    private int previousGameMode;

    public GameObject pausedFirstButton, controlsClosedButton;

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
            previousGameMode = (int)GameManager.currentGameMode;
            GameManager.SetGameMode(2);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pausedFirstButton);
        }
        else
        {
            /*gameObject.SetActive(false);
            AudioListener.pause = false;
            Time.timeScale = 1f;
            GameManager.SetGameMode(previousGameMode);*/
            StartCoroutine(_UnPauseGame());
        }
    }

    private IEnumerator _UnPauseGame()
    {
        int i = 0;
        while (i < 1)
        {
            yield return null;
            ++i;
        }
        gameObject.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1f;
        GameManager.SetGameMode(previousGameMode);
    }

    public void OpenControls()
    {
        //TODO
    }

    public void CloseControls()
    {
        //TODO
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsClosedButton);
    }
}
