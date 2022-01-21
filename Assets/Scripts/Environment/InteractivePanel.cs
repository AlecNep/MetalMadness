using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePanel : Interactive, ISaveable
{
    public Interactive[] interactables;
    [SerializeField]
    private string data;
    [SerializeField]
    private bool boolData;
    public enum State {Activated = 0, Standby = 1, Disabled = 2 }
    [SerializeField]
    protected State currentState = State.Standby;
    private GameObject[] screens;

    [Serializable]
    private struct SaveData
    {
        public int curState;
    }


    protected void Awake()
    {
        screens = new GameObject[3];
        foreach (State s in (State[])Enum.GetValues(typeof(State)))
        {
            screens[(int)s] = transform.Find(s.ToString() + " Screen").gameObject;
            print("screens[" + (int)s + "] = " + s.ToString());
        }
        SetScreen();
    }

    private void SetScreen()
    {
        for (int i=0; i<screens.Length; ++i)
        {
            if (i == (int)currentState)
                screens[i].SetActive(true);
            else
                screens[i].SetActive(false);
        }
    }

    public override void Interact()
    {
        if (data != "")
            _Interact(data);
        else
            _Interact();
    }

    public override void Interact(int input)
    {
        currentState = (State)input;
        SetScreen();
    }



    public void _Interact(string input)
    {
        foreach (Interactive i in interactables)
        {
            i.Interact(input);
        }
    }

    public void _Interact()
    {
        if (currentState == State.Standby)
            currentState = State.Activated;
        else if (currentState == State.Activated)
            currentState = State.Standby;
        SetScreen();

        foreach (Interactive i in interactables)
        {
            i.Interact();
        }
    }

    public object CaptureState()
    {
        return new SaveData
        {
            curState = (int)currentState
        };
    }

    public void LoadState(object data)
    {
        var saveData = (SaveData)data;
        currentState = (State)saveData.curState;
        SetScreen();
    }
}
