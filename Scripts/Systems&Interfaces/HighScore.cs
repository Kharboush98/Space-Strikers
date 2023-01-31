using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighScore
{
    public static int GetHighScore(string StageName)
    {
        return PlayerPrefs.GetInt(StageName);
    }

    public static bool TryNewScore(int newScore , string Levelnum)
    {
        int CurrentScore = GetHighScore(Levelnum);
        if(newScore > CurrentScore)
        {
            //new High Score
            PlayerPrefs.SetInt(Levelnum, newScore);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    // for testing :-
    public static void ResetHighScore(string StageName)
    {
        PlayerPrefs.SetInt(StageName , 0);
        PlayerPrefs.Save();
    }
}
