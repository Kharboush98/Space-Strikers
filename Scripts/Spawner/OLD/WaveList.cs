using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveList : MonoBehaviour
{
    public enum PreferredSpawnType
    {
        Random, SpawnPoints, Middle_SP, TopScreen_SP, BottomScreen_SP
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
        public float CountdownToNextWave = 3f;
        public bool lastPhaseinWave = false;
    };

    [Header("Wave Attributes")]
    [SerializeField] public WaveHandler WHandler;
    public int CurrentWaveNum = 0;
    [Space]
    public int PhaseNum = 0;

    [Space]
    [Header("Phases Attributes")]
    public List<GameObject> EnemyList = new List<GameObject>();
    public Phase[] Phases;
    [Space]
    [SerializeField] private int TotalNumOFEnemies = 0;
    private int RandomNum;
    private GameObject EnemyToSpawn;
    private float nextActionTime = 0.0f;
    private float period = 0.5f;
    private bool WaveDone = false;
    private Vector2 Spawnpos;


    private void Awake()
    {
        AddEnemyToList();
    }

    void Start()
    {   
        NamePhases();
        CalculateNumOfEnemyInWave();
    }

    // Update is called once per frame
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
    //void SpawnType(PreferredSpawnType ST)
    //{
    //    int SpawnPoint;

    //    switch (ST)
    //    {
    //        case PreferredSpawnType.Random:
    //            SetRandomSpawnPos();
    //            //Debug.Log("enemy spawned from Random pos" + Spawnpos);
    //            break;

    //        case PreferredSpawnType.SpawnPoints:
    //            SpawnPoint = Random.Range(0, WHandler.NormalSpawnPositions.Length);
    //            Spawnpos = WHandler.NormalSpawnPositions[SpawnPoint].transform.position;
    //            //Debug.Log("enemy spawned from Spawn points" + Spawnpos);
    //            break;

    //        case PreferredSpawnType.Middle_SP:
    //            Spawnpos = WHandler.NormalSpawnPositions[3].transform.position;
    //            //Debug.Log("enemy spawned from middle sp");
    //            break;

    //        case PreferredSpawnType.TopScreen_SP:
    //            SpawnPoint = Random.Range(0, WHandler.TopSpawnPositions.Length);
    //            Spawnpos = WHandler.TopSpawnPositions[SpawnPoint].transform.position;
    //            //Debug.Log("enemy spawned from top sp");
    //            break;

    //        case PreferredSpawnType.BottomScreen_SP:
    //            SpawnPoint = Random.Range(0, WHandler.BottomSpawnPositions.Length);
    //            Spawnpos = WHandler.BottomSpawnPositions[SpawnPoint].transform.position;
    //            //Debug.Log("enemy spawned from bottom sp");
    //            break;


    //    }
    //}

    void AddEnemyToList()
    {
        List<GameObject> EnemyGenerated = new List<GameObject>();
        Debug.Log("add enemy to list fn");

        for (int XEnemiesAdded = 0; XEnemiesAdded != Phases[PhaseNum].NumOfEnemies; XEnemiesAdded++)
        {
            Debug.Log("inside While loop");
            RandomNum = Random.Range(0, Phases[PhaseNum].Enemies.Length);

            if (Phases[PhaseNum].Enemies[RandomNum].Amount > 0)
            {
                Debug.Log("inside if statement");

                EnemyToSpawn = Phases[PhaseNum].Enemies[RandomNum].EnemiesPrefap;
                EnemyGenerated.Add(EnemyToSpawn);
                Phases[PhaseNum].Enemies[RandomNum].Amount--;
            }
            else
            {
                XEnemiesAdded--;
            }
        }

        EnemyList.Clear();
        EnemyList = EnemyGenerated;
    }

    private void SpawnEnemy2()
    {
        if (EnemyList.Count > 0)
        {
            WHandler.NumOfEnemySpawned++;
            //SpawnType(EnemyList[0].GetComponent<EnemyBase>().HowTospawn);
            Instantiate(EnemyList[0], Spawnpos, Quaternion.identity);
            EnemyList.RemoveAt(0);
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
        //SpawnType(PhaseNum, RandomNum);

        if (Phases[PhaseNum].Enemies[RandomNum].Amount > 0)
        {
            EnemyToSpawn = Phases[PhaseNum].Enemies[RandomNum].EnemiesPrefap;
            Phases[PhaseNum].Enemies[RandomNum].Amount--;
            WHandler.NumOfEnemySpawned++;
            Instantiate(EnemyToSpawn, Spawnpos, Quaternion.identity);
        }
        else
        {
            SpawnEnemy();
        }

    }

    public IEnumerator StartSpawning()
    {
        if (WHandler.NumOfEnemySpawned != Phases[PhaseNum].NumOfEnemies)
        {
            for (int i = 0; i < Phases[PhaseNum].NumOfEnemies; i++)
            {
                //SpawnEnemy();
                SpawnEnemy2();
                yield return new WaitForSeconds(Phases[PhaseNum].TimeBetweenEnemySpawn);
            }
        }
    }

    void CheckIfPhaseComplete()
    {
        int x = GetNumOfEnemiesInPhase();

        if (WHandler.NumOfEnemySpawned == Phases[PhaseNum].NumOfEnemies &&
            /*WHandler.NumOfEnemyDestroyed == Phases[PhaseNum].NumOfEnemies
            && */ !Phases[PhaseNum].lastPhaseinWave)
        {
            Debug.Log("1st Cond , spawned = " + WHandler.NumOfEnemySpawned + " numofEnem= " + Phases[PhaseNum].NumOfEnemies);
            WHandler.UpdateWaveState(WaveHandler.WaveState.PhaseComplete);
            //CheckIfWaveComplete();
        }
        else if (WHandler.NumOfEnemyDestroyed == TotalNumOFEnemies
            && Phases[PhaseNum].lastPhaseinWave)
        {
            Debug.Log("2nd Cond");
            WaveDone = true;
            WHandler.UpdateWaveState(WaveHandler.WaveState.AllPhasesInWaveDone);
            //CheckIfWaveComplete();
        }
    }
    public int GetNumOfEnemiesInPhase()
    {
        return Phases[PhaseNum].NumOfEnemies;
    }


    void CalculateNumOfEnemyInWave()
    {
        int t;

        for (int i = 0; i < Phases.Length; i++)
        {
            for (int j = 0; j < Phases[i].Enemies.Length; j++)
            {
                Phases[i].NumOfEnemies += Phases[i].Enemies[j].Amount;
                t = Phases[i].Enemies[j].Amount;
                TotalNumOFEnemies += t;
                t = 0;
            }
        }
    }

    void NamePhases()
    {
        for (int i = 0; i < Phases.Length; i++)
        {
            int N = i + 1;
            Phases[i].Name = "Phase " + N;
        }
    }
}
