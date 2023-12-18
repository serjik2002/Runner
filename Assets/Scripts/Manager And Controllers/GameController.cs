using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    [SerializeField] private int _startSpeed = 5;
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _maxSpeed = 25;
    [SerializeField] private PanelStartClick _panelStartClick;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _menuUiPanel;
    [SerializeField] private GameObject _gameUiPanel;
    [SerializeField] private int _scoreBetweenSpeedUp = 300;
    [SerializeField] private PlayerController _playerController;
    private int _score;

    public int Score => _score;
    public int Speed => _speed;

    public UnityEvent OnScoreAdded;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }
    private void Start()
    {
        _panelStartClick.OnClickStartGame.AddListener(StartGameLoop);
        _playerController.OnPlayerDie.AddListener(Resetvalues);
    }

    private void StartGameLoop()
    {
        _panelStartClick.enabled = false;
        _roadGenerator.enabled = true;
        _obstacleSpawner.enabled = true;
        _rigidbody.useGravity = true;
        _menuUiPanel.SetActive(false);
        _gameUiPanel.SetActive(true);

        InvokeRepeating("IncreaseScore", 0, 0.1f);
    }

    private void IncreaseScore()
    {
        _score++;
        OnScoreAdded.Invoke();

        if (_score != 0)
        {
            if (_score % _scoreBetweenSpeedUp == 0 && _speed <= _maxSpeed)
            {
                _speed += 5;
            }
        }
        
    }

    private void Resetvalues()
    {
        _speed = _startSpeed;
        _score = 0;
        
    }


}
