using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnMoveNext;
    public static event Action OnStickFallDown;
    public static event Action OnScoreUp;
    public static event Action OnPlayerStartHorizontalMovement;
    public static event Action OnPlayerEndHorizontalMovement;
    public static event Action OnEndGame;


    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerFallSpeed;


    public static Vector2 StartPos { get; private set; }

    
    private Vector2 playerPos;
    private Vector2 statrPosition;
    private Vector2 targetPosition;
    private float fraction;
    private float startTime;
    private float movementTime;
    private bool isMove;
    private bool isFall;
    private bool isContinueMove;


    #region Unity lifecycle

    private void Awake()
    {
        StartPos = new Vector2(-5, 0.42f);
        playerPos = transform.position;

        StartMenu.OnStartGame += Player_OnReloadGame;
        Stick.OnStickFallHorizontal += Player_OnStickFallHorizontal;
        EndMenu.OnReloadGame += Player_OnReloadGame;
    }


    private void OnDestroy()
    {
        StartMenu.OnStartGame -= Player_OnReloadGame;
        Stick.OnStickFallHorizontal -= Player_OnStickFallHorizontal;
        EndMenu.OnReloadGame -= Player_OnReloadGame;
    }


    private void Update()
    {
        if (isMove)
        {
            fraction = (Time.realtimeSinceStartup - startTime) / movementTime;

            if (fraction > 1f)
            {
                fraction = 1;

                isMove = false;

                if (isContinueMove && isFall)
                {
                    OnPlayerEndHorizontalMovement();
                }

                if (!isContinueMove)
                {
                    anim.SetBool("isRun", false);

                    if (!isFall)
                    {
                        OnPlayerEndHorizontalMovement();
                    }
                    else
                    {
                        OnEndGame();
                    }
                }
            }

            playerPos = Vector2.Lerp(statrPosition, targetPosition, fraction);
            transform.position = playerPos;

            if (!isMove && isContinueMove)
            {
                if (isFall)
                {
                    OnStickFallDown();

                    targetPosition = playerPos;
                    targetPosition.y -= 10;
                    movementTime = PlatformManager.MOVE_TIME;

                    anim.SetBool("isRun", false);
                }
                else
                {
                    OnScoreUp();
                    OnMoveNext();

                    targetPosition = StartPos;
                    movementTime = PlatformManager.MOVE_TIME;

                }

                statrPosition = playerPos;
                startTime = Time.realtimeSinceStartup;
                fraction = 0;

                isMove = true;
                isContinueMove = false;
            }
        }
    }

    #endregion


    #region Private methods

    private void StartMove(Vector2 targetPosition, float movementTime)
    {
        this.targetPosition = targetPosition;
        this.movementTime = movementTime;

        statrPosition = playerPos;
        startTime = Time.realtimeSinceStartup;
        fraction = 0;
        isMove = true;
        isFall = false;
        isContinueMove = false;
    }


    private void StartHorizontalMove(Vector2 targetPosition, float movementTime, bool isFall)
    {
        StartMove(targetPosition, movementTime);

        this.isFall = isFall;
        
        isContinueMove = true;

        anim.SetBool("isRun", true);

        OnPlayerStartHorizontalMovement();
    }

    #endregion


    #region Event handlers

    private void Player_OnStickFallHorizontal(float stickSize)
    {
        float endOfPlatform = PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        bool isFall = stickSize < PlatformManager.NewDistance || stickSize > endOfPlatform;

        Vector2 targetPosition = StartPos;
        float movementTime = 1f;

        if (isFall)
        {
            targetPosition.x += stickSize;
            movementTime = stickSize / playerSpeed;
        }
        else
        {
            targetPosition.x += PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
            movementTime = (PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth) / playerSpeed;

            float startRedSquare = PlatformManager.NewDistance + (PlatformManager.BehindPlatformWidth / 2f) - 0.25f;
            float endRedSquare = startRedSquare + 0.5f;
            if (stickSize > startRedSquare && stickSize < endRedSquare)
            {
                OnScoreUp();
            }
        }

        StartHorizontalMove(targetPosition, movementTime, isFall);
    }


    private void Player_OnReloadGame()
    {
        StartMove(StartPos, PlatformManager.MOVE_TIME);
    }

    #endregion
}
