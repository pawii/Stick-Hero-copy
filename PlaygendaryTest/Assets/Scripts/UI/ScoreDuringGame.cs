using UnityEngine;
using UnityEngine.UI;

public class ScoreDuringGame : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private AudioSource scoreUpAudio;


    private bool isEventSigned = false;
    private int currentScore;


    #region Unity lifecycle

    private void OnEnable()
    {
        if (!isEventSigned)
        {
            scoreText.text = "0";

            Player.OnScoreUp += ScoreDuringGame_OnScoreUp;
            Player.OnEndGame += ScoreDuringGame_OnEndGame;
            EndMenu.OnReloadGame += ScoreDuringGame_OnReloadGame;
            StartMenu.OnStartGame += ScoreDuringGame_OnStartGame;

            gameObject.SetActive(false);

            isEventSigned = true;
        }
    }

    #endregion


    #region Event handlers

    private void ScoreDuringGame_OnScoreUp()
    {
        currentScore++;
        scoreText.text = currentScore.ToString();

        scoreUpAudio.Play();
    }


    private void ScoreDuringGame_OnEndGame()
    {
        gameObject.SetActive(false);
    }


    private void ScoreDuringGame_OnReloadGame()
    {
        currentScore = 0;
        scoreText.text = "0";

        gameObject.SetActive(true);
    }


    private void ScoreDuringGame_OnStartGame()
    {
        gameObject.SetActive(true);
    }

    #endregion
}
