using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnMoveNext;
    public static event Action OnStickStartFallDown;
    public static event Action OnScoreUp;
    public static event Action OnPlayerStartHorizontalMovement;
    public static event Action OnPlayerEndHorizontalMovement;
    public static event Action OnEndGame;


    #region Fields

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float playerHorizontalMovingSpeed;
    [SerializeField]
    private float playerFallSpeed;
    [SerializeField]
    private float playerFallDistance;
    [SerializeField]
    private AudioSource fallAudio;


    private Vector2 playerPosition;
    private Vector2 statrMovementPosition;
    private Vector2 targetMovementPosition;
    private float fractionCoefficient;
    private float startMovingTime;
    private float movementTime;
    private bool isMoving;
    private PlayerMovementState movementState;

    #endregion


    public static Vector2 StartPosition { get; private set; }


    //private Transform trans;


    #region Unity lifecycle

    private void OnEnable()
    {
        //trans = GetComponent<Transform>();
        StartPosition = new Vector2(-5, 0.42f);
        playerPosition = transform.position;
        movementState = PlayerMovementState.TwoPoint;

        StartMenu.OnStartGame += Player_OnReloadGame;
        Stick.OnStickFellHorizontal += Player_OnStickFallHorizontal;
        EndMenu.OnReloadGame += Player_OnReloadGame;
    }


    private void OnDisable()
    {
        StartMenu.OnStartGame -= Player_OnReloadGame;
        Stick.OnStickFellHorizontal -= Player_OnStickFallHorizontal;
        EndMenu.OnReloadGame -= Player_OnReloadGame;
    }


    private void Update()
    {
        if (isMoving)
        {
            fractionCoefficient = (Time.realtimeSinceStartup - startMovingTime) / movementTime;

            if (fractionCoefficient > MathConsts.MAX_FRACTION_COEFFICIENT)
            {
                fractionCoefficient = MathConsts.MAX_FRACTION_COEFFICIENT;

                isMoving = false;

                switch (movementState)
                {
                    case PlayerMovementState.HorizontalBeforeReturn:
                        OnScoreUp();
                        OnMoveNext();
                        OnPlayerEndHorizontalMovement();
                        StartMove(StartPosition, PlatformManager.PLATFORM_MOVING_TIME, PlayerMovementState.Return);
                        break;

                    case PlayerMovementState.HorizontalBeforeFallDown:
                        OnPlayerEndHorizontalMovement();
                        OnStickStartFallDown();
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

                animator.SetBool("isRun", false);
            }

            playerPosition = Vector2.Lerp(statrMovementPosition, targetMovementPosition, fractionCoefficient);
            transform.position = playerPosition;

            /*Profiler.BeginSample("method transform player");
            System.Threading.Thread.Sleep(1000);
            for (int i = 0; i < 1000; i++)
              transform.position = playerPosition;
            Profiler.EndSample();

            Profiler.BeginSample("field transform player");
            System.Threading.Thread.Sleep(1000);
            for (int i = 0; i < 1000; i++)
                trans.position = playerPosition;
            Profiler.EndSample();*/
        }
    }

    #endregion


    #region Event handlers

    private void Player_OnStickFallHorizontal(float stickSize)
    {
        float endOfPlatformDistance = PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        bool isFall = (stickSize < PlatformManager.NewDistance) || stickSize > endOfPlatformDistance;
        movementState = isFall ? PlayerMovementState.HorizontalBeforeFallDown : PlayerMovementState.HorizontalBeforeReturn;

        Vector2 targetPosition = StartPosition;
        float movementTime;

        if (isFall)
        {
            targetPosition.x += stickSize;
            movementTime = stickSize / playerHorizontalMovingSpeed;
        }
        else
        {
            targetPosition.x += PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
            movementTime = (PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth) / playerHorizontalMovingSpeed;

            float startRedSquareDistance = PlatformManager.NewDistance;
            startRedSquareDistance += PlatformManager.BehindPlatformWidth * MathConsts.HALF_COEFFICIENT;
            startRedSquareDistance -= RedSquare.Width * MathConsts.HALF_COEFFICIENT;

            float endRedSquareDistance = startRedSquareDistance + RedSquare.Width;

            if (stickSize > startRedSquareDistance && stickSize < endRedSquareDistance)
            {
                OnScoreUp();
            }
        }
        
        OnPlayerStartHorizontalMovement();
        StartMove(targetPosition, movementTime, movementState);
    }


    private void Player_OnReloadGame()
    {
        StartTwoPointMove(StartPosition, PlatformManager.PLATFORM_MOVING_TIME);
    }

    #endregion


    #region Private methods

    private void StartTwoPointMove(Vector2 targetPosition, float movementTime)
    {
        this.targetMovementPosition = targetPosition;
        this.movementTime = movementTime;

        statrMovementPosition = playerPosition;
        startMovingTime = Time.realtimeSinceStartup;
        fractionCoefficient = MathConsts.MIN_FRACTION_COEFFICIENT;
        isMoving = true;
        movementState = PlayerMovementState.TwoPoint;
    }


    private void StartMove(Vector2 targetPosition, float movementTime, PlayerMovementState movementState)
    {
        StartTwoPointMove(targetPosition, movementTime);

        this.movementState = movementState;

        animator.SetBool("isRun", true);
    }

    #endregion
}
