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


    private bool isRotate;
    private Vector3 currentRotation;
    private Vector3 endRotation;
    private Vector3 scale;


    #region Unity lifecycle

    private new void Awake()
    {
        scale = Vector2.right;

        base.Awake();
        Player.OnStickFallDown += Stick_OnStickFallDown;
    }


    private new void OnDestroy()
    {
        base.OnDestroy();
        Player.OnStickFallDown += Stick_OnStickFallDown;
    }


    private void Update ()
    {
        if (state == States.Center)
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

    #endregion


    protected override void HandleOnMovePlatform()
    {
        if (state == States.Behind)
        {
            scale = Vector2.right;
            transform.localScale = scale;

            currentRotation.z = 0;
            transform.localEulerAngles = currentRotation;
        }
        else if (state == States.Front)
        {
            Vector2 newPos = Vector2.zero;
            newPos.x += PlatformManager.FontPlatformWidth / 2f;
            transform.localPosition = newPos;
        }
    }
}
