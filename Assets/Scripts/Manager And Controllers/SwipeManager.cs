using UnityEngine;
using UnityEngine.Events;

public class SwipeManager : MonoBehaviour
{
    public float swipeThreshold = 50f;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    public static event System.Action<Direction> OnSwipe;

    private void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Vector2 swipeDelta = endTouchPosition - startTouchPosition;

                if (swipeDelta.magnitude > swipeThreshold)
                {
                    Direction swipeDirection = GetSwipeDirection(swipeDelta);
                    OnSwipe?.Invoke(swipeDirection);
                }
            }
        }
    }

    public Direction GetSwipeDirection(Vector2 swipeDelta)
    {
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            return (swipeDelta.x > 0) ? Direction.Right : Direction.Left;
        }
        else if (Mathf.Abs(swipeDelta.x) < Mathf.Abs(swipeDelta.y))
        {
            return (swipeDelta.y > 0) ? Direction.Up : Direction.Down;
        }
        else
        {
            return Direction.None;
        }
    }
}
