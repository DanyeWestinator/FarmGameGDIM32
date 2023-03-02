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


    void Start()
    {
        timeValue = dayLength;
    }

    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
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
        timeValue = dayLength;
    }
}
