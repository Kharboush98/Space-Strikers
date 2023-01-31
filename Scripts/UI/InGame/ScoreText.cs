using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private GameObject _WHandler;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        _WHandler = GameObject.FindGameObjectWithTag("WaveHandler");
    }

    // Update is called once per frame
    void Update()
    {
        if(_WHandler != null)
        score = _WHandler.GetComponent<WaveHandler>().GetScore();

        scoreText.text = "Score : " + score;
    }
}
