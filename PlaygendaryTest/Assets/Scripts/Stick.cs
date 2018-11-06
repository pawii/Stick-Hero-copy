using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stick : PartOfPlatform
{
    public static event Action<float> OnStickFallHorizontal;
    //public static event Action<float> OnEndGame;


    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private AudioSource audio;


    private Vector3 rotation = new Vector3(0, 0, 0);


    #region Unity lifecycle

    private new void Awake()
    {
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
        if (Input.GetMouseButtonDown(0) && state == States.Center)
        {
            audio.Play();
        }

        if (Input.GetMouseButton(0) && state == States.Center)
        {
            Vector2 scale = transform.localScale;
            scale.y += growSpeed * Time.deltaTime;
            transform.localScale = scale;
        }

        if (Input.GetMouseButtonUp(0) && state == States.Center)
        {
            audio.Stop();

            StartCoroutine(StickFallingHorizontal());
        }
	}

    #endregion


    #region Private methods

    private IEnumerator StickFallingDown()
    {
        while (rotation.z != -180)
        {
            rotation.z -= rotationSpeed * Time.deltaTime;

            if (rotation.z < -180)
            {
                rotation.z = -180;
            }

            transform.localEulerAngles = rotation;

            yield return null;
        }

        //OnEndGame;
    }


    private IEnumerator StickFallingHorizontal()
    {
        while (rotation.z != -90)
        {
            rotation.z -= rotationSpeed * Time.deltaTime;

            if (rotation.z < -90)
            {
                rotation.z = -90;
            }

            transform.localEulerAngles = rotation;

            yield return null;
        }

        OnStickFallHorizontal(transform.localScale.y);
    }

    #endregion

    #region Event handlers

    private void Stick_OnStickFallDown()
    {
        if (state == States.Center)
        {
            StartCoroutine(StickFallingDown());
        }
    }

    #endregion


    protected override void HandleOnMovePlatform()
    {
        if (state == States.Behind)
        {
            transform.localScale = new Vector2(1, 0);

            rotation.z = 0;
            transform.localEulerAngles = rotation;
        }
        else if (state == States.Front)
        {
            Vector2 newPos = Vector2.zero;
            newPos.x += PlatformManager.FontPlatformWidth / 2f;
            transform.localPosition = newPos;
        }
    }
}
