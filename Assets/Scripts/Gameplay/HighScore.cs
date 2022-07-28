using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public Text highScoreText;
    
    void Start()
    {   
        if(highScoreText != null)
        { 
            highScoreText.text = getHighScore().ToString();
        }
    }

    public void setHighScore(int score)
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            if(getHighScore() < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public int getHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
}
