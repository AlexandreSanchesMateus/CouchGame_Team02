using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame1 : MonoBehaviour,IMinigame
{
    public bool TestWin(InputAction.CallbackContext callback)
    {
        return true;
    }
}
