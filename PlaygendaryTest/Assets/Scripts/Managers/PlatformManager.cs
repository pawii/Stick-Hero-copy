using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformManager : MonoBehaviour
{
    public const float MOVE_TIME = 0.5f;
    public const float CENTER_PLATFORM_OFFSET = 0.3f;


    public static event Action OnMovePlatform;


    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float minPlatformWidth;
    [SerializeField]
    private float maxPlatformWidth;


    public static float OldDistance { get; private set; }  // дистанция между краями платформ
    public static float NewDistance { get; private set; }  // (не между центрами)
    public static float CenterPlatformWidth { get; private set; }
    public static float FontPlatformWidth { get; private set; }
    public static float BehindPlatformWidth { get; private set; }


    #region Unity lifecycle

    private void Awake()
    {
        OldDistance = 4;
        NewDistance = 4;
        CenterPlatformWidth = 1;
        FontPlatformWidth = 1;
        BehindPlatformWidth = 1;

        Player.OnMoveNext += PlatformManager_OnMoveNext;
        StartMenu.OnStartGame += PlatformManager_OnMoveNext;
        EndMenu.OnReloadGame += PlatformManager_OnMoveNext;
    }


    private void OnDestroy()
    {
        Player.OnMoveNext -= PlatformManager_OnMoveNext;
        StartMenu.OnStartGame -= PlatformManager_OnMoveNext;
        EndMenu.OnReloadGame -= PlatformManager_OnMoveNext;
    }

    #endregion


    #region Event handlers

    private void PlatformManager_OnMoveNext()
    {
        OldDistance = NewDistance;
        NewDistance = UnityEngine.Random.Range(minDistance, maxDistance);

        CenterPlatformWidth = FontPlatformWidth;
        FontPlatformWidth = BehindPlatformWidth;
        BehindPlatformWidth = UnityEngine.Random.Range(minPlatformWidth, maxPlatformWidth);

        OnMovePlatform();
    }

    #endregion
}