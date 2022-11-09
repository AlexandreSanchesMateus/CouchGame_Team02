using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game1TextManager : MonoBehaviour
{
    [SerializeField] private int nbLine; // 1, 2 ou 3
    [SerializeField] private int nbColumn; // a, b ou c
    [SerializeField] List<string> letters;

    void Start()
    {
        nbLine = Random.Range(0, 2);
        nbColumn = Random.Range(0, 2);
    }

    void Update()
    {
        
    }
}
