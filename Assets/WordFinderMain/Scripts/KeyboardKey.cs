using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum Validity
{
 None,
 Valid,
 Potential,
 Invalid
}

public class KeyboardKey : MonoBehaviour
{
   [Header("Elements")]
   [SerializeField] private TextMeshProUGUI letterText;
   [SerializeField] private Image renderer;

   [Header("Events")] 
   public static Action<char> OnKeyPressed;

   [Header("Settings")] 
   private Validity _validity;
   
   private void Start()
   {
      GetComponent<Button>().onClick.AddListener(SendKeyPressedEvent);
      
      Initialize();
   }

   public void Initialize()
   {
      renderer.color=Color.white;
      _validity = Validity.None;
   }

   private void SendKeyPressedEvent()
   {
      OnKeyPressed?.Invoke(letterText.text[0]);
   }

   public char GetLetter()
   {
      return letterText.text[0];
   }

   public void SetValid()
   {
     renderer.color=Color.green;
     _validity = Validity.Valid;
   }

   public void SetPotential()
   {
      if (_validity==Validity.Valid)
      {
         return;
      }
      
      renderer.color=Color.yellow;
      _validity = Validity.Potential;
   }

   public void SetInvalid()
   {
      if (_validity==Validity.Valid || _validity==Validity.Potential)
      {
         return;
      }
      
      renderer.color=Color.gray;
      _validity = Validity.Invalid;
   }

   public bool IsUntouched()
   {
      return _validity == Validity.None;
   }
}
