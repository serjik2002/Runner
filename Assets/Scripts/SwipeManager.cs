using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public UnityEvent<Direction> OnSwipe;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private bool isSwiping = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        fingerDownPosition = eventData.position;
        isSwiping = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSwiping)
        {
            fingerUpPosition = eventData.position;
            CheckSwipe();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSwiping = false;
    }

    private void CheckSwipe()
    {
        float deltaX = fingerUpPosition.x - fingerDownPosition.x;
        float deltaY = fingerUpPosition.y - fingerDownPosition.y;

        Direction swipeDirection = Direction.None;

        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
        {
            swipeDirection = (deltaX > 0) ? Direction.Right : Direction.Left;
        }
        else
        {
            swipeDirection = (deltaY > 0) ? Direction.Up : Direction.Down;
        }

        OnSwipe?.Invoke(swipeDirection);
    }
}