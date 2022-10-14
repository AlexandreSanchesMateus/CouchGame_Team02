using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HackerController : MonoBehaviour
{
    private PlayerInput hackerInput;

    [SerializeField] private GameObject[] UIPage;
    private bool canUse = false;
    private int lastUIid = 0;
    private int currentUIID = 0;

    private void Awake()
    {
        hackerInput = new PlayerInput();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!(UIPage.Length > 0))
            return;

        for (int i = 1; i < UIPage.Length; i++)
           UIPage[i].SetActive(false);

        canUse = true;
        UIPage[0].SetActive(true);
    }

    private void OnEnable()
    {
        hackerInput.HackerController.WindowIncrement.performed += DoIncrement;
        hackerInput.HackerController.WindowIncrement.Enable();
    }

    private void DoIncrement(InputAction.CallbackContext ctx)
    {
        if (!canUse)
            return;

        UIPage[lastUIid].SetActive(false);
        currentUIID = (currentUIID + (int)ctx.ReadValue<float>()) % UIPage.Length;
        if (currentUIID < 0)
            currentUIID = UIPage.Length - 1;
        UIPage[currentUIID].SetActive(true);
        lastUIid = currentUIID;
    }
}
