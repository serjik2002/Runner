using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PanelStartClick _panelStartClick;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;

    private void Awake()
    {
        _panelStartClick.OnClickStartGame.AddListener(StartGameLoop);
    }

    private void StartGameLoop()
    {
        _panelStartClick.enabled = false;
        _roadGenerator.enabled = true;
        _obstacleSpawner.enabled = true;
    }

}
