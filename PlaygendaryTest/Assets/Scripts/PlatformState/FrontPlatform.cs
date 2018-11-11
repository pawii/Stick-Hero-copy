using UnityEngine;

public class FrontPlatform : IPlatformState
{
    #region IPlatformState 

    public Vector2 MovePlatform(Platform platform)
    {
        Vector2 targetPosition = Vector2.zero;
        targetPosition.x = Player.StartPosition.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPosition.x -= PlatformManager.FontPlatformWidth * MathConsts.HALF_COEFFICIENT;

        platform.State = new CenterPlatform();

        return targetPosition;
    }

    #endregion
}
