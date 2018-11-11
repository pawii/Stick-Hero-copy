using UnityEngine;
using System;

public class Stick : PartOfPlatform
{
    private const int FALL_DOWN_END_ANGLE = -180;
    private const int FALL_HORIZONTAL_END_ANGLE = -90;
    private const int START_ROTATION = 0;


    public static event Action<float> OnStickFellHorizontal;


    private static bool isLock;


    #region Fields

    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private AudioSource growStickAudio;


    private bool isRotating;
    private Vector3 currentRotation;
    private Vector3 endRotation;
    private Vector3 scale;

    #endregion


    #region Unity lifecycle

    private new void OnEnable()
    {
        scale = Vector2.right;
        isLock = true;

        base.OnEnable();
        Player.OnStickStartFallDown += Stick_OnStickStartFallDown;
        Platform.OnPlatformEndMovement += Stick_OnPlatformEndMovement;
        EndMenu.OnReloadGame += Stick_OnReloadGame;
    }


    private void OnDisable()
    {
        Player.OnStickStartFallDown -= Stick_OnStickStartFallDown;
        Platform.OnPlatformEndMovement -= Stick_OnPlatformEndMovement;
        EndMenu.OnReloadGame -= Stick_OnReloadGame;
    }


    private void Update ()
    {
        if (state == States.Center)
        {
            if (!isLock)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    growStickAudio.Play();
                }

                if (Input.GetMouseButton(0))
                {
                    scale.y += growSpeed * Time.deltaTime;

                    if (scale.y > maxSize)
                    {
                        return;
                    }
                    else
                    {
                        transform.localScale = scale;
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    growStickAudio.Stop();

                    endRotation.z = FALL_HORIZONTAL_END_ANGLE;
                    isRotating = true;
                    isLock = true;
                }
            }

            if (isRotating)
            {
                currentRotation.z -= rotationSpeed * Time.deltaTime;

                if (currentRotation.z < endRotation.z)
                {
                    currentRotation.z = endRotation.z;
                    isRotating = false;

                    if(currentRotation.z == FALL_HORIZONTAL_END_ANGLE)
                    {
                        OnStickFellHorizontal(transform.localScale.y);
                    }
                }

                transform.localEulerAngles = currentRotation;
            }
        }
	}

    #endregion


    #region Event handlers

    private void Stick_OnStickStartFallDown()
    {
        if (state == States.Center)
        {
            endRotation.z = FALL_DOWN_END_ANGLE;
            isRotating = true;
        }
    }


    private void Stick_OnPlatformEndMovement()
    {
        isLock = false;
    }


    private void Stick_OnReloadGame()
    {
        RefreshStick();
    }

    #endregion


    #region Private methods

    private void RefreshStick()
    {
        isRotating = false;

        scale = Vector2.right;
        transform.localScale = scale;

        currentRotation.z = START_ROTATION;
        transform.localEulerAngles = currentRotation;
    }

    #endregion


    protected override void HandleOnMovePlatform()
    {
        if (state == States.Behind)
        {
            RefreshStick();
        }
        else if (state == States.Front)
        {
            Vector2 targetPosition = Vector2.zero;
            targetPosition.x += PlatformManager.FontPlatformWidth * MathConsts.HALF_COEFFICIENT;
            transform.localPosition = targetPosition;
        }
    }
}
