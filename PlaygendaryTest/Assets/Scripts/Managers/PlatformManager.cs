using UnityEngine;
using System;

public class PlatformManager : MonoBehaviour
{
    public const float PLATFORM_MOVING_TIME = 0.5f;
    public const float CENTER_PLATFORM_OFFSET = 0.3f;


    public static event Action OnMovePlatform;


    [SerializeField]
    private float minPlatformDistance;
    [SerializeField]
    private float maxPlatformDistance;
    [SerializeField]
    private float minPlatformWidth;
    [SerializeField]
    private float maxPlatformWidth;


    public static float OldDistance { get; private set; }  // Distance between edges platforms
    public static float NewDistance { get; private set; }  // (not between centers)
    public static float CenterPlatformWidth { get; private set; }
    public static float FontPlatformWidth { get; private set; }
    public static float BehindPlatformWidth { get; private set; }


    #region Unity lifecycle

    private void OnEnable()
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


    private void OnDisable()
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
        NewDistance = UnityEngine.Random.Range(minPlatformDistance, maxPlatformDistance);

        CenterPlatformWidth = FontPlatformWidth;
        FontPlatformWidth = BehindPlatformWidth;
        BehindPlatformWidth = UnityEngine.Random.Range(minPlatformWidth, maxPlatformWidth);

        OnMovePlatform();
    }

    #endregion
}