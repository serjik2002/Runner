using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private int _roadsCount;
    [SerializeField] private int _speed = 5;
    [SerializeField] private Transform _nextSpawnTransform;

    private List<GameObject> _roadPool;
    private GameObject _lastRoadSegment;

    
    private void Start()
    {
        _roadPool = new List<GameObject>();
        InitializeObjectPool();
    }

    private void Update()
    {
        SpawnRoadSegments();
        MoveRoad();
    }

    public void InitializeObjectPool()
    {
        for (int i = 0; i < _roadsCount; i++)
        {
            var roadSegment = Instantiate(_roadPrefab);
            roadSegment.SetActive(false);
            _roadPool.Add(roadSegment);
        }
    }

    public void MoveRoad()
    {
        foreach (var item in _roadPool)
        {
            item.transform.position -= new Vector3(0, 0, 1) * _speed * Time.deltaTime;
        }
    }

    public void SpawnRoadSegments()
    {
        if(_lastRoadSegment == null)
        {
            _roadPool[0].transform.position = Vector3.zero;
            _roadPool[0].SetActive(true);
            _lastRoadSegment = _roadPool[0];
        }
        foreach (var item in _roadPool)
        {
            if(!item.activeSelf)
            {
                item.transform.position = _lastRoadSegment.transform.position + new Vector3(0, 0, 19);
                item.SetActive(true);
                _lastRoadSegment = item;
            }
            else if(item.transform.position.z < Camera.main.transform.position.z - 20)
            {
                item.SetActive(false);
            }
        }
    }
}
