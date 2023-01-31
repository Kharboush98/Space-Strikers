using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePause
{

    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
    }

}
