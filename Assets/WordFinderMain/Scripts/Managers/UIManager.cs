using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Elements")] 
    [SerializeField] private CanvasGroup menuCG;
    [SerializeField] private CanvasGroup gameCG;
    [SerializeField] private CanvasGroup levelCompleteCG;
    [SerializeField] private CanvasGroup gameOverCG;
    [SerializeField] private CanvasGroup settingsCG; 

    [Header("Menu Elements")] 
    [SerializeField] private TextMeshProUGUI menuCoins;
    [SerializeField] private TextMeshProUGUI menuBestScore;
    
    [Header("Level Complete Elements")] 
    [SerializeField] private TextMeshProUGUI levelCompleteCoins;
    [SerializeField] private TextMeshProUGUI levelCompleteSecretWord;
    [SerializeField] private TextMeshProUGUI levelCompleteScore;
    [SerializeField] private TextMeshProUGUI levelCompleteBestScore;
    
    [Header("Game Over Elements")]
    [SerializeField] private TextMeshProUGUI gameOverCoins;
    [SerializeField] private TextMeshProUGUI gameOverSecretWord;
    [SerializeField] private TextMeshProUGUI gameOverBestScore;
    
    [Header("Game Elements")]
    [SerializeField] private TextMeshProUGUI gameScore;
    [SerializeField] private TextMeshProUGUI gameCoins;
    

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

    private void Start()
    {
        ShowMenu();
        HideGame();
        HideLevelComplete();
        HideGameOver();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateChangedCallback;
        DataManager.OnCoinsUpdated += UpdateCoinsText;
    }
    
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameStateChangedCallback;
        DataManager.OnCoinsUpdated -= UpdateCoinsText;
    }
    
    private void UpdateCoinsText()
    {
        menuCoins.text = DataManager.Instance.GetCoins().ToString();
        gameCoins.text = menuCoins.text;
        levelCompleteCoins.text = menuCoins.text;
        gameOverCoins.text = menuCoins.text;
    }
    
    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                ShowMenu();
                HideGame();
                
                break;
            case GameState.LevelComplete:
                ShowLevelComplete();
                HideGame();
                
                break;
            
            case GameState.Game:
                ShowGame();
                HideMenu();
                HideLevelComplete();
                HideGameOver();
                break;
            
            case GameState.GameOver:
                ShowGameOver();
                HideGame();
                break;
        }
    }

    private void ShowGameOver()
    {
        gameOverCoins.text = DataManager.Instance.GetCoins().ToString();
        gameOverSecretWord.text = WordManager.Instance.GetSecretWord();
        gameOverBestScore.text = DataManager.Instance.GetBestScore().ToString();
        
        ShowCG(gameOverCG);
    }

    private void HideGameOver()
    {
        HideCG(gameOverCG);
    }

    private void ShowMenu()
    {
        menuCoins.text = DataManager.Instance.GetCoins().ToString();
        menuBestScore.text = DataManager.Instance.GetBestScore().ToString();
        
        ShowCG(menuCG);
    }

    private void HideMenu()
    {
        HideCG(menuCG);
    }
    private void ShowGame()
    {
        gameCoins.text = DataManager.Instance.GetCoins().ToString();
        gameScore.text = DataManager.Instance.GetScore().ToString();
        
        ShowCG(gameCG);
    }

    private void HideGame()
    {
        HideCG(gameCG);
    }

    public void ShowSettings()
    {
        ShowCG(settingsCG);
    }

    public void HideSettings()
    {
        HideCG(settingsCG);
    }

    private void ShowLevelComplete()
    {
        levelCompleteCoins.text = DataManager.Instance.GetCoins().ToString();
        levelCompleteSecretWord.text = WordManager.Instance.GetSecretWord();
        levelCompleteScore.text = DataManager.Instance.GetScore().ToString();
        levelCompleteBestScore.text = DataManager.Instance.GetBestScore().ToString();
        
        ShowCG(levelCompleteCG);
    }

    private void HideLevelComplete()
    {
        HideCG(levelCompleteCG);
    }

    private void ShowCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    private void HideCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    
    
    
}
