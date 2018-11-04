using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private States startState;

    public IPlatformState State { get; set; }


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

        PlatformManager.MovePlatform += OnMovePlatform;
    }


    private void OnDestroy()
    {
        PlatformManager.MovePlatform -= OnMovePlatform;
    }


    private void OnMovePlatform(float oldDistance, float newDistance)
    {
        //oldDistance = newDistance;
        //float returnDistance = State.OnMoveNext(this, oldDistance, minDistance, maxDistance);
        //if(returnDistance != oldDistance)
        //{
        //    newDistance = returnDistance;
        //}
        //Debug.Log("oldDistance: " + oldDistance);
        //Debug.Log("newDistance: " + newDistance);
        State.MovePlatform(this, oldDistance, newDistance);
    }
}
