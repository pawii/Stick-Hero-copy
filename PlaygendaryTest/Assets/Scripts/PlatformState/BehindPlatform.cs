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
        targetPos.x += Player.StartPos.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPos.x += PlatformManager.BehindPlatformWidth / 2f;
        targetPos.x += PlatformManager.NewDistance;

        platform.State = new FrontPlatform();

        return targetPos;
    }


    public Vector2 RefreshPlatform()
    {
        throw new NotImplementedException();
    }

    #endregion
}
