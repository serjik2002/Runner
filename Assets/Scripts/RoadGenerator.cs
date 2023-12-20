using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Road _roadPrefab;
    [SerializeField] private int _roadsCount;

    private ObjectPool<Road> _pool;
    private Road _lastRoadSegment;
    private List<Road> _activeRoadSegments;

    public ObjectPool<Road> Pool => _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Road>(_roadPrefab, _roadsCount, false);
    }
    private void Start()
    {
        var firstSegment = _pool.GetObjectFromPool();
        firstSegment.gameObject.transform.position = Vector3.zero;
        _lastRoadSegment = firstSegment;


    }

    private void Update()
    {
        //SpawnRoadSegments();
        if (_pool.TryGetObject())
        {
            var segment = _pool.GetObjectFromPool();
            segment.gameObject.transform.position = _lastRoadSegment.transform.position + new Vector3(0, 0, 19);
            _lastRoadSegment = segment;
        }
    }

    public void SpawnRoadSegments()
    {
        Road segment = _pool.GetObjectFromPool();
        _activeRoadSegments.Add(segment);
        _lastRoadSegment = segment;
        segment.transform.position = _lastRoadSegment.transform.position + new Vector3(0, 0, 19);
        

        foreach (var item in _activeRoadSegments)
        {
            if (item.gameObject.transform.position.z < Camera.main.transform.position.z - 20);
            {
                _pool.ReturnObjectToPool(item);
                _activeRoadSegments.Remove(item);
            }

        }
    }
}
