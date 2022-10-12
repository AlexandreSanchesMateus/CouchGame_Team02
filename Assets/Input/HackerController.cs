using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HackerController : MonoBehaviour
{

    public void Increment(InputAction.CallbackContext callback)
    {
        Debug.Log("Increment");
    }
    public void Decrement(InputAction.CallbackContext callback)
    {
        Debug.Log("Decrement");
    }
}
