using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoadAndObstacle : MonoBehaviour
{
    private GameController _gameController;
    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }
    void Update()
    {
        transform.position -= new Vector3(0, 0, 1) * _gameController.Speed * Time.deltaTime;
    }
}
