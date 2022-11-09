using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinthe : MonoBehaviour
{
    public GameObject GUIhover;
    public GameObject labirinthe;

    public static int gridSize;
    public static int idPlayerSlot;
    public static List<Slot> grid = new List<Slot>((int)Mathf.Pow(Labyrinthe.gridSize, 2));

    public struct Slot
    {
        public ACCESS rightWall;
        public ACCESS bottomWall;

        public Slot(Slot source)
        {
            rightWall = source.rightWall;
            bottomWall = source.bottomWall;
        }
    }

    public enum ACCESS
    {
        BLOCK,
        HACKER,
        ROBBER,
        BOTH,
        EITHER
    }

}
