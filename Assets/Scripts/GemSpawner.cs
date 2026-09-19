using ChickenCoop.Util;
using System.Collections.Generic; 
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenSpawns = 3;

    [SerializeField]
    private float shortestTimeBetweenSpawns = 0.4f;

    [SerializeField]
    private float timeToReachShortest = 30;

    [SerializeField]
    private AnimationCurve timeCurve;

    [SerializeField]
    private List<GridObject> gemPrefabs;

    [SerializeField]
    private GridObject chestPrefab;

    private List<GridObject> gridObjects = new();

    private float time;
    private float currentTimeBetweenSpawns;
    private float timeSinceLastSpawn;

    private void Start()
    {
        timeSinceLastSpawn = timeBetweenSpawns / 2;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Ended) return;

        time += Time.deltaTime;

        float t = Mathf.InverseLerp(0, timeToReachShortest, time);
        currentTimeBetweenSpawns = Mathf.Lerp(timeBetweenSpawns, shortestTimeBetweenSpawns, timeCurve.Evaluate(t));

        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn > currentTimeBetweenSpawns)
        {
            timeSinceLastSpawn = 0;

            SpawnGem();
        }
    }

    public void SpawnGem()
    {
        GridObject gridObject = Instantiate(gemPrefabs.GetRandomElement());
        gridObjects.Add(gridObject);
    }
}
