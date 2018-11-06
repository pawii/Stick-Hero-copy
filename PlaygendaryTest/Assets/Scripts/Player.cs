using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnMoveNext;


    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]


    public static Vector2 StartPos { get; private set; }
    private Vector2 playerPos;


    #region Unity lifecycle

    private void Awake()
    {
        StartPos = new Vector2(-4, 0.42f);
        playerPos = StartPos;

        Stick.OnStickFall += Player_OnStickFall;
    }


    private void OnDestroy()
    {
        Stick.OnStickFall -= Player_OnStickFall;
    }

    #endregion


    #region Private methods

    private IEnumerator PlayerMovement(float targetPos)
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

        playerPos = StartPos;
        StartCoroutine(MoveForTime(StartPos));

        anim.SetBool("isRun", false);

        OnMoveNext();
    }


    // ДУБЛИРОВАНИЕ КОДА
    private IEnumerator MoveForTime(Vector2 targetPos)
    {
        Vector2 startPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;

        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / PlatformManager.MOVE_TIME);
            transform.position = Vector2.Lerp(startPosition, targetPos, fraction);
            yield return null;
        }
    }
    // ДУБЛИРОВАНИЕ КОДА

    #endregion


    #region Event handlers

    private void Player_OnStickFall(float stickSize)
    {
        float targetPos = StartPos.x + PlatformManager.NewDistance + PlatformManager.BehindPlatformWidth;
        StartCoroutine(PlayerMovement(targetPos));
    }

    #endregion
}
