using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame : MonoBehaviour, IMinigame
{
    public bool isGood;
    public bool isInside;
    public Camera Cam;
    public int code;
    public TextMeshPro text;
    public bool interact(InputAction.CallbackContext callback) 
    {
        if (isGood)
        {
            text.text = code.ToString();
            return true;
        }
        else
        {
            text.text = Random.Range(0, 99).ToString();
            return false;
        }
    }

    public void Move(InputAction.CallbackContext callback)
    {
        //throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("cube"))
        {
            isGood = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cube"))
        {
            isGood = false;
        }
    }

}
