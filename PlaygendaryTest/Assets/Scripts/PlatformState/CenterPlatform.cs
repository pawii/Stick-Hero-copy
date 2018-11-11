using UnityEngine;

public class CenterPlatform : IPlatformState
{
    #region IPlatformState
    
    public Vector2 MovePlatform(Platform platform)
    {
        Vector2 targetPosition = Vector2.zero;
        targetPosition.x = Player.StartPosition.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPosition.x -= PlatformManager.CenterPlatformWidth * MathConsts.HALF_COEFFICIENT;
        targetPosition.x -= PlatformManager.FontPlatformWidth;
        targetPosition.x -= PlatformManager.OldDistance;

        platform.State = new BehindPlatform();

        return targetPosition;
    }

    #endregion
}
