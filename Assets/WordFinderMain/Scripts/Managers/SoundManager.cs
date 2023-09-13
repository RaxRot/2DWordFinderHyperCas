using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sounds")] 
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource letterAddedSound;
    [SerializeField] private AudioSource letterRemovedSound;
    [SerializeField] private AudioSource levelCompleteSound;
    [SerializeField] private AudioSource gameOverSound;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        InputManager.OnLetterAdded += PlayLetterAddedSound;
        InputManager.OnLetterRemoved += PlayLetterRemovedSound;

        GameManager.OnGameStateChanged += GameStateChangedCallback;
    }

    private void OnDisable()
    {
        InputManager.OnLetterAdded -= PlayLetterAddedSound;
        InputManager.OnLetterRemoved -= PlayLetterRemovedSound;
        
        GameManager.OnGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState state)
    {
        switch (state)
        {
            case GameState.LevelComplete:
                levelCompleteSound.Play();
                break;
            case GameState.GameOver:
                gameOverSound.Play();
                break;
        }
    }

    private void PlayLetterAddedSound()
    {
       letterAddedSound.Play();
    }
    
    private void PlayLetterRemovedSound()
    {
        letterRemovedSound.Play();
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    public void EnableSounds()
    {
        buttonSound.volume = 1;
        letterAddedSound.volume = 1;
        letterRemovedSound.volume = 1;
        levelCompleteSound.volume = 1;
        gameOverSound.volume = 1;
    }

    public void DisableSounds()
    {
        buttonSound.volume = 0;
        letterAddedSound.volume = 0;
        letterRemovedSound.volume = 0;
        levelCompleteSound.volume = 0;
        gameOverSound.volume = 0;
    }
}
