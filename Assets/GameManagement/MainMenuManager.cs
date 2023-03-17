using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string GameSceneName;
    [SerializeField] private GameObject creditsPanel;
    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }
    
    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    
    #else
        Application.Quit();
    #endif
    }
    /// <summary>
    /// Flips if the credits is on or off
    /// </summary>
    public void ToggleCredits()
    {
        creditsPanel.SetActive(!creditsPanel.activeInHierarchy);
    }
}
