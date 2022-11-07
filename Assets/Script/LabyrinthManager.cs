using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthManager : MonoBehaviour
{
    public static ScrLabyrinth labyrinth;

    private List<Transform> transformGridSlot;

    private void Start()
    {
        // Choisie un point de départ aléatoir et un point d'arriver à l'opposé.
        if(labyrinth.idStartLabyrinth == -1 || labyrinth.idEndLabyrinth == -1)
        {

        }
        else
            labyrinth.idPlayerSlot = labyrinth.idStartLabyrinth;
    }

    private void Awake()
    {
        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            transformGridSlot.Add(this.transform.GetChild(0).GetChild(i));
        }
    }

    public static void MovePlayerOnGrid(Vector2 direction, bool isRobberMoving)
    {
        bool allowToMove = false;

        switch (direction)
        {
                // ---- Vers la droite ---- //
            case { x: 1, y: 0 }:
                allowToMove = CheckSlotAccess(labyrinth.grid[labyrinth.idPlayerSlot].rightWall, isRobberMoving);
                break;

                // ---- Vers le haut ---- //
            case { x: 0, y: 1 }:

                break;

                // ---- Vers la gauche ---- //
            case { x: -1, y: 0 }:

                break;

                // ---- Vers le bas ---- //
            case { x: 0, y: -1 }:
                allowToMove = CheckSlotAccess(labyrinth.grid[labyrinth.idPlayerSlot].bottomWall, isRobberMoving);
                break;
        }

        if (allowToMove)
        {

        }
    }

    private static bool CheckSlotAccess(Slot.ACCESS access, bool isRobberMoving)
    {
        switch (access)
        {
            case Slot.ACCESS.BLOCK:
                return false;
            case Slot.ACCESS.BOTH:
                return true;
            case Slot.ACCESS.EITHER:
                ResetLabyrinthe();
                return false;
            case Slot.ACCESS.HACKER:
                if (!isRobberMoving)
                    return true;
                else
                    return false;
            case Slot.ACCESS.ROBBER:
                if (isRobberMoving)
                    return true;
                else
                    return false;
        }

        return false;
    }

    private static void ResetLabyrinthe()
    {

    }
}
