using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    private const string NAME_SCORE_VARIABLE = "BestScore";
    private const string NAME_CHERRY_VARIABLE = "CountCherry";


    public static event Action OnScoresUnload;


    public static int CurrentScore { get; private set; }
    public static int BestScore // Not authomatical property, because serialized (Richter)
    {
        get
        {
            return _bestScore;
        }
        private set
        {
            _bestScore = value;
        }
    }
    public static int CountCherry // Not authomatical property, because serialized (Richter)
    {
        get
        {
            return _countCherry;
        }
        private set
        {
            _countCherry = value;
        }
    }


    private static int _bestScore;
    private static int _countCherry;


    #region Unity lifecycle

    private void OnEnable()
    {
        Player.OnScoreUp += ScoreManager_OnScoreUp;
        Player.OnCherryUp += ScoreManager_OnCherryUp;
        EndMenu.OnReloadGame += ScoreManager_OnReloadGame;

        UnloadScore();

        OnScoresUnload();
    }


    private void OnDisable()
    {
        Player.OnScoreUp -= ScoreManager_OnScoreUp;
        EndMenu.OnReloadGame -= ScoreManager_OnReloadGame;
    }

    #endregion


    #region Event handlers

    private void ScoreManager_OnScoreUp()
    {
        CurrentScore ++;
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
            SaveScore();
        }
    }


    private void ScoreManager_OnCherryUp()
    {
        CountCherry ++;
        SaveScore();
    }


    private void ScoreManager_OnReloadGame()
    {
        CurrentScore = 0;
    }

    #endregion


    #region Private methods

    private void SaveScore()
    {
        if (CurrentScore == BestScore)
        {
            PlayerPrefs.SetInt(NAME_SCORE_VARIABLE, BestScore);
        }
        PlayerPrefs.SetInt(NAME_CHERRY_VARIABLE, CountCherry);
    }


    private void UnloadScore()
    {
        BestScore = PlayerPrefs.GetInt(NAME_SCORE_VARIABLE, 0);
        CountCherry = PlayerPrefs.GetInt(NAME_CHERRY_VARIABLE, 0);
    }

    #endregion
}
