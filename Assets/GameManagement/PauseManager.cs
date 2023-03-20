using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    


    public static PauseManager instance = null;
    public GameObject panel;

    #region UnityEvents    
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // panel = transform.GetChild(0).gameObject;
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

    // hides the pause menu without unpausing the game
    // will lock pause state until showpause is called
    public void HidePause()
    {
        panel.SetActive(false);
        GameStateManager.lockState = true;
    }

    // unlocks pause state and shows panel
    public void ShowPause()
    {
        panel.SetActive(true);
        GameStateManager.lockState = false;
    }

    public void MainMenu()
    {
        //Load the main menu scene
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        GameStateManager.TogglePause();
    }
    
    #endregion
}
