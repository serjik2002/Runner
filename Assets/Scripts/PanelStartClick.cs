using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PanelStartClick : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClickStartGame;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClickStartGame.Invoke();
        }
    }
}
