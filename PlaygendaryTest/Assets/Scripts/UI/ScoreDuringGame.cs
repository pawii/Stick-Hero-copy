using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDuringGame : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private AudioSource audio;


    private int score;


    #region Unity lifecycle

    private void Awake()
    {
        scoreText.text = "0";

        Player.OnScoreUp += ScoreDuringGame_OnScoreUp;
        Player.OnEndGame += ScoreDuringGame_OnEndGame;
        EndMenu.OnReloadGame += ScoreDuringGame_OnReloadGame;
        StartMenu.OnStartGame += ScoreDuringGame_OnStartGame;

        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        Player.OnScoreUp -= ScoreDuringGame_OnScoreUp;
        Player.OnEndGame -= ScoreDuringGame_OnEndGame;
        EndMenu.OnReloadGame -= ScoreDuringGame_OnReloadGame;
        StartMenu.OnStartGame -= ScoreDuringGame_OnStartGame;
    }

    #endregion


    #region Event handlers

    private void ScoreDuringGame_OnScoreUp()
    {
        score++;

        scoreText.text = score.ToString();

        audio.Play();
    }


    private void ScoreDuringGame_OnEndGame()
    {
        gameObject.SetActive(false);
    }


    private void ScoreDuringGame_OnReloadGame()
    {
        score = 0;
        scoreText.text = "0";
        gameObject.SetActive(true);
    }


    private void ScoreDuringGame_OnStartGame()
    {
        gameObject.SetActive(true);
    }

    #endregion
}
