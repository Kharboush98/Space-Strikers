using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDWave : MonoBehaviour
{
    //Definitions

    public enum PreferredSpawnType
    {
        Random , SpawnPoints , Middle_SP , TopScreen_SP , BottomScreen_SP
    };

    [System.Serializable]
    public class EnemyWeight
    {
        public GameObject EnemiesPrefap;
        public PreferredSpawnType HowTospawn;
        public int Amount;
    };

    [System.Serializable]
    public class Phase
    {
        [HideInInspector] public string Name;
        public int NumOfEnemies = 0;
        [Space]
        public EnemyWeight[] Enemies;
        [Space]
        public float TimeBetweenEnemySpawn;
        //public float TimerUntillNextPhaseSpwan = 0f;
        public float CountdownToNextWave = 3f;
    };

    //private bool TimeForNextPhaseToSpawn = false;

    //Fields

    [Header("Wave Attributes")]
    [SerializeField] public OLDWaveHandler WHandler;
    public int CurrentWaveNum = 0;
    [Space]
    public int PhaseNum = 0;
    private int TotalNumOfPhases = 0;
    

    [Header("Phases Attributes")]
    public Phase[] Phases;

    private int RandomNum;
    private GameObject EnemyToSpawn;
    private float nextActionTime = 0.0f;
    private float period = 0.5f;
    private bool WaveDone = false;
    private Vector2 Spawnpos;

    //Functions

    void Awake()
    {
        NamePhases();
    }

    void Start()
    {
        CalculateNumOfEnemyInWave();
        TotalNumOfPhases = Phases.Length;
    }

    void Update()
    {
        if (!WaveDone)
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                CheckIfPhaseComplete();
            }
        }
    }

    void SpawnType(int PhaseNum , int EnemyNum)
    {
        int SpawnPoint;

        switch (Phases[PhaseNum].Enemies[EnemyNum].HowTospawn)
        {
            case PreferredSpawnType.Random:
                SetRandomSpawnPos();
                //Debug.Log("enemy spawned from Random pos" + Spawnpos);
                break;

            case PreferredSpawnType.SpawnPoints:
                SpawnPoint = Random.Range(0, WHandler.NormalSpawnPositions.Length);
                Spawnpos = WHandler.NormalSpawnPositions[SpawnPoint].transform.position;
                //Debug.Log("enemy spawned from Spawn points" + Spawnpos);
                break;

            case PreferredSpawnType.Middle_SP:
                Spawnpos = WHandler.NormalSpawnPositions[3].transform.position;
                //Debug.Log("enemy spawned from middle sp");
                break;

            case PreferredSpawnType.TopScreen_SP:
                SpawnPoint = Random.Range(0, WHandler.TopSpawnPositions.Length);
                Spawnpos = WHandler.TopSpawnPositions[SpawnPoint].transform.position;
                //Debug.Log("enemy spawned from top sp");
                break;

            case PreferredSpawnType.BottomScreen_SP:
                SpawnPoint = Random.Range(0, WHandler.BottomSpawnPositions.Length);
                Spawnpos = WHandler.BottomSpawnPositions[SpawnPoint].transform.position;
                //Debug.Log("enemy spawned from bottom sp");
                break;


        }
    }

    void SetRandomSpawnPos()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0.1f));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1.1f, 0.9f));
        Spawnpos = new Vector2(max.x, Random.Range(min.y, max.y));
    }

    private void SpawnEnemy()
    {
        RandomNum = Random.Range(0, Phases[PhaseNum].Enemies.Length);
        SpawnType(PhaseNum, RandomNum);

        if (Phases[PhaseNum].Enemies[RandomNum].Amount > 0)
        {
            EnemyToSpawn = Phases[PhaseNum].Enemies[RandomNum].EnemiesPrefap;
            Instantiate(EnemyToSpawn, Spawnpos, Quaternion.identity);
            Phases[PhaseNum].Enemies[RandomNum].Amount--;
            WHandler.NumOfEnemySpawned++;
        }
        else
        {
            SpawnEnemy();
        }

    }

    //private void SpawnEnemy()
    //{
    //    Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0.1f));
    //    Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1.1f, 0.9f));
    //    Vector2 SpawnPos = new Vector2(max.x, Random.Range(min.y, max.y));

    //    RandomNum = Random.Range(0, Phases[PhaseNum].Enemies.Length);

    //    if (Phases[PhaseNum].Enemies[RandomNum].Amount > 0)
    //    {
    //        EnemyToSpawn = Phases[PhaseNum].Enemies[RandomNum].EnemiesPrefap;
    //        Instantiate(EnemyToSpawn, SpawnPos, Quaternion.identity);
    //        Phases[PhaseNum].Enemies[RandomNum].Amount--;
    //        WHandler.NumOfEnemySpawned++;
    //    }
    //    else
    //    {
    //        SpawnEnemy();
    //    }
    //}

    public IEnumerator SpawnWave()
    {
        //WHandler.UpdateWaveState(WaveHandler.WaveState.Spawning);

        if (WHandler.NumOfEnemySpawned != Phases[PhaseNum].NumOfEnemies)
        {
            for (int i = 0; i < Phases[PhaseNum].NumOfEnemies; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(Phases[PhaseNum].TimeBetweenEnemySpawn);
            }
        }
        yield break;
    }

    public void CheckIfWaveComplete()
    {
        if (PhaseNum >= TotalNumOfPhases - 1)
        {
            //Debug.Log("why are we here just to suffer");
            WaveDone = true;
            WHandler.UpdateWaveState(OLDWaveHandler.WaveState.AllPhasesInWaveDone);
        }
        else
        {
            WHandler.ResetCounters();
            PhaseNum++;
            WHandler.UpdateWaveState(OLDWaveHandler.WaveState.Waiting);
        }
    }


    //------------------------------------>here
    void CheckIfPhaseComplete()
    {
        //All Enemies of a phase Spawned and got destroyed
        //int TempPhaseNum = PhaseNum;

        if (WHandler.NumOfEnemySpawned == Phases[PhaseNum].NumOfEnemies
            && WHandler.NumOfEnemySpawned == WHandler.NumOfEnemyDestroyed)
        {
            WHandler.UpdateWaveState(OLDWaveHandler.WaveState.PhaseComplete);
            CheckIfWaveComplete();
        }
    }

    void CalculateNumOfEnemyInWave()
    {
        for (int i = 0; i < Phases.Length; i++)
        {
            for (int j = 0; j < Phases[i].Enemies.Length; j++)
            {
                Phases[i].NumOfEnemies += Phases[i].Enemies[j].Amount;
                //SetMinTimerUntilNextPhase(i);
            }
        }
    }

    void NamePhases()
    {
        for (int i = 0; i < Phases.Length; i++)
        {
            int N = i + 1;
            Phases[i].Name = "Phase " + N ;
        }
    }

}