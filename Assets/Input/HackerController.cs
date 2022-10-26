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
	private bool canInteract=true;
	public static HackerController instance;
	public CinemachineVirtualCamera cam1,cam2;
	private RaycastHit hit;
	private Coroutine lastCorout;



	private void Start()
	{
		if(instance == null)
        {
			instance = this;
        }
        else
        {
			Destroy(this);
        }

		rotToAdd = MiniGamescreens.GetComponent<screensholder>().rotToAdd;
		currentRot = 0;

		lastCorout = StartCoroutine(popupDelay());
    }
	private void Update()
	{
		Debug.DrawRay(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, Color.yellow);
		//if (popupCoFinished)
		//{
		//	popupCoFinished = false;
		//	StartCoroutine(popupDelay());
		//}

	}
	
	public void Increment(InputAction.CallbackContext callback)
	{
		if(callback.started&&canInteract)
		{
		//Debug.Log("Increment");
			MiniGamescreens.GetComponent<screensholder>().DoRotate(true);
        }

		StopCoroutine(lastCorout);
		lastCorout = StartCoroutine(popupDelay());
	}
	public void Decrement(InputAction.CallbackContext callback)
	{
		if (callback.started&&canInteract)
		{
			//Debug.Log("decrement");
			MiniGamescreens.GetComponent<screensholder>().DoRotate(false);
        }

		StopCoroutine(lastCorout);
		lastCorout = StartCoroutine(popupDelay());
	}
	public void Interact(InputAction.CallbackContext callback)
	{
		if (callback.started)
		{
			if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
			{
				Screen screen = hit.transform.GetComponent<Screen>();

				if (screen.screenState == ScreenState.Popups)
                {
					screen.FightPopup();

					if (screen.currentPopup.Count <= 0)
						lastCorout = StartCoroutine(popupDelay());
                }
				else if(screen.screenState == ScreenState.MiniGame)
				{
					screen.miniGame.GetComponent<IMinigame>().interact(callback);

                }
			}
		}
	}
	public void MoveInScreen(InputAction.CallbackContext callback)
	{
        if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
        {
            Screen screen = hit.transform.GetComponent<Screen>();

            if (screen.screenState == ScreenState.Popups)
            {
                screen.FightPopup();

                if (screen.currentPopup.Count <= 0)
                    lastCorout = StartCoroutine(popupDelay());
            }
            else if (screen.screenState == ScreenState.MiniGame)
            {
                screen.miniGame.GetComponent<IMinigame>().Move(callback);

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
    //IEnumerator Wait()
    //{
    //	canInteract = false;
    //	yield return new WaitForSeconds(2);
    //	canInteract = true;
    //	Debug.Log("Stuck in ads");
    //}

    IEnumerator popupDelay()
    {
		yield return new WaitForSeconds(Random.Range(5f, 10f));

		Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit);
		Screen scr = hit.transform.GetComponent<Screen>();

		if (scr.screenState != ScreenState.Popups && scr.currentPopup.Count <= 0)
			scr.displayPopUp();
	}

}
