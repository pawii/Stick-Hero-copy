using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private const string CHERRY_TAG = "cherry";
    private const string PLATFORM_TAG = "platform";


    public static event Action OnMoveNext;
    public static event Action OnStickStartFallDown;
    public static event Action OnScoreUp;
    public static event Action OnCherryUp;
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
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float rotationalPositionOffset;

    
    private Vector2 statrMovementPosition;
    private Vector2 targetMovementPosition;
    private float fractionCoefficient;
    private float startMovingTime;
    private float movementTime;
    private bool isMoving = false;
    private bool isPickUpCherry = false;
    private bool isRotationLock = true;

    #endregion


    public static Vector2 StartPosition { get; private set; }


    private PlayerMovementState MovementState { get; set; }


    #region Unity lifecycle

    private void OnEnable()
    {
        StartPosition = new Vector2(-5, 0.42f);
        MovementState = PlayerMovementState.TwoPoint;

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
            if (!isRotationLock)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    RotatePlayer();
                }
            }


            fractionCoefficient = (Time.realtimeSinceStartup - startMovingTime) / movementTime;

            if (fractionCoefficient > MathConsts.MAX_FRACTION_COEFFICIENT)
            {
                fractionCoefficient = MathConsts.MAX_FRACTION_COEFFICIENT;

                isMoving = false;

                switch (MovementState)
                {
                    case PlayerMovementState.HorizontalBeforeReturn:
                        if (isPickUpCherry)
                        {
                            OnCherryUp();
                            isPickUpCherry = false;
                        }
                        OnScoreUp();
                        OnMoveNext();
                        OnPlayerEndHorizontalMovement();
                        
                        StartMove(StartPosition, PlatformManager.PLATFORM_MOVING_TIME, PlayerMovementState.Return);
                        break;

                    case PlayerMovementState.HorizontalBeforeFallDown:
                        isRotationLock = true;

                        OnPlayerEndHorizontalMovement();
                        OnStickStartFallDown();
                        StartFallDownMovement();
                        break;

                    case PlayerMovementState.FallDown:
                        fallAudio.Play();
                        OnEndGame();
                        break;
                }

                animator.SetBool("isRun", false);
            }
            
            playerTransform.position = Vector2.Lerp(statrMovementPosition, targetMovementPosition, fractionCoefficient);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.tag;

        if (collisionTag == CHERRY_TAG)
        {
            isPickUpCherry = true;
        }
        else if (collisionTag == PLATFORM_TAG)
        {
            isRotationLock = true;

            if (playerTransform.localScale.y < 0)
            {
                StartFallDownMovement();
                OnStickStartFallDown();
                OnPlayerEndHorizontalMovement();
            }
        }
    }

    #endregion


    #region Event handlers

    private void Player_OnStickFallHorizontal(float stickSize)
    {
        float endOfPlatformDistance = PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        bool isFall = (stickSize < PlatformManager.NewDistance) || stickSize > endOfPlatformDistance;
        MovementState = isFall ? PlayerMovementState.HorizontalBeforeFallDown : PlayerMovementState.HorizontalBeforeReturn;

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
        StartMove(targetPosition, movementTime, MovementState);
        isRotationLock = false;

        animator.SetBool("isRun", true);
    }


    private void Player_OnReloadGame()
    {
        if (playerTransform.localScale.y < 0)
        {
            RotatePlayer();
        }
        StartTwoPointMove(StartPosition, PlatformManager.PLATFORM_MOVING_TIME);
    }

    #endregion


    #region Private methods

    private void StartTwoPointMove(Vector2 targetPosition, float movementTime)
    {
        this.targetMovementPosition = targetPosition;
        this.movementTime = movementTime;

        statrMovementPosition = playerTransform.position;
        startMovingTime = Time.realtimeSinceStartup;
        fractionCoefficient = MathConsts.MIN_FRACTION_COEFFICIENT;
        MovementState = PlayerMovementState.TwoPoint;
        isMoving = true;
    }


    private void StartMove(Vector2 targetPosition, float movementTime, PlayerMovementState movementState)
    {
        StartTwoPointMove(targetPosition, movementTime);

        this.MovementState = movementState;
    }


    private void StartFallDownMovement()
    {
        targetMovementPosition = playerTransform.position;
        targetMovementPosition.y -= playerFallDistance;
        float movementTime = playerFallDistance / playerFallSpeed;
        StartMove(targetMovementPosition, movementTime, PlayerMovementState.FallDown);
    }


    private void RotatePlayer()
    {
        Vector2 newScale = playerTransform.localScale;
        newScale.y = playerTransform.localScale.y * MathConsts.NEGATIVE_COEFFICIENT;
        playerTransform.localScale = newScale;

        Vector2 targetPosition = playerTransform.position;
        targetPosition.y += newScale.y * rotationalPositionOffset;
        statrMovementPosition.y = targetPosition.y;
        targetMovementPosition.y = targetPosition.y;
        playerTransform.position = targetPosition;
    }

    #endregion
}
