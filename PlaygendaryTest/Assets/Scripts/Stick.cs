using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stick : PartOfPlatform
{
    private const int FALL_DOWN_ANGLE = -180;
    private const int FALL_HORIZONTAL_ANGLE = -90;


    public static event Action<float> OnStickFallHorizontal;


    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private AudioSource audio;


    private bool isLock;
    private bool isRotate;
    private Vector3 currentRotation;
    private Vector3 endRotation;
    private Vector3 scale;


    #region Unity lifecycle

    private new void Awake()
    {
        scale = Vector2.right;
        isLock = false;

        base.Awake();
        Player.OnStickFallDown += Stick_OnStickFallDown;
        Player.OnPlayerStartHorizontalMovement += Stick_OnPlayerStartHorizontalMovement;
        Platform.OnPlatformEndMovement += Stick_OnPlatformEndMovement;
        Player.OnEndGame += Stick_OnEndGame;
        EndMenu.OnReloadGame += Stick_OnReloadGame;
    }


    private new void OnDestroy()
    {
        base.OnDestroy();
        Player.OnStickFallDown += Stick_OnStickFallDown;
        Player.OnPlayerStartHorizontalMovement -= Stick_OnPlayerStartHorizontalMovement;
        Platform.OnPlatformEndMovement -= Stick_OnPlatformEndMovement;
        Player.OnEndGame -= Stick_OnEndGame;
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
                    audio.Play();
                }

                if (Input.GetMouseButton(0))
                {
                    scale.y += growSpeed * Time.deltaTime;
                    transform.localScale = scale;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    audio.Stop();

                    endRotation.z = FALL_HORIZONTAL_ANGLE;
                    isRotate = true;
                }
            }

            if (isRotate)
            {
                currentRotation.z -= rotationSpeed * Time.deltaTime;

                if (currentRotation.z < endRotation.z)
                {
                    currentRotation.z = endRotation.z;
                    isRotate = false;

                    if(currentRotation.z == FALL_HORIZONTAL_ANGLE)
                    {
                        OnStickFallHorizontal(transform.localScale.y);
                    }
                }

                transform.localEulerAngles = currentRotation;
            }
        }
	}

    #endregion


    #region Event handlers

    private void Stick_OnStickFallDown()
    {
        if (state == States.Center)
        {
            endRotation.z = FALL_DOWN_ANGLE;
            isRotate = true;
        }
    }


    private void Stick_OnPlayerStartHorizontalMovement()
    {
        isLock = true;
    }


    private void Stick_OnPlatformEndMovement()
    {
        isLock = false;
    }


    private void Stick_OnEndGame()
    {
        isLock = true;
    }


    private void Stick_OnReloadGame()
    {
        isLock = false;
        
        RefreshStick();
    }

    #endregion


    #region Private methods

    private void RefreshStick()
    {
        isRotate = false;

        scale = Vector2.right;
        transform.localScale = scale;

        currentRotation.z = 0;
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
            Vector2 newPos = Vector2.zero;
            newPos.x += PlatformManager.FontPlatformWidth / 2f;
            transform.localPosition = newPos;
        }
    }
}
