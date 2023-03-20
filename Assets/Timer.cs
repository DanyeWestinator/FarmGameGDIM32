using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Class by Eric
public class Timer : MonoBehaviour
{
    public GameObject DayOver;
    public ScoreKeeper ScoreKeeper;
    public TextMeshProUGUI DayScore;
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI TotalScore;
    public float dayLength = 90;
    public float timeValue = 0;
    public Text timeText;

    private bool _paused = false;
    public void enablePause(){_paused = true; print("Enabling pause from timer");}
    public void disablePause(){_paused = false;print("Disabling pause from timer");}
    void Start()
    {
        timeValue = dayLength;
        GameStateManager.StartPause.AddListener(enablePause);
        GameStateManager.EndPause.AddListener(disablePause);
    }

    private void OnDestroy()
    {
        GameStateManager.StartPause.RemoveListener(enablePause);
        GameStateManager.EndPause.RemoveListener(disablePause);
    }

    void Update()
    {
        //Don't decrease if paused
        if (timeValue > 0 && _paused == false)
        {
            timeValue -= Time.deltaTime;
        }
        else if (timeValue <= 0)
        {
            timeValue = 0;
            
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            // pause game and show menu
            GameStateManager.SetPause();
            timeToDisplay = 0;
            int dayScore = ScoreKeeper.NewDay();
            DayOver.SetActive(true);

            DayScore.text = $"Day Score:{dayScore}";
            HighScore.text = $"High Score:{ScoreKeeper.highScore}";
            TotalScore.text = $"Total:{ScoreKeeper.currentScore}";
            
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        print("resetting day");
        GameStateManager.SetPlay();
        timeValue = dayLength;
    }
}
