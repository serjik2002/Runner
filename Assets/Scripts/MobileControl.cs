public class MobileControl : PlatformInputConroller
{
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
        return Direction.None;
    }
}
