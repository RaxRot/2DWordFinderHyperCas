using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Image soundsImage;
    [SerializeField] private Image hapticsImage;

    [Header("Settings")] 
    private bool _soundsState;

    private void Start()
    {
        LoadStates();
    }

    private void LoadStates()
    {
        _soundsState = PlayerPrefs.GetInt(TagManager.SOUND_PREFS,1) == 1;
        UpdateSoundsState();
    }

    private void SaveStates()
    {
        PlayerPrefs.SetInt(TagManager.SOUND_PREFS,_soundsState? 1:0);
    }

    public void SoundsButtonCallback()
    {
        _soundsState = !_soundsState;

        UpdateSoundsState();
        
        SaveStates();
    }

    private void UpdateSoundsState()
    {
        if (_soundsState)
        {
            EnableSounds();
        }
        else
        {
            DisableSounds();
        }
    }
    
    private void EnableSounds()
    {
       SoundManager.Instance.EnableSounds();
        soundsImage.color=Color.white;
    }

    private void DisableSounds()
    {
        SoundManager.Instance.DisableSounds();
        soundsImage.color=Color.gray;
    }
}
