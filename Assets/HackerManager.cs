using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerManager : MonoBehaviour
{
    // Start is called before the first frame update
    //singleton
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
