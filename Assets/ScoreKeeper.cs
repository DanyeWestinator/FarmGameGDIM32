using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Daichi -- score container and medium between UI, dayscore and player 
public class ScoreKeeper : MonoBehaviour
{

    public TextMeshProUGUI scoreCounter;

    public int currentScore = 0;

    public int dailyScore = 0;

    public int highScore = 0;

    // returns daily score
    public int NewDay()
    {
        if (dailyScore > highScore)
        {
            highScore = dailyScore;
        }

        int dayScore = dailyScore;
        
        dailyScore = 0;

        return dayScore;
    }


    
    public void AddScore(int toAdd)
    {
        currentScore += toAdd;
        dailyScore += toAdd;
        scoreCounter.text = $"Current score:\n{currentScore}";

        // print("scorekeeper: score added");
        
    }
}
