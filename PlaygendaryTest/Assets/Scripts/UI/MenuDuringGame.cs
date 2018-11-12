using UnityEngine;
using UnityEngine.UI;

public class MenuDuringGame : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private AudioSource scoreUpAudio;
    [SerializeField]
    private Text cherryText;


    private bool isEventSigned = false;
    private int currentScore;
    private int currentCountCherry;


    #region Unity lifecycle

    private void OnEnable()
    {
        if (!isEventSigned)
        {
            Player.OnScoreUp += ScoreDuringGame_OnScoreUp;
            Player.OnCherryUp += ScoreDuringGame_OnCherryUp;
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
        scoreText.text = "0";
        currentCountCherry = ScoreManager.CountCherry;
        cherryText.text = currentCountCherry.ToString();
        gameObject.SetActive(true);
    }


    private void ScoreDuringGame_OnCherryUp()
    {
        currentCountCherry++;
        cherryText.text = currentCountCherry.ToString();
    }

    #endregion
}
