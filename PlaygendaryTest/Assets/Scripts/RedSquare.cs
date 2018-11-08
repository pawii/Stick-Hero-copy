using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSquare : PartOfPlatform
{
    public static readonly float Width;


    static RedSquare()
    {
        Width = 0.5f;
    }


    protected override void HandleOnMovePlatform()
    {
        if(state == States.Behind)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
