using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("Data")] 
    private int _coins;
    private int _score;
    private int _bestScore;

    [Header("Evemts")]
    public static Action OnCoinsUpdated;

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

        LoadData();
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
       
        SaveData();
        
        OnCoinsUpdated?.Invoke();
    }

    public void RemoveCoins(int amount)
    {
        _coins -= amount;
        _coins = Mathf.Max(_coins, 0);
        
        SaveData();
        
        OnCoinsUpdated?.Invoke();
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;
        if (_score>_bestScore)
        {
            _bestScore = _score;
        }
        
        SaveData();
    }

    public int GetCoins()
    {
        return _coins;
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetBestScore()
    {
        return _bestScore;
    }

    private void LoadData()
    {
        _coins = PlayerPrefs.GetInt(TagManager.COINS_PREFS, 150);
        _score = PlayerPrefs.GetInt(TagManager.SCORE_PREFS);
        _bestScore = PlayerPrefs.GetInt(TagManager.BESTSCORE_PREFS);
    }
    
    private void SaveData()
    {
        PlayerPrefs.SetInt(TagManager.COINS_PREFS,_coins);
        PlayerPrefs.SetInt(TagManager.SCORE_PREFS,_score);
        PlayerPrefs.SetInt(TagManager.BESTSCORE_PREFS,_bestScore);
    }

    public void ResetScore()
    {
        _score = 0;
        SaveData();
    }
}
