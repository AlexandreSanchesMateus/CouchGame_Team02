using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HackerMultipleController : MonoBehaviour
{
    private PlayerInput multipleHackerController;
    [Header ("Camera")]
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private Transform neutralFocus;
    [SerializeField] private AnimationCurve camMouvement;
    [SerializeField] private float camSpeed;

    [Header ("Games")]
    [Tooltip ("Doit commencer par le haut, dans le sens des aiguille d'une montre")]
    [SerializeField] private GameObject[] interactibleEcran;

    private Quaternion lastOrientation;
    private GameObject target;
    private GameObject lastTarget;

    private float orientationTimer;
    private float positionTimer;
    private float stickRotationInDeg;

    private BaseGame gameScript;
    private Vector3 currentPos;
    private Vector3 startPos;
    private bool moveCamPosition = false;
    private bool reverse;

    private void Awake()
    {
        multipleHackerController = new PlayerInput();
    }

    private void OnEnable()
    {
        multipleHackerController.HackerController.Look.performed += DoLook;
        multipleHackerController.HackerController.Look.canceled += DoStart;
        multipleHackerController.HackerController.Look.Enable();

        multipleHackerController.HackerController.Interact.performed += DoInteract;
        multipleHackerController.HackerController.Interact.Enable();

        multipleHackerController.HackerController.Return.performed += DoReturn;
        multipleHackerController.HackerController.Return.Enable();
    }

    private void OnDisable()
    {
        multipleHackerController.HackerController.Look.Disable();
    }

    private void Start()
    {
        target = neutralFocus.gameObject;
        startPos = cameraObj.transform.position;
        lastTarget = null;
        orientationTimer = 1;
        currentPos = cameraObj.transform.position;
    }

    private void Update()
    {
        Debug.DrawLine(cameraObj.transform.position, target.transform.position, Color.green);
        // cameraObj.transform.rotation =  Quaternion.LookRotation(target.transform.position - cameraObj.transform.position);

        // Mouvement de Rotation
        if(orientationTimer < 1)
        {
            cameraObj.transform.rotation = Quaternion.Lerp(lastOrientation, Quaternion.LookRotation(target.transform.position - cameraObj.transform.position), camMouvement.Evaluate(orientationTimer));
            orientationTimer += Time.deltaTime * camSpeed;
        }
        else
            cameraObj.transform.rotation = Quaternion.LookRotation(target.transform.position - cameraObj.transform.position);

        // Mouvement de Position
        if (moveCamPosition) {
            if (reverse)
            {
                cameraObj.transform.position = Vector3.Lerp(startPos, currentPos, positionTimer);
                positionTimer -= Time.deltaTime * camSpeed;
                Debug.Log("Reverse");
            }
            else
            {
                cameraObj.transform.position = Vector3.Lerp(currentPos, gameScript.getPOF, positionTimer);
                positionTimer += Time.deltaTime * camSpeed;
                Debug.Log("Comming");
            }

            if (positionTimer < 0 || positionTimer > 1)
                moveCamPosition = false;
        }
    }

    private void DoLook(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>().normalized;

        stickRotationInDeg = Mathf.Rad2Deg * Mathf.Atan2(value.x, value.y);
        if (stickRotationInDeg < 0)
            stickRotationInDeg += 360;

        if (stickRotationInDeg == 0)
            return;

        if (!gameScript)
        {
            if (stickRotationInDeg > 330.0f || stickRotationInDeg < 30.0f)
                target = interactibleEcran[0];
            else if(stickRotationInDeg > 30.0f && stickRotationInDeg < 90.0f)
                target = interactibleEcran[1];
            else if(stickRotationInDeg > 90.0f && stickRotationInDeg < 150.0f)
                target = interactibleEcran[2];
            else if (stickRotationInDeg > 150.0f && stickRotationInDeg < 210.0f)
                target = interactibleEcran[3];
            else if (stickRotationInDeg > 210.0f && stickRotationInDeg < 270.0f)
                target = interactibleEcran[4];
            else if (stickRotationInDeg > 270.0f && stickRotationInDeg < 330.0f)
                target = interactibleEcran[5];

            if(lastTarget != target)
            {
                lastOrientation = cameraObj.transform.rotation;
                lastTarget = target;
                orientationTimer = 0.0f;
            }
        }
    }

    private void DoStart(InputAction.CallbackContext ctx)
    {
        if (!gameScript)
            RestartCameOrientation();
    }

    private void DoInteract(InputAction.CallbackContext ctx)
    {
        if (gameScript)
            return;

        if (target.transform.TryGetComponent<BaseGame>(out gameScript))
        {
            currentPos = cameraObj.transform.position;
            moveCamPosition = true;
            reverse = false;
            positionTimer = 0.0f;
        }
    }

    private void DoReturn(InputAction.CallbackContext ctx)
    {
        if (!gameScript)
            return;

        gameScript = null;
        RestartCameOrientation();
        currentPos = cameraObj.transform.position;
        moveCamPosition = true;
        reverse = true;
        positionTimer = 1.0f;
    }

    private void RestartCameOrientation()
    {
        lastOrientation = cameraObj.transform.rotation;
        target = neutralFocus.gameObject;
        orientationTimer = 0.0f;
        lastTarget = null;
    }
}