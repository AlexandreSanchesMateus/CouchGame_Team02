using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerProto2 : MonoBehaviour
{
    public static PlayerControllerProto2 instance { get; private set; }

    public GameObject cameraObj;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float sensitivity = 30f;

    [Header("Interact Option")]
    [SerializeField] private float radius;
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;
    public Transform hand;
    private bool haveSomthingInHand = false;

    [Header("Headbob Option")]
    [SerializeField] bool enableHeadbob;
    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float _frequency = 10.0f;
    private float startPosY;
    private float timer;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> walk;

    private CharacterController controller;
    private float xRotation;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public static bool enablePlayerMovement { get; set; }
    private GameObject interactibleObject;

    private Vector2 movementInput;
    private Vector2 rotateInput;

    private Vector2 flechaction;

    private float previousSin;
    private bool walked;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        startPosY = cameraObj.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        enablePlayerMovement = true;
    }

    void Update()
    {
        if (enableHeadbob)
            HeadbobHandle();

        // Désactivation des mouvement
        if (!enablePlayerMovement)
            return;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Grab
        if (!haveSomthingInHand)
        {
            // Interaction Raycast
            RaycastHit info;
            if (Physics.SphereCast(cameraObj.transform.position, radius, cameraObj.transform.forward, out info, range, layer))
            {
                Debug.DrawLine(cameraObj.transform.position, info.transform.position, Color.green);
                if (interactibleObject == null && info.transform.TryGetComponent<IInteractible>(out IInteractible interactibleScript))
                {
                    interactibleObject = info.transform.gameObject;
                    interactibleScript.OnItemHover();
                }
            }
            else
            {
                Debug.DrawLine(cameraObj.transform.position, cameraObj.transform.position + (cameraObj.transform.forward * range), Color.black);
                if (interactibleObject != null)
                {
                    interactibleObject.GetComponent<IInteractible>().OnItemExit();
                    interactibleObject = null;
                }
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

    private void HeadbobHandle()
    {
        if (!controller.isGrounded)
        {
            ResetPosition();
            return;
        }

        if (Mathf.Abs(movementInput.x) > 0.02f && enablePlayerMovement || Mathf.Abs(movementInput.y) > 0.02f && enablePlayerMovement)
        {
            //Debug.Log(movementInput.x + movementInput.y);
            timer += Time.deltaTime * _frequency;
            cameraObj.transform.localPosition = new Vector3(cameraObj.transform.localPosition.x, startPosY + Mathf.Sin(timer) * _amplitude, cameraObj.transform.localPosition.z);
            AudioOnWalk();
            previousSin = Mathf.Sin(timer);
            
        }
        else if (cameraObj.transform.localPosition.y != startPosY)
            ResetPosition();
    }

    private void ResetPosition()
    {
        if (cameraObj.transform.localPosition.y == startPosY)
            return;

        cameraObj.transform.localPosition = new Vector3(cameraObj.transform.localPosition.x, Mathf.Lerp(cameraObj.transform.localPosition.y, startPosY, Time.time * 0.01f), cameraObj.transform.localPosition.z);
        timer = 0;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<Vector2>();

        if (interactibleObject != null)
            InteractWithEnigmes();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && interactibleObject != null)
        {
            interactibleObject.GetComponent<IInteractible>().OnInteract();

            if (interactibleObject.GetComponent<InspectedObject>() != null)
                haveSomthingInHand = true;
        }
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (!haveSomthingInHand)
        {
            if (context.performed && interactibleObject != null)
                interactibleObject.GetComponent<IInteractible>().OnReturn();
            else
                return;
        }
        else
        {
            if (context.canceled && interactibleObject != null)
                interactibleObject.GetComponent<IInteractible>().OnReturn();
            else
                return;
        }

        interactibleObject = null;
        haveSomthingInHand = false;
    }

    public void OnActions(InputAction.CallbackContext context)
    {
        flechaction = context.ReadValue<Vector2>();

        if (context.performed && interactibleObject != null)
            InteractWithEnigmes();
    }

    public void OnSwitchInteraction(InputAction.CallbackContext context)
    {
        if(haveSomthingInHand && context.performed && interactibleObject != null)
        {
            interactibleObject.GetComponent<IInteractible>().OnRightShoulder();
        }
    }

    public void OnHoldReturnButton(InputAction.CallbackContext context)
    {
        if (haveSomthingInHand && context.performed && interactibleObject != null)
        {
            interactibleObject.GetComponent<IInteractible>().OnHoldReturn();
        }
    }

    private void InteractWithEnigmes()
    {
        if(interactibleObject != null)
            interactibleObject.GetComponent<IInteractible>().OnActions(flechaction, rotateInput);
    }

    private void AudioOnWalk()
    {
        if (walk.Count == 0)
            return;

        if (previousSin < Mathf.Sin(timer) && !walked)
        {
            GetComponent<AudioSource>().clip = walk[Random.Range(0, walk.Count)];
            GetComponent<AudioSource>().Play();
            walked = true;
        }
        else if(previousSin > Mathf.Sin(timer))
        {
            walked = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cameraObj.transform.position, radius);// d�part
        Gizmos.DrawSphere(cameraObj.transform.position + (cameraObj.transform.forward * range), radius);// arriver
    }
}

