using UnityEngine;


public class ComputerControl : PlatformInputConroller
{
    public override PlatformInputConroller CheckPlatform()
    {
#if UNITY_STANDALONE||UNITY_EDITOR
        return this;
#else
        return null;
#endif
    }

    public override SwipeManager.Direction PerformControl()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            return SwipeManager.Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            return SwipeManager.Direction.Right;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            return SwipeManager.Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            return SwipeManager.Direction.Down;
        }

        return SwipeManager.Direction.None;
        
    }
}
