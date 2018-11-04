using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformManager : MonoBehaviour
{
    private GameObject platform1;
    private GameObject platform2;

    public static event Action<float> ChangePlatform1Size;
    public static event Action<float> ChangePlatform2Size;


    private void Awake()
    {
        platform1 = Resources.Load<GameObject>("Platform");
        platform2 = Resources.Load<GameObject>("Platform");
    }


    private void RandomizePlatform(GameObject platform)
    {
        throw new NotImplementedException();
    }
}
