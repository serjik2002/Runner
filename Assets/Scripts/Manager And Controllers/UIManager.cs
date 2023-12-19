using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameController _gameController;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameLoopPanel;
    [SerializeField] private GameObject _leaderBoardPanel;

    [SerializeField] private Button _restartGameButton;
    [SerializeField] private Button _showAdButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _closeLeaderboardButton;

    [SerializeField] private TMP_Text _gameOverScore;
    [SerializeField] private AdManager _adManager;
    [SerializeField] private LeaderBoard _leaderboard;
    [SerializeField] private RectTransform _gameOverPanelRect;

    private FirebaseAuth _auth;


    private void Start()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _gameController.OnScoreAdded.AddListener(UpdateScoreText);
        _playerController.OnPlayerDie.AddListener(()=> 
        {
            OpenPanel(_gameOverPanel);
            _gameOverPanelRect.DOAnchorPos(Vector2.zero, 0.25f);
            _gameOverScore.text = "You score: " + _gameController.Score.ToString();

        });
        _showAdButton.onClick.AddListener(()=> 
        {
            _adManager.LoadRewardedAd();
            _adManager.ShowRewardedAd();
        });
        _restartGameButton.onClick.AddListener(() =>
        {
            _playerController.StartRun();
            ClosePanel(_gameOverPanel);
            Time.timeScale = 1;
        });
        _pauseButton.onClick.AddListener(() =>
        {
            OpenPanel(_pausePanel);
            ClosePanel(_gameLoopPanel);
            Time.timeScale = 0;
            
        });
        _resumeButton.onClick.AddListener(() =>
        {
            OpenPanel(_gameLoopPanel);
            ClosePanel(_pausePanel);
            Time.timeScale = 1;
        });
        _leaderboardButton.onClick.AddListener(async() => 
        {
            OpenPanel(_leaderBoardPanel);
            await _leaderboard.UpdateLeaderboard();
        });
        _closeLeaderboardButton.onClick.AddListener(() =>
        {
            ClosePanel(_leaderBoardPanel);
        });

    }

    private void UpdateScoreText()
    {
        _scoreText.text = _gameController.Score.ToString();
    }

    private void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void SignOut()
    {
        SceneManager.LoadScene("MainMenu");
        _auth.SignOut();
    }
}
