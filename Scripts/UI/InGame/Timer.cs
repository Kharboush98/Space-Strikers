using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI Timertext;
    private float TimeToCountdown;
    private int WaveNum;
    void Start()
    {
        Timertext = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        TimeToCountdown -= 1 * Time.deltaTime;
        //Timertext.text = "Next Wave in " + TimeToCountdown.ToString("0");
        Timertext.text = "Wave " + "(" + WaveNum + ")" + " in " + TimeToCountdown.ToString("0");
        
        if (TimeToCountdown <= 0)
        {
            TimeToCountdown = 0;
            gameObject.SetActive(false);
        }
    }

    public void SetTimer(float WaveTime)
    {
        gameObject.SetActive(true);
        TimeToCountdown = WaveTime;
    }

    public void SetWaveNumText(int num)
    {
        WaveNum = num;
    }

}
