using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame1 : MonoBehaviour,IMinigame
{
    
    public bool interact(InputAction.CallbackContext callback)
    {
        Debug.Log("grosse bite");
        return true;
    }

    public void Move(InputAction.CallbackContext callback)
    {
        //throw new System.NotImplementedException();
    }
}
