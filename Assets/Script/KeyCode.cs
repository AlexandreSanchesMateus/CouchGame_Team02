using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyCode : MonoBehaviour, IInteractible
{
    [Header("Canvas")]
    [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Keypad;
    [SerializeField] private TextMeshProUGUI displayCode;
    [SerializeField] private GameObject[] keys;

    private int idKey;
    private bool isOpen;


    void Start()
    {
        isOpen = false;
        idKey = 0;
        GUIhover.SetActive(false);
        Keypad.SetActive(false);
    }

    public void OnActions(Vector2 action)
    {
        Debug.Log(action);
    }

    public void OnItemExit()
    {
        GUIhover.SetActive(false);
    }

    public void OnItemHover()
    {
        GUIhover.SetActive(true);
    }

    public void OnIteract()
    {        
        GUIhover.SetActive(false);
        Keypad.SetActive(true);
    }

    public void OnReturn()
    {
        isOpen = false;
        idKey = 0;
        GUIhover.SetActive(false);
        Keypad.SetActive(false);
    }
}
