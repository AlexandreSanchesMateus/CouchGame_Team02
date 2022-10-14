using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class HackerController : MonoBehaviour
{
    public GameObject MiniGamescreens;
    
    public GameObject curentDisplayedScreen;
    public MiniGame focusedMinigame;
    public bool isInFrontOfMiniGame;
    
    public bool isFocused;
    
    public static HackerController instance;
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.yellow);
    }
    void Start()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Increment(InputAction.CallbackContext callback)
    {
        if(callback.started)
        {
        Debug.Log("Increment");
        MiniGamescreens.transform.Rotate(0, -MiniGamescreens.GetComponent<screensholder>().rotToAdd, 0);
        }


        //raycast catch gameobject

        


    }
    public void Decrement(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            Debug.Log("Increment");
            MiniGamescreens.transform.Rotate(0, MiniGamescreens.GetComponent<screensholder>().rotToAdd, 0);
        }

       
            
    }
    public void Interact(InputAction.CallbackContext callback)
    {
        Debug.Log("Interact");
        MiniGame mg;
        RaycastHit hit;
        Screen sc;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * 2, out hit))
        {
            if (hit.transform.tag=="MiniGame")
            {
                Debug.Log("is a game");
                    sc = hit.transform.GetComponent<Screen>();
                    mg = hit.transform.GetComponent<Screen>().game;
                    

                    if (mg.TestWin())
                    {
                        Debug.Log("win");
                    }
                    else
                    {
                        Debug.Log("nop");
                    }

            }
            else
            {
                Debug.Log("not game");
            }
        }
        else
        {
           
        }
        Debug.Log(hit.transform.tag);

    }
    public void Back(InputAction.CallbackContext callback)
    {
        Debug.Log("Back");
    }
    //rotate MiniGamescreens from 90 degrees on axe y


}
