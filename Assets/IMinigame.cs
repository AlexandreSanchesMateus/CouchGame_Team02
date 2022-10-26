using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMinigame
{
    public bool interact(InputAction.CallbackContext callback);
}
