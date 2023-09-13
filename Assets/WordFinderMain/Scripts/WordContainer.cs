using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordContainer : MonoBehaviour
{
    [Header("Elements")] 
    private LetterContainer[] _letterContainers;

    [Header("Settings")]
    private int _currentLetterIndex;

    private void Awake()
    {
        _letterContainers = GetComponentsInChildren<LetterContainer>();
        
        //Initialize();
    }

    public void Initialize()
    {
        _currentLetterIndex = 0;
        
        for (int i = 0; i < _letterContainers.Length; i++)
        {
            _letterContainers[i].Initialize();
        }
    }

    public void Add(char letter)
    {
        _letterContainers[_currentLetterIndex].SetLetter(letter);
        _currentLetterIndex++;
    }

    public bool IsComplete()
    {
        return _currentLetterIndex >= _letterContainers.Length;
    }

    public string GetWord()
    {
        string word = "";

        for (int i = 0; i < _letterContainers.Length; i++)
        {
            word += _letterContainers[i].GetLetter().ToString();
        }

        return word;
    }

    public bool RemoveLetter()
    {
        if (_currentLetterIndex<=0)
        {
            return false;
        }

        _currentLetterIndex--;
        _letterContainers[_currentLetterIndex].Initialize();

        return true;
    }

    public void Colorize(string secretWord)
    {
        List<char> chars = new List<char>(secretWord.ToCharArray());
        for (int i = 0; i < chars.Count; i++)
        {
            Debug.Log(chars[i]);
        }
        
        for (int i = 0; i < _letterContainers.Length; i++)
        {
            char letterToCheck = _letterContainers[i].GetLetter();

            if (letterToCheck==secretWord[i])
            {
                //Valid
                _letterContainers[i].SetValid();
                chars.Remove(letterToCheck);

            }else if (chars.Contains(letterToCheck))
            {
                //Potential
                _letterContainers[i].SetPotential();
                chars.Remove(letterToCheck);
            }
            else
            {
                //Invalid
                _letterContainers[i].SetInvalid();
            }
        }
    }


    public void AddAsHint(int letterIndex, char letter)
    {
       _letterContainers[letterIndex].SetLetter(letter,true);
    }
}
