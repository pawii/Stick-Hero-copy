using System.Collections;
using UnityEngine;

public class WaitImage : MonoBehaviour
{
    private bool isEventSigned = false;


    #region Unity lifecycle

    private void OnEnable()
    {
        if (!isEventSigned)
        {
            EndMenu.OnReloadGame += WaitImage_OnReloadGame;

            gameObject.SetActive(false);

            isEventSigned = true;
        }
    }

    #endregion


    #region Event handlers

    private void WaitImage_OnReloadGame()
    {
        gameObject.SetActive(true);
        StartCoroutine(Wait());
    }

    #endregion
    

    #region Private methods

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(PlatformManager.PLATFORM_MOVING_TIME);
        gameObject.SetActive(false);
    }

    #endregion
}
