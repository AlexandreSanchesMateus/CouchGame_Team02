using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public bool isGood;
    public bool isInside;
    public Camera Cam;
    public int code;
    public TextMeshPro text;
    public bool TestWin() 
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
