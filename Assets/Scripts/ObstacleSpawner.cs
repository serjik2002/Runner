
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _obstaclePrefabs;
    [SerializeField] private int _spawnInterval;
    [SerializeField] private int _timeToBeginSpawn;
    [SerializeField] private Transform[] _obstaclePositions;

    private List<GameObject> _obstaclesPool = new List<GameObject>();
    private float _time = 0;
    private void Start()
    {
        InitializePool();
        SpawnObstacle();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time>=_spawnInterval)
        {
            SpawnObstacle();
            _time = 0;
        }
    }
    private void InitializePool()
    {
        foreach (var item in _obstaclePrefabs)
        {
            var obstacle = Instantiate(item);
            obstacle.SetActive(false);
            _obstaclesPool.Add(obstacle);
        }

    }

    public void SpawnObstacle()
    {
        int lanesToSpawn = Random.Range(1, _obstaclePositions.Length + 1);

        List<int> availableLanes = new List<int>(_obstaclePositions.Length);
        for (int i = 0; i < _obstaclePositions.Length; i++)
        {
            availableLanes.Add(i);
        }

        for (int i = 0; i < lanesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableLanes.Count);
            int randomLane = availableLanes[randomIndex];
            availableLanes.RemoveAt(randomIndex);

            GameObject obstacle = GetPooledObstacle();
            if (obstacle != null)
            {
                obstacle.transform.position = _obstaclePositions[randomLane].position;
                obstacle.SetActive(true);
            }
        }

    }

    GameObject GetPooledObstacle()
    {
        foreach (GameObject obstacle in _obstaclesPool)
        {
            if (!obstacle.activeInHierarchy)
            {
                return obstacle;
            }
        }
        int index = Random.Range(0, _obstaclePrefabs.Length);
        GameObject newObstacle = Instantiate(_obstaclePrefabs[index], transform.position, Quaternion.identity);
        newObstacle.SetActive(false);
        _obstaclesPool.Add(newObstacle);

        return newObstacle;
    }


    public void DeactivateObstacles()
    {
        foreach (var item in _obstaclesPool)
        {
            if(item.activeSelf)
            {
                if (item.transform.position.z < Camera.main.transform.position.z - 20)
                    item.SetActive(false);
            }
        }
    }

    
}
