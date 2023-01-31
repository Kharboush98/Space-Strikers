using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OLDWaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] WavesArray;
    [Space]
    [SerializeField] private OLDWave CurrentWave;
    public int ActiveWaveID = 0;

    private bool QNextWave = false;

    public enum WaveState
    {
        Waiting, Spawning, PhaseComplete, AllPhasesInWaveDone ,AllWavesDone
    };
    public WaveState State;
    public static event Action<WaveState> OnStateChange;

    //Variables
    [Header("General")]
    public int NumOfEnemySpawned;
    public int NumOfEnemyDestroyed;

    [Header("SpawnPos")]
    public Transform[] NormalSpawnPositions;
    public Transform[] TopSpawnPositions;
    public Transform[] BottomSpawnPositions;

    //UI

    private void Awake()
    {
        OnStateChange += WaveManagerOnstateChange;
    }

    private void OnDisable()
    {
        OnStateChange -= WaveManagerOnstateChange;
    }

    void Start()
    {
        //StartCoroutine(Wait(0.1f));
        SetCurrentWave(ActiveWaveID);
        UpdateWaveState(WaveState.Waiting);
    }


    public void UpdateWaveState(WaveState WS)
    {
        State = WS;

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
        // when the states changes to one of these corresponding code will run
        switch (wave)
        {
                //waiting/countdown to next wave
            case WaveState.Waiting:
                QNextWave = false;
                StartCoroutine(WaitThenStartSpawning());
                Debug.Log("Waiting");
                break;

                //is spawning current wave
            case WaveState.Spawning:
                int WN = CurrentWave.CurrentWaveNum + 1;
                Debug.Log("Spawning wave " + WN);
                CurrentWave.StartCoroutine(CurrentWave.SpawnWave());

                break;

                //when num of enemy spawned = num of enemies in wave AND num of enemies destroyed
            case WaveState.PhaseComplete:
                int Phase = CurrentWave.PhaseNum + 1;
                Debug.Log("Phase " + Phase + " Finished");
                //CurrentWave.PhaseNum++;
                break;

                //All Phases of a waves Completed Level is done
            case WaveState.AllPhasesInWaveDone:

                Debug.Log("All phases of Wave " + CurrentWave.CurrentWaveNum + " are Finished");
                QNextWave = true;
                QueueNextWave();
                break;

                //All Waves Finished , Level Done !
            case WaveState.AllWavesDone:

                Debug.Log("LevelDone");
                break;
        }
    }

    public IEnumerator WaitThenStartSpawning()
    {
        //wait for Seconds , show Wave number/name then start Spawning Wave
        //Call Spawnwave() From here
        
        Debug.Log("Starting to Spawn enemies after countdown");
        yield return new WaitForSeconds(CurrentWave.Phases[CurrentWave.PhaseNum].CountdownToNextWave);
        //CurrentWave.StartCoroutine(CurrentWave.SpawnWave());
        UpdateWaveState(WaveState.Spawning);

        yield break;
    }

    public void SetCurrentWave(int WaveID)
    {
        CurrentWave = WavesArray[WaveID].gameObject.transform.GetComponent<OLDWave>();
    }

    public void QueueNextWave()
    {
        Debug.Log("QueueNextWave Fn");
        if(CurrentWave.CurrentWaveNum < WavesArray.Length - 1 && QNextWave)
        {
            ActiveWaveID++;
            SetCurrentWave(ActiveWaveID);
            ResetCounters();
            UpdateWaveState(OLDWaveHandler.WaveState.Waiting);
        }
        else
        {
            UpdateWaveState(OLDWaveHandler.WaveState.AllWavesDone);
        }
    }

    public void ResetCounters()
    {
        NumOfEnemySpawned = 0;
        NumOfEnemyDestroyed = 0;
    }
}
