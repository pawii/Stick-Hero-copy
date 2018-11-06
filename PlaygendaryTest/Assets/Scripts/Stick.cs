using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stick : PartOfPlatform
{
    public static event Action<float> OnStickFall;


    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private AudioSource audio;


    private Vector3 rotation = new Vector3(0, 0, 0);


    #region Unity lifecycle

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

            StartCoroutine(StickFalling());
        }
	}

    #endregion


    #region Private methods

    private IEnumerator StickFalling()
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

        OnStickFall(transform.localScale.y);
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
