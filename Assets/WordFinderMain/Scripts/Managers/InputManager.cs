using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    [Header("Elements")]
    [SerializeField] private WordContainer[] wordContainers;
    [SerializeField] private Button tryButton;
    [SerializeField] private KeyBoardColorizer keyBoardColorizer;

    [Header("Settings")] 
    private int _currentWordContainerIndex;
    private bool _canAddLetter=true;
    private bool _canRemoveLetter = true;
    private bool _shouldReset;

    [Header("Events")] 
    public static Action OnLetterAdded;
    public static Action OnLetterRemoved;

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
        KeyboardKey.OnKeyPressed += KeyPressedCallback;
        GameManager.OnGameStateChanged += GameStateChangedCallback;
    }

    private void OnDisable()
    {
        KeyboardKey.OnKeyPressed -= KeyPressedCallback;
        GameManager.OnGameStateChanged -= GameStateChangedCallback;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentWordContainerIndex = 0;
        _canAddLetter = true;
        
        DisableTryButton();
        
        for (int i = 0; i < wordContainers.Length; i++)
        {
            wordContainers[i].Initialize();
        }

        _shouldReset = false;
    }
    
    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.LevelComplete:
                _shouldReset = true;
                break;
            
            case GameState.Game:
                if (_shouldReset)
                {
                    Initialize();
                }
                _canRemoveLetter = true;
                break;
            
            case GameState.GameOver:
                _canRemoveLetter = false;
                _shouldReset = true;
                break;
        }
    }
    
    private void KeyPressedCallback(char letter)
    {
        if (!_canAddLetter)
        {
            return;
        }
        
        wordContainers[_currentWordContainerIndex].Add(letter);
        
        if (wordContainers[_currentWordContainerIndex].IsComplete())
        {
            _canAddLetter = false;
            
            EnableTryButton();
        }
        
        OnLetterAdded?.Invoke();
    }

    public void CheckWord()
    {
        string wordToCheck = wordContainers[_currentWordContainerIndex].GetWord();
        string secretWord = WordManager.Instance.GetSecretWord();

        wordContainers[_currentWordContainerIndex].Colorize(secretWord);
        keyBoardColorizer.Colorize(secretWord,wordToCheck);

        if (wordToCheck==secretWord)
        {
            SetLevelComplete();
        }
        else
        {
            _currentWordContainerIndex++;
            DisableTryButton();

            if (_currentWordContainerIndex>=wordContainers.Length)
            {
                DataManager.Instance.ResetScore();
                GameManager.Instance.SetGameState(GameState.GameOver);
            }
            else
            {
                _canAddLetter = true;
            }
        }
    }

    private void SetLevelComplete()
    {
        UpdateData();
        
       GameManager.Instance.SetGameState(GameState.LevelComplete);
    }

    private void UpdateData()
    {
        int scoreToAdd = 6 - _currentWordContainerIndex;
        int coinsToAdd = scoreToAdd * 10;
        
        DataManager.Instance.IncreaseScore(scoreToAdd);
        DataManager.Instance.AddCoins(coinsToAdd);
    }

    public void BackspacePressedCallback()
    {
        if (!GameManager.Instance.IsGameState())
        {
            return;
        }
        bool removedLetter = wordContainers[_currentWordContainerIndex].RemoveLetter();
        
            if (removedLetter)
            {
                DisableTryButton();
            }

            _canAddLetter = true;
            
            OnLetterRemoved?.Invoke();
    }

    private void EnableTryButton()
    {
        tryButton.interactable = true;
    }

    private void DisableTryButton()
    {
        tryButton.interactable = false;
    }

    public WordContainer GetCurrentWordContainer()
    {
        return wordContainers[_currentWordContainerIndex];
    }
}
