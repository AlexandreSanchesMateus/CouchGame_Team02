using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using DG.Tweening;
using Cinemachine;
public class HackerController : MonoBehaviour
{
    public GameObject MiniGamescreens;
    public float rotToAdd;
    public float currentRot;
    public static HackerController instance;
    public CinemachineVirtualCamera cam1,cam2;
    private void Start()
    {
        rotToAdd = MiniGamescreens.GetComponent<screensholder>().rotToAdd;
        currentRot = 0;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, Color.yellow);
    }
    
    public void Increment(InputAction.CallbackContext callback)
    {
        if(callback.started)
        {
        Debug.Log("Increment");
            MiniGamescreens.GetComponent<screensholder>().DoRotate(true);

            //MiniGamescreens.transform.Rotate(0, MiniGamescreens.GetComponent<screensholder>().rotToAdd, 0);
        }
        
    }
    public void Decrement(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            Debug.Log("decrement");
            MiniGamescreens.GetComponent<screensholder>().DoRotate(false);
        } 
    }
    public void Interact(InputAction.CallbackContext callback)
    {
        Debug.Log("Interact");
        MiniGame mg;
        RaycastHit hit;
        Screen sc;
        if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
        {
            if (hit.transform.tag=="MiniGame")
            {
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

    }
    public void SwitchCam(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if (cam1.Priority == 10)
            {
                cam1.Priority = 0;
                cam2.Priority = 10;
            }
            else
            {
                cam1.Priority = 10;
                cam2.Priority = 0;
            }
        }
    }
    public void Back(InputAction.CallbackContext callback)
    {
        Debug.Log("Back");
    }
    //rotate MiniGamescreens from 90 degrees on axe y


}
