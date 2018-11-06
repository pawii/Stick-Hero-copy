using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRectangle : PartOfPlatform
{

    protected override void HandleOnMovePlatform()
    {
        if (state == States.Behind)
        {
            transform.localScale = new Vector2(PlatformManager.BehindPlatformWidth, 1);
        }
    }

}