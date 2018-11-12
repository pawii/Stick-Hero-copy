using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    [SerializeField]
    private float probabilityOfAppear;
    [SerializeField]
    private Transform cherryTransform;
    [SerializeField]
    private float OffsetFromThePlatform;


    private float positionY;
    private bool isEventSigned = false;


    #region Unity lifecycle

    private void OnEnable()
    {
        if (!isEventSigned)
        {
            positionY = cherryTransform.position.y;

            Platform.OnPlatformEndMovement += Cherry_OnPlatformEndMovement;
            PlatformManager.OnMovePlatform += Cherry_OnMovePlatform;

            isEventSigned = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }

    #endregion


    #region Event handlers

    private void Cherry_OnMovePlatform()
    {
        gameObject.SetActive(false);
    }


    private void Cherry_OnPlatformEndMovement()
    {
        bool isAppears = Random.value <= probabilityOfAppear;
        if (isAppears)
        {
            float minPositionX = Player.StartPosition.x;
            minPositionX += OffsetFromThePlatform;

            float maxPositionX = minPositionX + PlatformManager.NewDistance;
            maxPositionX -= OffsetFromThePlatform;

            float positionX = Random.Range(minPositionX, maxPositionX);

            cherryTransform.position = new Vector2(positionX, positionY);

            gameObject.SetActive(true);
        }
    }

    #endregion
}
