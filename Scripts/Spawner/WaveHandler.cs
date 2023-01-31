using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] WavesArray;
    [Space]
    [SerializeField] private Wave CurrentWave;
    public int ActiveWaveID = 0;

    private bool QNextWave = false;

    private int Score = 0;
    private bool LevelDone = false;

    public enum WaveState
    {
        Waiting, Spawning, PhaseComplete, AllPhasesInWaveDone, AllWavesDone
    };
    public WaveState State;
    public static event Action<WaveState> OnStateChange;

    //Variables
    [Header("General")]
    public int NumOfEnemySpawned;
    public int NumOfEnemyDestroyed;

    //[Header("SpawnPos")]
    [HideInInspector] public GameObject GridPoints;
    //public Transform[] NormalSpawnPositions;
    //public Transform[] TopSpawnPositions;
    //public Transform[] BottomSpawnPositions;

    [Header("Handlers")]
    [SerializeField] private GameObject Timer_Go;

    void Awake()
    {
        OnStateChange += WaveManagerOnstateChange;
    }

    private void OnDisable()
    {
        OnStateChange -= WaveManagerOnstateChange;
    }

    void Start()
    {
        GridPoints = GameObject.FindGameObjectWithTag("GridPoints");
        //Timer = GameObject.FindGameObjectWithTag("Timer");
        SetCurrentWave(ActiveWaveID);
        UpdateWaveState(WaveState.Waiting);
    }

    public void UpdateWaveState(WaveState WS)
    {
        switch (WS)
        {
            case WaveState.Waiting:
                break;
            case WaveState.Spawning:
                break;
            case WaveState.PhaseComplete:
                break;
            case WaveState.AllPhasesInWaveDone:
                break;
            case WaveState.AllWavesDone:
                break;
        }
        OnStateChange?.Invoke(WS);
    }

    void WaveManagerOnstateChange(WaveState wave)
    {
        int WN = CurrentWave.CurrentWaveNum + 1;
        int PN = CurrentWave.PhaseNum + 1;
        if (!Pause.isGamePaused)
        {
            switch (wave)
            {
                case WaveState.Waiting:
                    QNextWave = false;
                    StartCoroutine(WaitThenStartSpawning());
                    break;

                case WaveState.Spawning:
                    int x = CurrentWave.GetNumOfEnemiesInPhase();
                    Debug.Log("Spawning wave " + WN + " Phase: " + PN + " Enemies= " + x);
                    CurrentWave.StartCoroutine(CurrentWave.StartSpawning());
                    break;

                case WaveState.PhaseComplete:
                    int Phase = CurrentWave.PhaseNum + 1;
                    Debug.Log("Phase " + Phase + " Finished");
                    ResetNumOfEnemySpawned();
                    //StopCoroutine(WaitThenStartSpawning());
                    //StopCoroutine(CurrentWave.StartSpawning());
                    CurrentWave.PhaseNum++;
                    UpdateWaveState(WaveState.Waiting);
                    break;

                case WaveState.AllPhasesInWaveDone:
                    Debug.Log("All phases of Wave " + WN + " are Finished");
                    CurrentWave.gameObject.SetActive(false);
                    QNextWave = true;
                    QueueNextWave();
                    break;

                case WaveState.AllWavesDone:
                    Debug.Log("LevelDone");
                    LevelDone = true;
                    break;
            }
        }
    }

    public IEnumerator WaitThenStartSpawning()
    {

        Debug.Log("Starting to Spawn enemies after countdown");
        DisplayTimerText();
        yield return new WaitForSeconds(CurrentWave.Phases[CurrentWave.PhaseNum].CountdownToNextWave);
        UpdateWaveState(WaveState.Spawning);
    }

    public void SetCurrentWave(int WaveID)
    {
        CurrentWave = WavesArray[WaveID].gameObject.transform.GetComponent<Wave>();
        CurrentWave.gameObject.SetActive(true);
    }

    public void QueueNextWave()
    {
        Debug.Log("QueueNextWave Fn");
        if (CurrentWave.CurrentWaveNum < WavesArray.Length - 1 && QNextWave)
        {
            //ResetCounters();
            ActiveWaveID++;
            SetCurrentWave(ActiveWaveID);
            ResetNumOfEnemySpawned();
            ResetNumOfEnemyDestroyed();
            UpdateWaveState(WaveHandler.WaveState.Waiting);
        }
        else
        {
            UpdateWaveState(WaveHandler.WaveState.AllWavesDone);
        }
    }
    public void ResetNumOfEnemySpawned()
    {
        NumOfEnemySpawned = 0;
    }

    public void ResetNumOfEnemyDestroyed()
    {
        NumOfEnemyDestroyed = 0;
    }

    public bool isLevelDone()
    {
        return LevelDone;
    }

    //-------------- Timer Handlers ------------//
    private void DisplayTimerText()
    {
        if (CurrentWave.PhaseNum == 0)
        {
            Timer_Go.transform.GetComponent<Timer>().SetWaveNumText(CurrentWave.CurrentWaveNum + 1);
            Timer_Go.transform.GetComponent<Timer>().SetTimer(CurrentWave.Phases[CurrentWave.PhaseNum].CountdownToNextWave);
        }
    }

    //-------------- Score Handlers ------------//
    public void AddScore(int Scorepoints)
    {
        Score += Scorepoints;
    }

    public int GetScore()
    {
        return Score;
    }

}
