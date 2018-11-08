using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CenterPlatform : IPlatformState
{
    #region IPlatformState
    
    public Vector2 MovePlatform(Platform platform)
    {
        Vector2 targetPos = Vector2.zero;
        targetPos.x = Player.StartPosition.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPos.x -= PlatformManager.CenterPlatformWidth / 2f;
        targetPos.x -= PlatformManager.FontPlatformWidth;
        targetPos.x -= PlatformManager.OldDistance;

        platform.State = new BehindPlatform();

        return targetPos;
    }

    #endregion
}
