using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class UIManager : MonoBehaviour
{

    [SerializeField] private GameController _gameController;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _gameOverPanel;
   // private RectTransform _gameOverPanelRect;

    private void Start()
    {
        //_gameOverPanelRect=GetComponent<RectTransform>();
        _gameController.OnScoreAdded.AddListener(UpdateScoreText);
        _playerController.OnPlayerDie.AddListener(OpenGameOverPanel);
    }

    private void UpdateScoreText()
    {
        _text.text = _gameController.Score.ToString();
    }

    private void OpenGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        //_gameOverPanelRect.transform.DOMove(Vector3.zero, 0.5f);
    }
}
