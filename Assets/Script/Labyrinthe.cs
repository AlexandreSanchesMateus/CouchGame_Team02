using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinthe : MonoBehaviour
{
    public GameObject GUIhover;
    public GameObject labirinthe;

    public static int gridSizeX;
    public static int gridSizeY;
    public static int idPlayerSlot;
    public static List<Slot> grid = new List<Slot>();

    private int b;
}
