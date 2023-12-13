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

    public override Direction PerformControl()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            return Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            return Direction.Right;
        }

        // Optionally, you can handle jumping as well
        if (Input.GetKeyDown(KeyCode.W))
        {
            return Direction.Up;
        }

        return Direction.None;
        
    }
}
