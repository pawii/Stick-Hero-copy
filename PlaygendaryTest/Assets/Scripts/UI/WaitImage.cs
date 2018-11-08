using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitImage : MonoBehaviour
{
    #region Unity lifecycle

    private void Awake()
    {
        EndMenu.OnReloadGame += WaitImage_OnReloadGame;

        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        EndMenu.OnReloadGame -= WaitImage_OnReloadGame;
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
        yield return new WaitForSeconds(PlatformManager.MOVE_TIME);
        gameObject.SetActive(false);
    }

    #endregion
}
