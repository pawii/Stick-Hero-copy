using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;

    private Vector2 playerPos;

    // НЕ КОНСТАНТА (ОФСЕТ НА КРАЙ)
    private const float offsetPos = 1;


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
        StartCoroutine(PlayerMovement(stickSize + offsetPos));
    }


    private IEnumerator PlayerMovement(float targetPos)
    {
        // проверить производительность с локальной переменной playerPos
        playerPos = transform.position;

        while(playerPos.x != targetPos)
        {
            playerPos.x += playerSpeed * Time.deltaTime;

            if(playerPos.x > targetPos)
            {
                playerPos.x = targetPos;
            }

            transform.position = playerPos;

            yield return new WaitForFixedUpdate();
        }
    }
}
