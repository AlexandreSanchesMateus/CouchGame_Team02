using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCode : MonoBehaviour, IInteractible
{

    [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Keypad;


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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
