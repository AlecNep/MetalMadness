using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public PlayerControls player;
    public GameObject UI;
    public PauseMenu pauseMenu;

    public enum GameMode { Gameplay = 0, WeaponWheel = 1, Menu = 2, Map = 3 };
    private GameMode _currentGameMode = GameMode.Gameplay;
    public GameMode currentGameMode
    {
        //might be a bit redundant with the SetGameMode method
        get
        {
            return _currentGameMode;
        }
        private set
        {
            _currentGameMode = value;
            print("Changed game mode to " + _currentGameMode);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetGameMode(int mode)
    {
        currentGameMode = (GameMode)mode;
    }
}
