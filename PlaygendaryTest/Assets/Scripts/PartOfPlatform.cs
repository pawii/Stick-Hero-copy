using UnityEngine;

public abstract class PartOfPlatform : MonoBehaviour
{
    [SerializeField]
    protected States state;


    private bool isEventSigned = false;
    private const int MIN_STATE = 1;
    private const int MAX_STATE = 3;
    private const int CHANGE_STEP = 1;


    #region Unity lifecycle

    protected void OnEnable()
    {
        if (!isEventSigned)
        {
            PlatformManager.OnMovePlatform += PartOfPlatform_OnMovePlatform;
            isEventSigned = true;
        }
    }

    #endregion


    #region Event handlers

    private void PartOfPlatform_OnMovePlatform()
    {
        HandleOnMovePlatform();

        int newState = (int)state - CHANGE_STEP;
        if (newState < MIN_STATE)
        {
            newState = MAX_STATE;
        }
        state = (States)newState;
    }

    #endregion


    protected abstract void HandleOnMovePlatform();
}
