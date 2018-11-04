using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformManager : MonoBehaviour
{
    public static event Action<float, float> MovePlatform;

    // сделать ридонли
    public static Vector2 StartPos { get; private set; }

    private float oldDistance = 8;
    private float newDistance = 8;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;


    private void Awake()
    {
        StartPos = new Vector2(-4, 0);
        Player.MoveNext += OnMoveNext;
    }


    private void OnDestroy()
    {
        Player.MoveNext -= OnMoveNext;
    }


    private void OnMoveNext()
    {
        oldDistance = newDistance;
        newDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        MovePlatform(oldDistance, newDistance);
        Debug.Log(oldDistance);
    }
}
