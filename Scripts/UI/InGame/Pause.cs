using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool isGamePaused = false ;
    [SerializeField] private GameObject SettingsMenu;

    private void Awake()
    {
        isGamePaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SceneLoader.Instance.inTransition)
        {
            if (isGamePaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        GamePause.PauseGame();
        SettingsMenu.gameObject.SetActive(true);
    }

    public void UnPauseGame()
    {
        isGamePaused = false;
        GamePause.ResumeGame();
        SettingsMenu.gameObject.SetActive(false);     
    }
}
