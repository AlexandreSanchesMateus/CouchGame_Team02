using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Screen : MonoBehaviour
{
    public MiniGame game;
    private void Start()
    {
        if (game == null)
        {
            gameObject.tag = "ads";  
        }
        else
        {
            gameObject.tag = "MiniGame";
        }
    }
    
}
