using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Diagnostics.Tracing;

public class MiniGameChest : MonoBehaviour, IMinigame 
{
    [SerializeField] private string code;
    public TextMeshPro text; 
    public bool interact(InputAction.CallbackContext callback)
    {
            switch (callback.action.name)
            {
                case "West":
                text.text += " ";
                    break;
                case "South":
                text.text += ".";
                break;
                case "East":
                text.text += "_";
                break;
                case "North":
                    if(text.text.Length>0)
                    text.text.Remove(text.text.Length);
                break;
                default:
                break;
            }
        
        if (text.text == code)
            return true;
        return false;
    }
    public void Move(InputAction.CallbackContext callback) { }
}
