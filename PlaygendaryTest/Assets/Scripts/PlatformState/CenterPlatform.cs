using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPlatform : IPlatformState
{
    public void MovePlatform(Platform platform, float oldDistance, float newDistance)
    {
        Vector2 targetPos = PlatformManager.StartPos;
        targetPos.x -= oldDistance;
        platform.transform.position = targetPos;

        platform.State = new BehindPlatform();
    }
}
