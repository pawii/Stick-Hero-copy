using UnityEngine;

public class Background : MonoBehaviour
{
    private static float Width;


    [SerializeField]
    private float movingSpeed;


    private Vector2 position;
    private bool isMoving;


    #region Unity lifecycle

    private void OnEnable()
    {
        position = transform.position;
        isMoving = false;
        Width = transform.localScale.x;

        Player.OnPlayerStartHorizontalMovement += Background_OnPlayerStartHorizontalMovement;
        Player.OnPlayerEndHorizontalMovement += Background_OnPlayerEndHorizontalMovement;
    }


    private void OnDisable()
    {
        Player.OnPlayerStartHorizontalMovement -= Background_OnPlayerStartHorizontalMovement;
        Player.OnPlayerEndHorizontalMovement -= Background_OnPlayerEndHorizontalMovement;
    }


    private void Update()
    {
        if (isMoving)
        {
            position.x -= movingSpeed * Time.deltaTime;
            if (position.x < -Width)
            {
                position.x = Width;
            }

            transform.localPosition = position;
        }
    }

    #endregion


    #region Event handlers

    private void Background_OnPlayerStartHorizontalMovement()
    {
        isMoving = true;
    }


    private void Background_OnPlayerEndHorizontalMovement()
    {
        isMoving = false;
    }

    #endregion
}