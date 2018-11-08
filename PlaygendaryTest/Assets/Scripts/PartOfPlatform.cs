using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartOfPlatform : MonoBehaviour
{
    [SerializeField]
    protected States state;


    #region Unity lifecycle

    protected void Awake()
    {
        PlatformManager.OnMovePlatform += PartOfPlatform_OnMovePlatform;
    }


    protected void OnDestroy()
    {
        PlatformManager.OnMovePlatform -= PartOfPlatform_OnMovePlatform;
    }

    #endregion


    #region Event handlers
    
    private void PartOfPlatform_OnMovePlatform()
    {
        HandleOnMovePlatform();

        int newState = (int)state - 1;
        if (newState == 0)
        {
            newState = 3;
        }
        state = (States)newState;
    }

    #endregion


    protected abstract void HandleOnMovePlatform();
}
