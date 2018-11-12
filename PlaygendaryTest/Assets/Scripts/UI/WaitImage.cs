using System.Collections;
using UnityEngine;

public class WaitImage : MonoBehaviour
{
    private bool isEventSigned = false;
    private bool isReloadGameCome = false;
    private float startTimeOfReload;
    private float pastTimeOfReload;


    #region Unity lifecycle

    private void OnEnable()
    {
        if (!isEventSigned)
        {
            EndMenu.OnReloadGame += WaitImage_OnReloadGame;
            StartupController.OnSceneLoad += WaitImage_OnSceneLoad;

            isEventSigned = true;
        }
    }


    private void OnDisable()
    {
        StartupController.OnSceneLoad -= WaitImage_OnSceneLoad;
    }


    private void Update()
    {
        if (isReloadGameCome)
        {
            pastTimeOfReload = Time.realtimeSinceStartup - startTimeOfReload;

            if (pastTimeOfReload > PlatformManager.PLATFORM_MOVING_TIME)
            {
                gameObject.SetActive(false);

                isReloadGameCome = false;
            }
        }
    }

    #endregion


    #region Event handlers

    private void WaitImage_OnReloadGame()
    {
        gameObject.SetActive(true);

        startTimeOfReload = Time.realtimeSinceStartup;
        isReloadGameCome = true;
    }


    private void WaitImage_OnSceneLoad()
    {
        gameObject.SetActive(false);
    }

    #endregion

}
