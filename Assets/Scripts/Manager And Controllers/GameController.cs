using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using System;
using Firebase.Extensions;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    [SerializeField] private int _startSpeed = 5;
    [SerializeField] private int _speed = 5;
    [SerializeField] private int _maxSpeed = 10;
    [SerializeField] private PanelStartClick _panelStartClick;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _menuUiPanel;
    [SerializeField] private GameObject _gameUiPanel;
    [SerializeField] private int _scoreBetweenSpeedUp = 300;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Button _restartGameButton;
    [SerializeField] private int _speedStep = 2;
    
    private int _score;
    private DatabaseReference _databaseReference;

    public int Score => _score;
    public int Speed => _speed;

    public UnityEvent OnScoreAdded;
    private readonly string _leaderboard = "leaderboard";

    private void Start()
    {
        _panelStartClick.OnClickStartGame.AddListener(StartGameLoop);
        _restartGameButton.onClick.AddListener(Resetvalues);
        _playerController.OnPlayerDie.AddListener(()=> {
            UpdateScoreInDB(_score);
            _speed = 0;
        });
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private async void UpdateScoreInDB(int newScore)
    {
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        DatabaseReference userReference = _databaseReference.Child(_leaderboard).Child(userId);

        // Получение текущего счета из базы данных
        string json = null;
        await userReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Обработка ошибки
                Debug.LogError("Ошибка при получении данных: " + task.Exception);
                return;
            }

            var value = task.Result.Value?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                ScoreData data = JsonUtility.FromJson<ScoreData>(value);

                // Проверка, больше ли новый счет предыдущего
                if (newScore > data.Score)
                {
                    data.Score = newScore;
                    json = JsonUtility.ToJson(data);
                }
                else
                {
                    // Новый счет меньше или равен предыдущему, не обновляем
                    Debug.Log("Новый счет меньше или равен предыдущему, не обновляем");
                }
            }
            else
            {
                // Первый раз добавляем счет в базу данных
                ScoreData newData = new ScoreData { Score = newScore };
                json = JsonUtility.ToJson(newData);
            }
        });

        // Обновление счета в базе данных
        if (!string.IsNullOrEmpty(json))
        {
            await userReference.SetValueAsync(json).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Обработка ошибки
                    Debug.LogError("Ошибка при обновлении данных: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    // Успешное обновление данных
                    Debug.Log("Данные успешно обновлены");
                }
            });
        }
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
        if(_playerController.IsRunning)
        {
            _score++;
            OnScoreAdded.Invoke();

            if (_score != 0)
            {
                if (_score % _scoreBetweenSpeedUp == 0 && _speed <= _maxSpeed)
                {
                    _speed += _speedStep;
                }
            }
        }
        
        
    }

    private void Resetvalues()
    {
        _speed = _startSpeed;
        _score = 0;
    }


}
