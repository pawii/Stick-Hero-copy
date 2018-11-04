using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stick : MonoBehaviour
{
    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private States state;

    private Vector3 rotation = new Vector3(0, 0, 0);

    public static event Action<float> StickFall;


    private void Awake()
    {
        Player.MoveNext += OnMoveNext;
    }


    private void OnDestroy()
    {
        Player.MoveNext -= OnMoveNext;
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

            StartCoroutine(StickFalling());
        }
	}


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

            yield return null;//new WaitForFixedUpdate();
        }

        StickFall(transform.localScale.y);
    }


    private void OnMoveNext()
    {
        int newState = (int)state - 1;
        if(newState == 0)
        {
            newState = 3;
        }
        state = (States)newState;

        if (state == States.Front)
        {
            rotation.z = 0;
            transform.localEulerAngles = rotation;
            transform.localScale = new Vector2(1, 0);
        }
    }
}
