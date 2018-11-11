using UnityEngine;
using UnityEngine.UI;
using System;

public class EndMenu : MonoBehaviour
{
    public static event Action OnReloadGame;


    [SerializeField]
    private Text countScoreText;
    [SerializeField]
    private Text countBestScoreText;


    private bool isEventSigned = false;


    #region Unity lifecycle

    private void OnEnable()
    {
        if (!isEventSigned)
        {
            Player.OnEndGame += EndMenu_OnEndGame;
            isEventSigned = true;

            gameObject.SetActive(false);
        }
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