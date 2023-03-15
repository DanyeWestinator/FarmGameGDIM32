using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string GameSceneName;
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
}
