using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHighNewScore : MonoBehaviour
{
    private TextMeshProUGUI HighScoreText;
    [SerializeField] private string LevelName;
    //private GameObject Levelmanager;

    [SerializeField] private bool MainMenu;

    void Start()
    {
        HighScoreText = GetComponent<TextMeshProUGUI>();

        //Levelmanager = GameObject.FindGameObjectWithTag("LevelManager");
        //int Highscore = PlayerPrefs.GetInt(Levelmanager.GetComponent<LevelManager>().LevelName);
        //int Highscore = PlayerPrefs.GetInt(LevelName);
        //HighScoreText.text = "High Score: " + Highscore;

        ShowScore();
    }

    void ShowScore()
    {
        int HS = PlayerPrefs.GetInt(LevelName);
        if (HS == 0)
        {
            HighScoreText.text = "---------";
        }
        else
        {
            if (MainMenu)
            {
                HighScoreText.text = HS.ToString();
            }
            else
            {
                HighScoreText.text = "High Score : " + HS;
            }
        }
    }

}
