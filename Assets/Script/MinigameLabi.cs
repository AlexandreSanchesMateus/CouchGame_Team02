using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinigameLabi : MonoBehaviour, IMinigame
{
    public bool interact(InputAction.CallbackContext callback)
    {
        return false;
    }
    public void Move(InputAction.CallbackContext callback)
    {
        Vector2 value = callback.ReadValue<Vector2>().normalized;
        if(value.x<0&&value.y>0)
        {
            Debug.Log("left");
            LabyrinthManager.instance.MovePlayerOnGrid(new Vector2(-1,0));
        }
        else if (value.x > 0 && value.y > 0)
        {
            Debug.Log("right");
            LabyrinthManager.instance.MovePlayerOnGrid(new Vector2(1, 0));
        }
        else if (value.x < 0 && value.y < 0)
        {
            Debug.Log("down");
            LabyrinthManager.instance.MovePlayerOnGrid(new Vector2(0, -1));
        }
        else if (value.x > 0 && value.y < 0)
        {
            Debug.Log("up");
            LabyrinthManager.instance.MovePlayerOnGrid(new Vector2(0, 1));
        }
    }
}
