using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    //Definitions

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
    public List<GameObject> EnemyList = new List<GameObject>();
    public List<PreferredSpawnType> SpawnTypeList = new List<PreferredSpawnType>();


    [Header("Phases Attributes")]
    public Phase[] Phases;
    [Space]
    [SerializeField] private int TotalNumOFEnemies = 0;
    private int RandomNum;

    private GameObject EnemyToSpawn;
    private PreferredSpawnType ST;

    private float nextActionTime = 0.0f;
    private float period = 0.5f;
    private bool WaveDone = false;
    private Vector2 Spawnpos;

    private void Awake()
    {
        NamePhases();

    }

    void Start()
    {
        CalculateNumOfEnemyInWave();
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

    void SpawnType(PreferredSpawnType PST)
    {
        int SpawnPoint;

        switch (PST)
        {
            case PreferredSpawnType.Random:
                //SetRandomSpawnPos();
                //Debug.Log("enemy spawned from Random pos" + Spawnpos);
                Vector2 min = WHandler.GridPoints.GetComponent<PointsContainers>().NormalSpawnPoints[0].transform.position;
                Vector2 max = WHandler.GridPoints.GetComponent<PointsContainers>().NormalSpawnPoints[6].transform.position;
                Spawnpos = new Vector2(max.x, Random.Range(min.y, max.y));
                break;

            case PreferredSpawnType.SpawnPoints:
                SpawnPoint = Random.Range(0, WHandler.GridPoints.GetComponent<PointsContainers>().NormalSpawnPoints.Length);
                Spawnpos = WHandler.GridPoints.GetComponent<PointsContainers>().NormalSpawnPoints[SpawnPoint].transform.position;
                //Debug.Log("enemy spawned from Spawn points" + Spawnpos);
                break;

            case PreferredSpawnType.Middle_SP:
                Spawnpos = WHandler.GridPoints.GetComponent<PointsContainers>().NormalSpawnPoints[3].transform.position;
                //Debug.Log("enemy spawned from middle sp");
                break;

            case PreferredSpawnType.TopScreen_SP:
                SpawnPoint = Random.Range(0, WHandler.GridPoints.GetComponent<PointsContainers>().TopSpawnPoints.Length);
                Spawnpos = WHandler.GridPoints.GetComponent<PointsContainers>().TopSpawnPoints[SpawnPoint].transform.position;
                //Debug.Log("enemy spawned from top sp");
                break;

            case PreferredSpawnType.BottomScreen_SP:
                SpawnPoint = Random.Range(0, WHandler.GridPoints.GetComponent<PointsContainers>().BottomSpawnPoints.Length);
                Spawnpos = WHandler.GridPoints.GetComponent<PointsContainers>().BottomSpawnPoints[SpawnPoint].transform.position;
                //Debug.Log("enemy spawned from bottom sp");
                break;
        }
    }

    public void AddEnemyToList()
    {
        List<GameObject> EnemyGenerated = new List<GameObject>();
        List<PreferredSpawnType> SpawnTypeGenerated = new List<PreferredSpawnType>();

        for (int XEnemiesAdded = 0; XEnemiesAdded != Phases[PhaseNum].NumOfEnemies; XEnemiesAdded++)
        {
            RandomNum = Random.Range(0, Phases[PhaseNum].Enemies.Length);

            if (Phases[PhaseNum].Enemies[RandomNum].Amount > 0)
            {
                EnemyToSpawn = Phases[PhaseNum].Enemies[RandomNum].EnemiesPrefap;
                ST = Phases[PhaseNum].Enemies[RandomNum].HowTospawn;

                EnemyGenerated.Add(EnemyToSpawn);
                SpawnTypeGenerated.Add(ST);

                Phases[PhaseNum].Enemies[RandomNum].Amount--;
            }
            else
            {
                XEnemiesAdded--;
            }
        }

        EnemyList.Clear();
        SpawnTypeList.Clear();
        EnemyList = EnemyGenerated;
        SpawnTypeList = SpawnTypeGenerated;
    }

    private void SpawnEnemy()
    {
        if (EnemyList.Count > 0)
        {
            SpawnType(SpawnTypeList[0]);
            EnemyToSpawn = EnemyList[0];
            Instantiate(EnemyToSpawn, Spawnpos, Quaternion.identity);
            WHandler.NumOfEnemySpawned++;

            EnemyList.RemoveAt(0);
            SpawnTypeList.RemoveAt(0);
        }
    }

    public IEnumerator StartSpawning()
    {
        if (WHandler.NumOfEnemySpawned != Phases[PhaseNum].NumOfEnemies)
        {
            AddEnemyToList();
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < Phases[PhaseNum].NumOfEnemies; i++)
            {
                if (!Pause.isGamePaused)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(Phases[PhaseNum].TimeBetweenEnemySpawn);
                }
            }
        }
    }

    /*
    private int EnemyArrayLength;
    public void SpawnEnemy()
    {
        EnemyArrayLength = Phases[PhaseNum].Enemies.Length;
        RandomNum = Random.Range(0, EnemyArrayLength);

        if (Phases[PhaseNum].Enemies[RandomNum].Amount > 0 && !EmptyIndex.Contains(RandomNum))
        {
            SpawnType(PhaseNum, RandomNum);
            EnemyToSpawn = Phases[PhaseNum].Enemies[RandomNum].EnemiesPrefap;
            Instantiate(EnemyToSpawn, Spawnpos, Quaternion.identity);
            Debug.Log("EnemySpawned");
            Phases[PhaseNum].Enemies[RandomNum].Amount--;
            WHandler.NumOfEnemySpawned++;
        }
        else
        {
            if (!EmptyIndex.Contains(RandomNum))
            {
                EmptyIndex.Add(RandomNum);
            }

            SpawnEnemy();
        }
    }

    public IEnumerator StartSpawning()
    {
        if (WHandler.NumOfEnemySpawned != Phases[PhaseNum].NumOfEnemies)
        {
            //EmptyIndex.Clear();
            for (int i = 0; i < Phases[PhaseNum].NumOfEnemies; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(Phases[PhaseNum].TimeBetweenEnemySpawn);
            }
        }
    }
    */


    void CheckIfPhaseComplete()
    {
        if (WHandler.NumOfEnemySpawned == Phases[PhaseNum].NumOfEnemies &&
            /*WHandler.NumOfEnemyDestroyed == Phases[PhaseNum].NumOfEnemies
            &&*/  !Phases[PhaseNum].lastPhaseinWave)
        {
            Debug.Log("1st Cond , spawned = " + WHandler.NumOfEnemySpawned + " numofEnem = " + Phases[PhaseNum].NumOfEnemies);
            WHandler.UpdateWaveState(WaveHandler.WaveState.PhaseComplete);
            //CheckIfWaveComplete();
        }
        else if (WHandler.NumOfEnemyDestroyed == TotalNumOFEnemies && Phases[PhaseNum].lastPhaseinWave)
        {
            //Debug.Log("2nd Cond");
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
