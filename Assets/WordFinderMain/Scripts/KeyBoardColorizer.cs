using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyBoardColorizer : MonoBehaviour
{
    [Header("Elements")] 
    private KeyboardKey[] _keys;

    [Header("Settings")] 
    private bool _shouldReset;

    private void Awake()
    {
        _keys = GetComponentsInChildren<KeyboardKey>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateChangedCallback;
    }
    
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                if (_shouldReset)
                {
                    Initialize();
                }
                break;
            case GameState.LevelComplete:
                _shouldReset = true;
                break;
            
            case GameState.GameOver:
                _shouldReset = true;
                break;
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < _keys.Length; i++)
        {
            _keys[i].Initialize();
        }

        _shouldReset = false;
    }

    public void Colorize(string secretWord, string wordToCheck)
    {
        for (int i = 0; i < _keys.Length; i++)
        {
            char keyLetter = _keys[i].GetLetter();

            for (int j = 0; j < wordToCheck.Length; j++)
            {
                if (keyLetter!=wordToCheck[j])
                {
                    continue;
                }

                if (keyLetter==secretWord[j])
                {
                    //Valid
                    _keys[i].SetValid();
                }else if (secretWord.Contains(keyLetter))
                {
                    //Potential
                    _keys[i].SetPotential();
                }
                else
                {
                    //Invalid
                    _keys[i].SetInvalid();
                }
            }
        }
    }
}
