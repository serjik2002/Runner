using UnityEngine;

public class MobileControl : PlatformInputConroller
{

    private SwipeManager _swipeManager;

    public override PlatformInputConroller CheckPlatform()
    {
#if UNITY_ANDROID||UNITY_IOS
        return this;
#else
        return null;
#endif
    }

    public override Direction PerformControl()
    {
        return (Direction)_swipeManager.GetSwipeDirection();
    }

    private void HandleSwipe(SwipeManager.Direction direction)
    {
        switch (direction)
        {
            case SwipeManager.Direction.Up:
                // Обработка свайпа вверх
                Debug.Log("Up");
                break;
            case SwipeManager.Direction.Down:
                // Обработка свайпа вниз
                Debug.Log("Down");
                break;
            case SwipeManager.Direction.Left:
                // Обработка свайпа влево
                Debug.Log("Left");
                break;
            case SwipeManager.Direction.Right:
                // Обработка свайпа вправо
                Debug.Log("Right");
                break;
            case SwipeManager.Direction.None:
                Debug.Log("No swipe");
                break;
        }
    }


}
