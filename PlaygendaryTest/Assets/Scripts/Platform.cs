using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private States startState;


    public IPlatformState State { get; set; }


    #region Unity lifecycle

    private void Awake()
    {
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

    #endregion


    #region Event handlers

    private void Platform_OnMovePlatform()
    {
        Vector2 targetPos = State.MovePlatform(this);
        StartCoroutine(MoveForTime(targetPos));
    }

    #endregion


    private IEnumerator MoveForTime(Vector2 targetPos)
    {
        Vector2 startPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;

        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / PlatformManager.MOVE_TIME);
            transform.position = Vector2.Lerp(startPosition, targetPos, fraction);
            yield return null;
        }
    }
}
