using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HintManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private GameObject keyBoard;
    private KeyboardKey[] _keys;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI keyBoardPriceText;
    [SerializeField] private TextMeshProUGUI letterPriceText;

    [Header("Settings")] 
    private bool _shouldReset;
    [SerializeField] private int keyBoardHintPrice;
    [SerializeField] private int letterHintPrice;
    

    private void Awake()
    {
        _keys = keyBoard.GetComponentsInChildren<KeyboardKey>();

        keyBoardPriceText.text = keyBoardHintPrice.ToString();
        letterPriceText.text = letterHintPrice.ToString();
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
            case GameState.Menu:
                break;
            case GameState.Game:
                if (_shouldReset)
                {
                    letterHintGivenIndices.Clear();
                    _shouldReset = false;
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

    public void KeyBoardHint()
    {
        if (DataManager.Instance.GetCoins()<keyBoardHintPrice)
        {
            return;
        }
        
        string secretWord = WordManager.Instance.GetSecretWord();

        List<KeyboardKey> untouchedKeys = new List<KeyboardKey>();

        for (int i = 0; i < _keys.Length; i++)
        {
            if (_keys[i].IsUntouched())
            {
                untouchedKeys.Add(_keys[i]);
            }
        }
        //We have list untouched keys

        List<KeyboardKey> t_untouchedKeys = new List<KeyboardKey>(untouchedKeys);

        for (int i = 0; i < untouchedKeys.Count; i++)
        {
            if (secretWord.Contains(untouchedKeys[i].GetLetter()))
            {
                t_untouchedKeys.Remove(untouchedKeys[i]);
            }
        }
        //We have a list untouchedKeys not contained into secret word

        if (t_untouchedKeys.Count<=0)
        {
            return;
        }

        int randomKeyIndex = Random.Range(0, t_untouchedKeys.Count);
        t_untouchedKeys[randomKeyIndex].SetInvalid();
        
        DataManager.Instance.RemoveCoins(keyBoardHintPrice);
    }

    private List<int> letterHintGivenIndices = new List<int>();
    public void LetterHint()
    {
        if (DataManager.Instance.GetCoins()<letterHintPrice)
        {
            return;
        }
        
        if (letterHintGivenIndices.Count>=5)
        {
            return;
        }

        List<int> letterHintNotGivenIndices = new List<int>();

        for (int i = 0; i < 5; i++)
        {
            if (!letterHintGivenIndices.Contains(i))
            {
                letterHintNotGivenIndices.Add(i);
            }
        }

        WordContainer currentWordContainer = InputManager.Instance.GetCurrentWordContainer();
        string secretWord = WordManager.Instance.GetSecretWord();

        int randomIndex = letterHintNotGivenIndices[Random.Range(0, letterHintNotGivenIndices.Count)];
        letterHintGivenIndices.Add(randomIndex);
        
        currentWordContainer.AddAsHint(randomIndex,secretWord[randomIndex]);
        
        DataManager.Instance.RemoveCoins(letterHintPrice);
    }
}
