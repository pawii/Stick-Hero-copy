using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnMoveNext;
    public static event Action OnStickFallDown;
    public static event Action OnScoreUp;


    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerFallSpeed;


    public static Vector2 StartPos { get; private set; }


    private Vector2 playerPos;


    #region Unity lifecycle

    private void Awake()
    {
        StartPos = transform.position;
        playerPos = StartPos;

        Stick.OnStickFallHorizontal += Player_OnStickFallHorizontal;
    }


    private void OnDestroy()
    {
        Stick.OnStickFallHorizontal -= Player_OnStickFallHorizontal;
    }

    #endregion


    #region Private methods

    private IEnumerator PlayerMovement(float targetPos, bool isFall)
    {
        anim.SetBool("isRun", true);

        while(playerPos.x != targetPos)
        {
            playerPos.x += playerSpeed * Time.deltaTime;

            if(playerPos.x > targetPos)
            {
                playerPos.x = targetPos;
            }

            transform.position = playerPos;

            yield return null;
        }

        if (!isFall)
        {
            playerPos = StartPos;
            StartCoroutine(MoveBackForTime());

            OnScoreUp();
            OnMoveNext();
        }
        else
        {
            OnStickFallDown();
            StartCoroutine(PlayerFall());
        }

        anim.SetBool("isRun", false);
    }


    private IEnumerator PlayerFall()
    {
        float targetPos = playerPos.y - 10;
        while (playerPos.y != targetPos)
        {
            playerPos.y -= playerFallSpeed * Time.deltaTime;

            if (playerPos.y < targetPos)
            {
                playerPos.y = targetPos;
            }

            transform.position = playerPos;

            yield return null;
        }
    }


    // ДУБЛИРОВАНИЕ КОДА
    private IEnumerator MoveBackForTime()
    {
        Vector2 beginMovementPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;

        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / PlatformManager.MOVE_TIME);
            transform.position = Vector2.Lerp(beginMovementPosition, StartPos, fraction);
            yield return null;
        }
    }
    // ДУБЛИРОВАНИЕ КОДА

    #endregion


    #region Event handlers
        
    private void Player_OnStickFallHorizontal(float stickSize)
    {
        float endOfPlatform = PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        bool isFall = stickSize < PlatformManager.NewDistance || stickSize > endOfPlatform;

        if (isFall)
        {
            float targetPos = StartPos.x + stickSize;
            StartCoroutine(PlayerMovement(targetPos, isFall));
        }
        else
        {
            float targetPos = StartPos.x + PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
            StartCoroutine(PlayerMovement(targetPos, isFall));

            if ()
        }
    }

    #endregion
}
