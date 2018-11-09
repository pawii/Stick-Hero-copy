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


    #region Fields

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerFallSpeed;
    [SerializeField]
    private float playerFallDistance;
    [SerializeField]
    private AudioSource fallAudio;


    private Vector2 playerPosition;
    private Vector2 statrMovementPosition;
    private Vector2 targetMovementPosition;
    private float fraction;
    private float startTime;
    private float movementTime;
    private bool isMove;
    private PlayerMovementState movementState;

    #endregion


    public static Vector2 StartPosition { get; private set; }


    #region Unity lifecycle

    private void Awake()
    {
        StartPosition = new Vector2(-5, 0.42f);
        playerPosition = transform.position;
        movementState = PlayerMovementState.TwoPoint;

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

                switch (movementState)
                {
                    case PlayerMovementState.Horizontal:
                        OnScoreUp();
                        OnMoveNext();
                        OnPlayerEndHorizontalMovement();
                        StartMove(StartPosition, PlatformManager.MOVE_TIME, PlayerMovementState.Back);
                        break;

                    case PlayerMovementState.FallHorizontal:
                        OnPlayerEndHorizontalMovement();
                        OnStickFallDown();
                        targetMovementPosition = playerPosition;
                        targetMovementPosition.y -= playerFallDistance;
                        float movementTime = playerFallDistance / playerFallSpeed;
                        StartMove(targetMovementPosition, movementTime, PlayerMovementState.FallDown);
                        break;

                    case PlayerMovementState.FallDown:
                        OnEndGame();
                        fallAudio.Play();
                        break;
                }

                anim.SetBool("isRun", false);
            }

            playerPosition = Vector2.Lerp(statrMovementPosition, targetMovementPosition, fraction);
            transform.position = playerPosition;
        }
    }

    #endregion


    #region Event handlers

    private void Player_OnStickFallHorizontal(float stickSize)
    {
        float endOfPlatform = PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        bool isFall = (stickSize < PlatformManager.NewDistance) || stickSize > endOfPlatform;
        movementState = isFall ? PlayerMovementState.FallHorizontal : PlayerMovementState.Horizontal;

        Vector2 targetPosition = StartPosition;
        float movementTime;

        if (isFall)
        {
            if (stickSize > 15)
            {
                stickSize = 15;
            }
            targetPosition.x += stickSize;
            movementTime = stickSize / playerSpeed;
        }
        else
        {
            targetPosition.x += PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
            movementTime = (PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth) / playerSpeed;

            float startRedSquare = PlatformManager.NewDistance + (PlatformManager.BehindPlatformWidth / 2f) - (RedSquare.Width / 2f);
            float endRedSquare = startRedSquare + RedSquare.Width;
            if (stickSize > startRedSquare && stickSize < endRedSquare)
            {
                OnScoreUp();
            }
        }
        
        OnPlayerStartHorizontalMovement();
        StartMove(targetPosition, movementTime, movementState);
    }


    private void Player_OnReloadGame()
    {
        StartTwoPointMove(StartPosition, PlatformManager.MOVE_TIME);
    }

    #endregion


    #region Private methods

    private void StartTwoPointMove(Vector2 targetPosition, float movementTime)
    {
        this.targetMovementPosition = targetPosition;
        this.movementTime = movementTime;

        statrMovementPosition = playerPosition;
        startTime = Time.realtimeSinceStartup;
        fraction = 0;
        isMove = true;
        movementState = PlayerMovementState.TwoPoint;
    }


    private void StartMove(Vector2 targetPosition, float movementTime, PlayerMovementState movementState)
    {
        StartTwoPointMove(targetPosition, movementTime);

        this.movementState = movementState;

        anim.SetBool("isRun", true);
    }

    #endregion
}
