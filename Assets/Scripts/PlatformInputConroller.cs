using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlatformInputConroller : MonoBehaviour
{
    public abstract Direction PerformControl();
    public abstract PlatformInputConroller CheckPlatform();


}
