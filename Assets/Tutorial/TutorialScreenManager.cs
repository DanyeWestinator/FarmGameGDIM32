using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// manages the tutorial slideshow
public class TutorialScreenManager : MonoBehaviour
{

    public GameObject screenHolder; // enable/disable
    public GameObject[] screens;
    private int screenIndex = 0;
    private bool screenActive = false;
    private GameObject currentScreen = null;

    private static TutorialScreenManager instance;
    public static bool IsOpen => instance.screenHolder.activeInHierarchy;
    private void Awake()
    {
        instance = this;
    }

    public void toggle()
    {
        
        screenActive = !screenActive;
        screenHolder.SetActive(screenActive);
        PlayerController.ToggleCanMove();
        if (currentScreen == null) switchTo(screenIndex);
    }

    public void nextScreen()
    {
        screenIndex++;
        if (screenIndex == screens.Length) screenIndex = 0;
        switchTo(screenIndex);
    }

    public void previousScreen()
    {
        screenIndex--;
        if (screenIndex < 0) screenIndex = screens.Length - 1;
        switchTo(screenIndex);
    }

    private void switchTo(int index)
    {
        Destroy(currentScreen);
        currentScreen = Instantiate(screens[index], screenHolder.transform);
    }
    
}
