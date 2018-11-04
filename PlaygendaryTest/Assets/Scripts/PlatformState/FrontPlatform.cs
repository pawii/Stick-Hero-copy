using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontPlatform : MonoBehaviour, IPlatformState
{
    public void MovePlatform(Platform platform, float oldDistance, float newDistance)
    {
        platform.transform.position = PlatformManager.StartPos;

        platform.State = new CenterPlatform();
    }
}
