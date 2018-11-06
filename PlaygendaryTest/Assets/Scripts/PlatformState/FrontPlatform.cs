using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontPlatform : IPlatformState
{
    #region IPlatformState 
    
    public Vector2 MovePlatform(Platform platform)
    {
        Vector2 targetPos = Vector2.zero;
        targetPos.x = Player.StartPos.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPos.x -= PlatformManager.FontPlatformWidth / 2f;

        platform.State = new CenterPlatform();

        return targetPos;
    }

    #endregion
}
