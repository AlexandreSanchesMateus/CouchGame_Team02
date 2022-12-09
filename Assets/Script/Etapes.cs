using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Etapes
{
    public Situation[] situations = { new Situation(ELEMENTS.POP), new Situation(ELEMENTS.SUS), new Situation(ELEMENTS.PEPE), new Situation(ELEMENTS.DOGE), };
}

[System.Serializable]
public struct Situation
{
    public ELEMENTS element;
    public int goodkey;

    public Situation(ELEMENTS el)
    {
        element = el;
        goodkey = 0;
    }

}

[System.Serializable]
public enum ELEMENTS
{
    POP,
    SUS,
    PEPE,
    DOGE
}
