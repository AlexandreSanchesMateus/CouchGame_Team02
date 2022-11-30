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
    private bool isWin;
    public TextMeshPro text; 
    public bool interact(InputAction.CallbackContext callback)
    {
        if (!isWin)
            switch (callback.action.name)
            {
                case "West":
                text.text += " ";
                    break;
                case "South":
                text.text += ".";
                break;
                case "East":
                text.text += "-";
                break;
                case "North":
                    if(text.text.Length>0)
                    text.text = text.text.Substring(0, text.text.Length - 1);
                break;
                default:
                break;
            }

        if (text.text == code)
        {
            isWin = true;
            return true;
        }
        return false;
    }
    private void Update()
    {
        if(isWin)
        {
            text.text = "Douglas";
            text.color = Color.green;
        }
    }
    public void Move(InputAction.CallbackContext callback) { }
}
