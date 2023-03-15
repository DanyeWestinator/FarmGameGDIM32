using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    


    public static PauseManager instance = null;
    private GameObject panel;

    #region UnityEvents    
    void Awake()
    {
        instance = this;
        panel = transform.GetChild(0).gameObject;
        GameStateManager.StartPause.AddListener(OpenPause);
        GameStateManager.EndPause.AddListener(ClosePause);
        
    }

    private void OnDestroy()
    {
        GameStateManager.StartPause.RemoveListener(OpenPause);
        GameStateManager.EndPause.RemoveListener(ClosePause);
    }
    #endregion

    #region PauseFunctions

    public void OpenPause()
    {
        print("Opening pause panel");
        panel.SetActive(true);
    }
    public void ClosePause()
    {
        print("Close pause panel");
        panel.SetActive(false);
    }
    
    #endregion
}
