using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public PlayerControls player;
    public GameObject UI;
    public PauseMenu pauseMenu { get; private set; }
    public WeaponSelector weaponWheel { get; private set; }

    public enum GameMode { Gameplay = 0, WeaponWheel = 1, Menu = 2, Map = 3 };
    private static GameMode _currentGameMode = GameMode.Gameplay;
    public static GameMode currentGameMode //consider making this static
    {
        //might be a bit redundant with the SetGameMode method
        get
        {
            return _currentGameMode;
        }
        private set
        {
            _currentGameMode = value;
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

        pauseMenu = UI.transform.Find("PauseMenu").GetComponent<PauseMenu>();
        if (pauseMenu == null)
            Debug.LogError("Unable to find PauseMenu inside the UI");
        weaponWheel = UI.transform.Find("Weapon Wheel").GetComponent<WeaponSelector>();
        if (weaponWheel == null)
            Debug.LogError("Unable to find Weapon Wheel inside the UI");
    }

    public static void SetGameMode(int mode)
    {
        currentGameMode = (GameMode)mode;
    }
}
