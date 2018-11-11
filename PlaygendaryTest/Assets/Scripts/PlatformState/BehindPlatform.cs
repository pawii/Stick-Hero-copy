using UnityEngine;

public class BehindPlatform : IPlatformState
{
    private Vector2 hidePosition = new Vector2(10, 0);


    #region IPlatformState
    
    public Vector2 MovePlatform(Platform platform)
    {
        platform.transform.position = hidePosition;

        Vector2 targetPosition = Vector2.zero;
        targetPosition.x += Player.StartPosition.x + PlatformManager.CENTER_PLATFORM_OFFSET;
        targetPosition.x += PlatformManager.BehindPlatformWidth * MathConsts.HALF_COEFFICIENT;
        targetPosition.x += PlatformManager.NewDistance;

        platform.State = new FrontPlatform();

        return targetPosition;
    }

    #endregion
}
