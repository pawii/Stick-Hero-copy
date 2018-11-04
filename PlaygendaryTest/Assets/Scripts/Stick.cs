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

    private Vector3 rotation = new Vector3(0, 0, 0);

    public static event Action<float> StickFall;


	private void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audio.Play();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 scale = transform.localScale;
            scale.y += growSpeed * Time.deltaTime;
            transform.localScale = scale;
        }

        if (Input.GetMouseButtonUp(0))
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

            yield return new WaitForFixedUpdate();
        }

        rotation.z = 0;

        StickFall(transform.localScale.y);
    }

}
