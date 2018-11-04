using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private Vector2 startPos;

    private Vector2 playerPos;

    // НЕ КОНСТАНТА (ОФСЕТ НА КРАЙ)
    private const float offsetPos = 1;

    public static event Action MoveNext;

    private void Awake()
    {
        Stick.StickFall += StartMovement;
    }


    private void OnDestroy()
    {
        Stick.StickFall -= StartMovement;
    }


    private void StartMovement(float stickSize)
    {
        StartCoroutine(PlayerMovement(stickSize + offsetPos + startPos.x));
    }


    private IEnumerator PlayerMovement(float targetPos)
    {
        // проверить производительность с локальной переменной playerPos

        while(playerPos.x != targetPos)
        {
            playerPos.x += playerSpeed * Time.deltaTime;

            if(playerPos.x > targetPos)
            {
                playerPos.x = targetPos;
            }

            transform.position = playerPos;

            yield return null;//new WaitForFixedUpdate();
        }

        MoveNext();
        transform.position = startPos;
        playerPos = startPos;
    }
}
