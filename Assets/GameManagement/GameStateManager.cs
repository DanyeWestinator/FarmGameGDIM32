using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum States
{
    Play,
    Pause,
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public static States _state => instance.currentState;
    public States currentState = States.Play;

    public static UnityEvent StartPlay = new UnityEvent();
    public static UnityEvent EndPlay = new UnityEvent();
    public static UnityEvent StartPause = new UnityEvent();
    public static UnityEvent EndPause = new UnityEvent();

    private void Awake()
    {
        instance = this;
    }

    public static void SetState(States state)
    {
        print($"old: {_state}, new: {state}");
        //If we're changing gameplay state
        if (_state != state)
        {
            //Close old state
            if (_state == States.Play)
                EndPlay.Invoke();
            else if (_state == States.Pause)
                EndPause.Invoke();
            
            
            //Starting new states
            if (state == States.Play)
                StartPlay.Invoke();
            else if (state == States.Pause)
                StartPause.Invoke();
            instance.currentState = state;
        }
    }

    public static void TogglePause()
    {
        print($"Toggling pause from {_state}");
        if (_state == States.Pause)
            SetState(States.Play);
        else if (_state == States.Play)
            SetState(States.Pause);
    }
}
