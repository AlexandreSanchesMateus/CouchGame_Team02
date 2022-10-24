using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerProto2 : MonoBehaviour
{

    [SerializeField] private GameObject cameraObj;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float sensitivity = 30f;
    [Header("Interact Option")]
    [SerializeField] private float radius;
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;

    private float xRotation;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public static bool enablePlayerMovement { get; set; }
    private IInteractible interactibleObject;

    private Vector2 movementInput;
    private Vector2 rotateInput;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        enablePlayerMovement = true;
    }

    void Update()
    {
        // Désactivation des mouvement
        if (!enablePlayerMovement)
            return;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        RaycastHit info;
        if (Physics.SphereCast(cameraObj.transform.position, radius, cameraObj.transform.forward, out info, range, layer))
        {
            Debug.DrawLine(cameraObj.transform.position, info.transform.position, Color.green);
            if (interactibleObject == null && info.transform.TryGetComponent<IInteractible>(out interactibleObject))
            {
                Debug.Log("HOVER");
                interactibleObject.OnItemHover();
            }
        }
        else
        {
            Debug.DrawLine(cameraObj.transform.position, cameraObj.transform.position + (cameraObj.transform.forward * range), Color.black);
            if (interactibleObject != null)
            {
                Debug.Log("EXIT");
                interactibleObject.OnItemExit();
                interactibleObject = null;
            }
        }

        Vector3 move = transform.rotation * new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Souris Horitale
        transform.Rotate(Vector3.up * (rotateInput.x * sensitivity * Time.deltaTime));

        // Souris verticale
        xRotation -= rotateInput.y * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        cameraObj.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && interactibleObject != null)
        {
            interactibleObject.OnIteract();
        }
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed && interactibleObject != null)
        {
            interactibleObject.OnReturn();
            interactibleObject = null;
        }
    }

    public void OnActions(InputAction.CallbackContext context)
    {
        if (context.performed && interactibleObject != null)
        {
            interactibleObject.OnActions(context.ReadValue<Vector2>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cameraObj.transform.position, radius);// d�part
        Gizmos.DrawSphere(cameraObj.transform.position + (cameraObj.transform.forward * range), radius);// arriver
    }
}

