using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction movement;


    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInput.FPSController.Move.performed += DoMove;
        playerInput.FPSController.Move.Enable();
        
        playerInput.FPSController.Look.performed += DoLook;
        playerInput.FPSController.Look.Enable();
       // movement = playerInput.FPSController.Move;
       // movement.Enable();

        playerInput.FPSController.Interact.performed += DoInteract;
        playerInput.FPSController.Interact.Enable();

    }
    private void DoInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("interact");
    }

    private void DoMove(InputAction.CallbackContext ctx)
    {
        Debug.Log("Movement Vector 2 : " + ctx.ReadValue<Vector2>());
    }
    
    private void DoLook(InputAction.CallbackContext ctx)
    {
        Debug.Log("Looking Vector 2 : " + ctx.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        
    }
}

