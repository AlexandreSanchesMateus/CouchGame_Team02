using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MiniGameManager : MonoBehaviour
{
    //singleton of minigame manager
    public static MiniGameManager instance;

    public List<MiniGame> minigames = new List<MiniGame>();
    
    void Start()
    {
        
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
