using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraTransit : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;  
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private PanelStartClick _panelStartClick;

    private void Start()
    {
        _panelStartClick.OnClickStartGame.AddListener(StartCameraTransition);
    }


    void StartCameraTransition()
    {
        // Используем DOTween для создания анимации
        transform.DOMove(_targetTransform.position, _transitionDuration)
            .SetEase(Ease.Linear);

        transform.DOLocalRotate(_targetTransform.rotation.eulerAngles, _transitionDuration)
            .SetEase(Ease.Linear);

    }

}
