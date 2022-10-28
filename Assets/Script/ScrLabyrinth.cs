using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Labyrinth", menuName = "ScriptableObjects/LabyrinthScriptableObject", order = 1)]
public class ScrLabyrinth : ScriptableObject
{
    public int idPlayerSlot;

    public int gridSizeX;
    public int gridSizeY;
    public List<Slot> grid = new List<Slot>();

    public int idStartLabyrinth;
    public int idEndLabyrinth;
}
