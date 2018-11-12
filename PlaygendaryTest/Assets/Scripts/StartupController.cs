using System;
using UnityEngine;

public class StartupController : MonoBehaviour
{
    private const string SCENE_PREFAB_NAME = "MainScene";


    public static event Action OnSceneLoad;


    #region Unity lifecycle

    private void OnEnable()
    {
        ScoreManager.OnScoresUnload += StartupController_OnScoresUnload;
    }


    private void OnDisable()
    {
        ScoreManager.OnScoresUnload -= StartupController_OnScoresUnload;
    }

    #endregion


    #region Event handlers

    private void StartupController_OnScoresUnload()
    {
        GameObject scenePrefab = Resources.Load(SCENE_PREFAB_NAME) as GameObject;
        if (scenePrefab != null)
        {
            Instantiate(scenePrefab);
        }
        OnSceneLoad();
    }

    #endregion
}
