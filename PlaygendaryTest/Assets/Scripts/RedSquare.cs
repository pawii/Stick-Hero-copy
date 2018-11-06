using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSquare : PartOfPlatform
{

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
