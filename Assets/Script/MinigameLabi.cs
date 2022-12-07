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
        if (!callback.performed || callback.ReadValue<Vector2>().magnitude < 0.5f) return;

        Vector2 value = callback.ReadValue<Vector2>().normalized;
        Debug.Log(new Vector2(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y)));

        LabyrinthManager.instance.MovePlayerOnGrid(new Vector2(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y)));

        /*if (Mathf.Abs(value.x) > -0.5f && Mathf.Abs(value.x) < 0.5f)
        {
            if (value.x < 0)
                Debug.Log("Bas");
            else
                Debug.Log("Haut");
        }
        else
        {
            if (value.y < 0)
                Debug.Log("Gauche");
            else
                Debug.Log("Doite");
        }*/


        /*if(value.x<0&&value.y>0)
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
        }*/
    }
}
