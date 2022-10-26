using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Etapes
{
    public Situation[] situations = { new Situation(ELEMENTS.FIRE), new Situation(ELEMENTS.WATER), new Situation(ELEMENTS.WIND), new Situation(ELEMENTS.EARTH), };
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
    FIRE,
    WATER,
    WIND,
    EARTH
}
