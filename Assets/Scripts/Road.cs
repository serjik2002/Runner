using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private RoadGenerator _roadGenerator;

    private void Start()
    {
        _roadGenerator = FindObjectOfType<RoadGenerator>();
    }

    public void OffSegment()
    {
        if (gameObject.transform.position.z < Camera.main.transform.position.z - 20)
        {
            _roadGenerator.Pool.ReturnObjectToPool(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Road")
        {
            _roadGenerator.Pool.ReturnObjectToPool(this);
        }
    }
}
