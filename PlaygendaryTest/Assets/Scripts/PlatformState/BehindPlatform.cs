using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehindPlatform : IPlatformState
{
    #region IPlatformState
    
    public Vector2 MovePlatform(Platform platform)
    {
        platform.transform.position = new Vector2(10, 0);

        Vector2 targetPos = Vector2.zero;
        targetPos.x += Player.StartPosition.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPos.x += PlatformManager.BehindPlatformWidth / 2f;
        targetPos.x += PlatformManager.NewDistance;

        platform.State = new FrontPlatform();

        return targetPos;
    }

    #endregion
}
