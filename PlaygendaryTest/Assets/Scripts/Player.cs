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
    private AudioSource fallAudio;


    private Vector2 playerPosition;
    private Vector2 statrMovementPosition;
    private Vector2 targetMovementPosition;
    private float fraction;
    private float startTime;
    private float movementTime;
    private bool isMove;
    private bool isFall;
    private bool isContinueMove;

    #endregion


    public static Vector2 StartPosition { get; private set; }


    #region Unity lifecycle

    private void Awake()
    {
        StartPosition = new Vector2(-5, 0.42f);
        playerPosition = transform.position;

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
                        fallAudio.Play();
                    }
                }
            }

            playerPosition = Vector2.Lerp(statrMovementPosition, targetMovementPosition, fraction);
            transform.position = playerPosition;

            if (!isMove && isContinueMove)
            {
                if (isFall)
                {
                    OnStickFallDown();

                    targetMovementPosition = playerPosition;
                    targetMovementPosition.y -= 10;
                    movementTime = PlatformManager.MOVE_TIME;

                    anim.SetBool("isRun", false);
                }
                else
                {
                    OnScoreUp();
                    OnMoveNext();

                    targetMovementPosition = StartPosition;
                    movementTime = PlatformManager.MOVE_TIME;

                }

                statrMovementPosition = playerPosition;
                startTime = Time.realtimeSinceStartup;
                fraction = 0;

                isMove = true;
                isContinueMove = false;
            }
        }
    }

    #endregion


    #region Event handlers

    private void Player_OnStickFallHorizontal(float stickSize)
    {
        float endOfPlatform = PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        bool isFall = (stickSize < PlatformManager.NewDistance) || stickSize > endOfPlatform;

        Vector2 targetPosition = StartPosition;
        float movementTime;

        if (isFall)
        {
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

        StartHorizontalMove(targetPosition, movementTime, isFall);
    }


    private void Player_OnReloadGame()
    {
        StartMove(StartPosition, PlatformManager.MOVE_TIME);
    }

    #endregion


    #region Private methods

    private void StartMove(Vector2 targetPosition, float movementTime)
    {
        this.targetMovementPosition = targetPosition;
        this.movementTime = movementTime;

        statrMovementPosition = playerPosition;
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
}
