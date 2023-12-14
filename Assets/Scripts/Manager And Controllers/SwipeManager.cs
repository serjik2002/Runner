using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private bool detectSwipeOnlyAfterRelease = false;

    public float minDistanceForSwipe = 20f;

    public Direction GetSwipeDirection()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerUpPosition = touch.position;
                    return CheckSwipe();
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                return CheckSwipe();
            }
        }

        return Direction.None;
    }

    private Direction CheckSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) > minDistanceForSwipe)
        {
            float deltaX = fingerUpPosition.x - fingerDownPosition.x;
            float deltaY = fingerUpPosition.y - fingerDownPosition.y;

            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                // Свайп по горизонтали
                return (deltaX > 0) ? Direction.Right : Direction.Left;
            }
            else
            {
                // Свайп по вертикали
                return (deltaY > 0) ? Direction.Up : Direction.Down;
            }
        }

        return Direction.None;
    }
}
