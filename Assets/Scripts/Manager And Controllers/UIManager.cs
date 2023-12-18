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

    [SerializeField] private GameController _gameController;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameLoopPanel;
    [SerializeField] private AdManager _adManager;

    [SerializeField] private Button _showAdButton;
    [SerializeField] private Button _restartGameButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private TMP_Text _gameOverScore;

    private FirebaseAuth _auth;
    // private RectTransform _gameOverPanelRect;

    private void Start()
    {
        _auth = FirebaseAuth.DefaultInstance;
        //_gameOverPanelRect=GetComponent<RectTransform>();
        _gameController.OnScoreAdded.AddListener(UpdateScoreText);
        _playerController.OnPlayerDie.AddListener(()=> 
        {
            OpenPanel(_gameOverPanel);
            _gameOverScore.text = "You score: " + _gameController.Score.ToString();

        });
        _showAdButton.onClick.AddListener(()=> 
        {
            _adManager.LoadRewardedAd();
            _adManager.ShowRewardedAd();
        });
        _restartGameButton.onClick.AddListener(() =>
        {
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

    }

    private void UpdateScoreText()
    {
        _text.text = _gameController.Score.ToString();
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

    public void SignOut()
    {
        SceneManager.LoadScene("MainMenu");
        _auth.SignOut();
    }
}
