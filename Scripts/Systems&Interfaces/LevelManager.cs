using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{    
    public string LevelName;
    
    private GameObject _Player;
    private GameObject _WHandler;
    private int CurrentScore;
    [SerializeField] private int CurrentLevelNum;
    private bool runOnce = false;

    [Space]
    [SerializeField] private bool LastLevel = false;

    [Space]
    [Header("EndScreens")]
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private GameObject SettingsScreenManager;

    void Start()
    {
        //CurrentLevelNum = PlayerPrefs.GetInt("LevelReached");
        _Player = GameObject.FindGameObjectWithTag("Player");
        _WHandler = GameObject.FindGameObjectWithTag("WaveHandler");

    }

    void Update()
    {
        EndScreen();
    }

    void EndScreen()
    {
        if (_Player.GetComponent<PlayerAttributes>().isPlayerAlive() == true &&
            _WHandler.GetComponent<WaveHandler>().isLevelDone() == true
            && !runOnce)
        {
            // Won game
            Pause.isGamePaused = true;
            GamePause.PauseGame();
            if(SettingsScreenManager != null) SettingsScreenManager.gameObject.SetActive(false);
            runOnce = true;
            CurrentLevelNum += 1;
            CheckForNewHighScore();
            if(!LastLevel) PlayerPrefs.SetInt("LevelReached", CurrentLevelNum);
            if (WinScreen != null) WinScreen.gameObject.SetActive(true);
            //Debug.Log("Level manager : Won");
        }
        else if (_Player.GetComponent<PlayerAttributes>().isPlayerAlive() == false &&
            _WHandler.GetComponent<WaveHandler>().isLevelDone() == false
            && !runOnce)
        {
            // Lost / Died
            Pause.isGamePaused = true;
            GamePause.PauseGame();
            Debug.Log(Time.timeScale);
            if (SettingsScreenManager != null) SettingsScreenManager.gameObject.SetActive(false);
            runOnce = true;
            CheckForNewHighScore();
            if (LoseScreen != null) LoseScreen.gameObject.SetActive(true);
            Debug.Log("Level manager : Lost");
            Debug.Log(Time.timeScale);
        }
    }

    void CheckForNewHighScore()
    {
        CurrentScore = _WHandler.GetComponent<WaveHandler>().GetScore();
        HighScore.TryNewScore(CurrentScore, LevelName);
    }

}
