using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction movement;
    private InputAction looking;

    private Rigidbody rb;
    private float xRotation = 0.0f;

    [SerializeField] private GameObject cameraObj;
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float sensitivity = 100.0f;
    
    
    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        movement = playerInput.FPSController.Move;
        movement.Enable();

        looking = playerInput.FPSController.Look;
        looking.Enable();

        playerInput.FPSController.Interact.performed += DoInteract;
        playerInput.FPSController.Interact.Enable();

    }

    private void OnDisable()
    {
        movement.Disable();
        looking.Disable();
        playerInput.FPSController.Interact.Disable();
    }

    private void DoInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("interact");
    }

    private void Update()
    {
        // Souris horizontale
        transform.Rotate(Vector3.up * (looking.ReadValue<Vector2>().x * sensitivity * Time.deltaTime));

        // Souris verticale
        xRotation -= looking.ReadValue<Vector2>().y * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        cameraObj.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    private void FixedUpdate()
    {
        // Déplacement dans le monde
        Debug.Log("Movement : " + movement.ReadValue<Vector2>());
        Vector3 direction = transform.rotation * new Vector3(movement.ReadValue<Vector2>().x, 0.0f, movement.ReadValue<Vector2>().y);

        rb.AddForce(direction * speed * Time.fixedDeltaTime);
    }
}

