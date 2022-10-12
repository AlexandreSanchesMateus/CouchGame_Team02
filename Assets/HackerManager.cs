using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerManager : MonoBehaviour
{


    
    public static HackerManager instance;
    void Start()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
