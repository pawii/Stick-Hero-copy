using UnityEngine;

public class BlackRectangle : PartOfPlatform
{
    private const float HEIGHT = 1;


    protected override void HandleOnMovePlatform()
    {
        if (state == States.Behind)
        {
            transform.localScale = new Vector2(PlatformManager.BehindPlatformWidth, HEIGHT);
        }
    }
}