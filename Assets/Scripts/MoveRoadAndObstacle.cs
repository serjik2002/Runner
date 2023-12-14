using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoadAndObstacle : MonoBehaviour
{
    private int _speed = 5;

    void Update()
    {
        transform.position -= new Vector3(0, 0, 1) * _speed * Time.deltaTime;
    }
}
