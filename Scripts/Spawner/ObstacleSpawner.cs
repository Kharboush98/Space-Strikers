using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public WaveHandler WHandler;

    [System.Serializable]
    public class Obstacle
    {
        public GameObject Obstacle_GO;
        public int WaveNumToPlay;
        [HideInInspector] public bool Spawned = false;
    }
    public Obstacle[] Obstacles;


    void Start()
    {
        
    }

    void Update()
    {
        ObstaclesToSpawn();
    }

    void ObstaclesToSpawn()
    {
        if (WHandler.ActiveWaveID == Obstacles[0].WaveNumToPlay 
            && !Obstacles[0].Spawned)
        {
            Obstacles[0].Spawned = true;
            Instantiate(Obstacles[0].Obstacle_GO,transform.position,Quaternion.identity);
        }
    }

}
