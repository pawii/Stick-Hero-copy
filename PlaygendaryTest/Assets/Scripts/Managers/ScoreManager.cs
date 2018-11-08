using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static uint CurrentScore { get; private set; }
    public static uint BestScore // Не автоматическое свойство, потому что сериализуется
    {
        get
        {
            return bestScore;
        }
        private set
        {
            bestScore = value;
        }
    }


    private string filename;
    private static uint bestScore;


    #region Unity lifecycle

    private void Awake()
    {
        filename = Path.Combine(Application.persistentDataPath, "game.dat");

        Player.OnScoreUp += ScoreManager_OnScoreUp;
        Player.OnEndGame += ScoreManager_OnEndGame;
        EndMenu.OnReloadGame += ScoreManager_OnReloadGame;

        UnloadScore();
    }


    private void OnDestroy()
    {
        Player.OnScoreUp -= ScoreManager_OnScoreUp;
        Player.OnEndGame -= ScoreManager_OnEndGame;
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
        }
    }


    private void ScoreManager_OnEndGame()
    {
        if (CurrentScore == BestScore)
        {
            SaveScore();
        }
    }


    private void ScoreManager_OnReloadGame()
    {
        CurrentScore = 0;
    }

    #endregion


    #region Private methods

    private void SaveScore()
    {
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, bestScore);
        stream.Close();
    }


    private void UnloadScore()
    {
        if (!File.Exists(filename))
        {
            bestScore = 0;
            return;
        }
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        bestScore = (uint)formatter.Deserialize(stream);
        stream.Close();
    }

    #endregion
}
