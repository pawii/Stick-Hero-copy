using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public static event Action OnStartGame;


    [SerializeField]
    private Text soundButtonText;


    private bool isAudioPaused;


    #region Unity lifecycle

    private void Awake()
    {
        isAudioPaused = false;
        soundButtonText.text = "Sound on";
    }

    #endregion


    #region Event handlers

    public void PlayButton_OnClick()
    {
        OnStartGame();

        gameObject.SetActive(false);
    }


    public void SoundButton_OnCLick()
    {
        isAudioPaused = !isAudioPaused;
        AudioListener.pause = isAudioPaused;

        soundButtonText.text = isAudioPaused ? "Sound off" : "Sound on";
    }

    #endregion
}
