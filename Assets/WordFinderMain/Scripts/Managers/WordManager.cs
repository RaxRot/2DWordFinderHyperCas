using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordManager : MonoBehaviour
{
   public static WordManager Instance;

   [Header("Elements")]
   [SerializeField] private string secretWord;
   [SerializeField] private TextAsset wordsText;
   private string _words;

   [Header("Settings")] 
   private bool _shouldReset;

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

      _words = wordsText.text;
   }

   private void OnEnable()
   {
      GameManager.OnGameStateChanged += GameStateChangedCallback;
   }

   private void OnDisable()
   {
      GameManager.OnGameStateChanged -= GameStateChangedCallback;
   }
   
   private void Start()
   {
      SetNewSecretWord();
   }

   public string GetSecretWord()
   {
      return secretWord.ToUpper();
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
               SetNewSecretWord();
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

   private void SetNewSecretWord()
   {
      int wordCount = (_words.Length + 2) / 7;
      int wordIndex = Random.Range(0, wordCount);
      int wordStartIndex = wordIndex * 7;

      secretWord = _words.Substring(wordStartIndex, 5).ToUpper();

      _shouldReset = false;
   }
}
