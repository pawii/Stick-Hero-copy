using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float width;


    private Vector2 position;
    private bool isMove;


    #region Unity lifecycle

    private void Awake()
    {
        position = transform.position;
        isMove = false;

        Player.OnPlayerStartHorizontalMovement += Background_OnPlayerStartHorizontalMovement;
        Player.OnPlayerEndHorizontalMovement += Background_OnPlayerEndHorizontalMovement;
    }


    private void OnDestroy()
    {
        Player.OnPlayerStartHorizontalMovement -= Background_OnPlayerStartHorizontalMovement;
        Player.OnPlayerEndHorizontalMovement -= Background_OnPlayerEndHorizontalMovement;
    }


    private void Update()
    {
        if (isMove)
        {
            position.x -= moveSpeed * Time.deltaTime;
            if (position.x < -width)
            {
                position.x = width;
            }

            transform.localPosition = position;
        }
    }

    #endregion


    #region Event handlers

    private void Background_OnPlayerStartHorizontalMovement()
    {
        isMove = true;
    }


    private void Background_OnPlayerEndHorizontalMovement()
    {
        isMove = false;
    }

    #endregion
}
