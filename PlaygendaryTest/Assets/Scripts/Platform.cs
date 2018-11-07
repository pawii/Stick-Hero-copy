using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private States startState;


    public IPlatformState State { get; set; }


    private bool isMove;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float startTime;
    private float fraction;


    #region Unity lifecycle

    private void Awake()
    {
        isMove = false;

        switch (startState)
        {
            case States.Behind:
                State = new BehindPlatform();
                break;
            case States.Center:
                State = new CenterPlatform();
                break;
            case States.Front:
                State = new FrontPlatform();
                break;
        }

        PlatformManager.OnMovePlatform += Platform_OnMovePlatform;
    }


    private void OnDestroy()
    {
        PlatformManager.OnMovePlatform -= Platform_OnMovePlatform;
    }


    private void Update()
    {
        if (isMove)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / PlatformManager.MOVE_TIME);
            if (fraction > 1f)
            {
                fraction = 1;
                isMove = false;
            }

            transform.position = Vector2.Lerp(startPosition, targetPosition, fraction);
        }
    }

    #endregion


    #region Event handlers

    private void Platform_OnMovePlatform()
    {
        targetPosition = State.MovePlatform(this);
        startPosition = transform.position;
        startTime = Time.realtimeSinceStartup;
        fraction = 0f;
        isMove = true;
    }

    #endregion
}
