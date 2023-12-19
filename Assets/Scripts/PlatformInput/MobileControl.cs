using UnityEngine;

public class MobileControl : PlatformInputConroller
{
    private SwipeManager swipeManager;
    private Direction currentSwipeDirection;
    private bool _swiped = false;
    private void Start()
    {
        swipeManager = FindObjectOfType<SwipeManager>();

        if (swipeManager != null)
        {
            SwipeManager.OnSwipe += HandleSwipe;
        }
    }

    private void OnDestroy()
    {
        if (swipeManager != null)
        {
            SwipeManager.OnSwipe -= HandleSwipe;
        }
    }

    public override PlatformInputConroller CheckPlatform()
    {
        return this;
    }

    public override Direction PerformControl()
    {
        if(_swiped)
        {
            _swiped = false;
            return currentSwipeDirection;
        }    
        return Direction.None;
    }

    private void HandleSwipe(Direction swipeDirection)
    {
        _swiped = true;
        currentSwipeDirection = swipeDirection;
    }
}
