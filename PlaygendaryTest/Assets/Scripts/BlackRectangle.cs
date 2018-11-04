using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlackRectangle : MonoBehaviour
{
    [SerializeField]
    private float minSize;
    [SerializeField]
    private float maxSize;

    private const int scaleY = 1;


    private void Awake()
    {
        //PlatformManager.NextPlatform += ChangeScale;
    }


    private void OnDestroy()
    {
        //PlatformManager.NextPlatform -= ChangeScale;
    }

    
    private void ChangeScale(float scale)
    {
        throw new NotImplementedException();
        //transform.localScale = new Vector2(Random.Range(minSize, maxSize), scaleY);
    }
}
