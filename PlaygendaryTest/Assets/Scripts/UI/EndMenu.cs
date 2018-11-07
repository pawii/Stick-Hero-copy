using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EndMenu : MonoBehaviour
{
    [SerializeField]
    private Text countScoreText;
    [SerializeField]
    private Text countBestScoreText;


    public static event Action OnReloadGame;


    #region Unity lifecycle

    private void Awake()
    {
        Player.OnEndGame += EndMenu_OnEndGame;

        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        Player.OnEndGame -= EndMenu_OnEndGame;
    }

    #endregion


    #region Event handlers

    private void EndMenu_OnEndGame()
    {
        countScoreText.text = ScoreManager.CurrentScore.ToString();
        countBestScoreText.text = ScoreManager.BestScore.ToString();
        gameObject.SetActive(true);
    }


    public void ReloadButton_OnClick()
    {
        OnReloadGame();
        gameObject.SetActive(false);
    }

    #endregion
}