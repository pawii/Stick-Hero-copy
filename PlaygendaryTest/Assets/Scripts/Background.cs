using UnityEngine;

public class Background : MonoBehaviour
{
    private const int START_POSITION = 0;


    [SerializeField]
    private float movingSpeed;
    [SerializeField]
    private float width;
    [SerializeField]
    private Transform backgroundTransform;


    private Vector2 currentPosition;
    private bool isMoving;


    #region Unity lifecycle

    private void OnEnable()
    {
        currentPosition = transform.position;
        isMoving = false;

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
            currentPosition.x -= movingSpeed * Time.deltaTime;
            if (currentPosition.x < -width)
            {
                currentPosition.x += width;
            }

            backgroundTransform.localPosition = currentPosition;
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