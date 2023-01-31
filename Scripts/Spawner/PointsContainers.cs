using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsContainers : MonoBehaviour
{
    [Header("SpawnPoints")]
    public Transform[] NormalSpawnPoints;
    public Transform[] TopSpawnPoints;
    public Transform[] BottomSpawnPoints;

    [Header("GridPoints")]
    public Transform[] XaxisGrid;
    public Transform[] YaxisGrid;
    public Transform[] CornersGrid;

    [Header("DestinationPoints")]
    public Transform[] NormalDesPoints;
    public Transform[] TopDesPoints;
    public Transform[] BottomDesPoints;
}
