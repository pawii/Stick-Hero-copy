using UnityEngine;
using System;

public class Platform : MonoBehaviour
{
    public static event Action OnPlatformEndMovement;


    [SerializeField]
    private States startState;


    private bool isMoving;
    private Vector2 startMovingPosition;
    private Vector2 targetMovingPosition;
    private float startMovingTime;
    private float fractionCoefficient;


    public IPlatformState State { get; set; }


    #region Unity lifecycle

    private void OnEnable()
    {
        isMoving = false;

        switch (startState)
        {
            case States.Behind:
                State = new BehindPlatform();
                break;
            case States.Center:
                State = new CenterPlatform();
                break;
            case States.Front:
                State = new FrontPlatform();
                break;
        }

        PlatformManager.OnMovePlatform += Platform_OnMovePlatform;
    }


    private void OnDisable()
    {
        PlatformManager.OnMovePlatform -= Platform_OnMovePlatform;
    }


    private void Update()
    {
        if (isMoving)
        {
            fractionCoefficient = (Time.realtimeSinceStartup - startMovingTime) / PlatformManager.PLATFORM_MOVING_TIME;
            if (fractionCoefficient > MathConsts.MAX_FRACTION_COEFFICIENT)
            {
                fractionCoefficient = MathConsts.MAX_FRACTION_COEFFICIENT;
                isMoving = false;

                OnPlatformEndMovement();
            }

            transform.position = Vector2.Lerp(startMovingPosition, targetMovingPosition, fractionCoefficient);
        }
    }

    #endregion


    #region Event handlers

    private void Platform_OnMovePlatform()
    {
        targetMovingPosition = State.MovePlatform(this);
        startMovingPosition = transform.position;
        startMovingTime = Time.realtimeSinceStartup;
        fractionCoefficient = MathConsts.MIN_FRACTION_COEFFICIENT;
        isMoving = true;
    }

    #endregion
}
