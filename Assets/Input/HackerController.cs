using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using DG.Tweening;
using Cinemachine;

public enum GameState
{
	MiniGame,
	Popups,
	Update
};

public class HackerController : MonoBehaviour
{
	private GameState gameState;
	private bool popupIsActive = false;
	public GameObject MiniGamescreens;
	public float rotToAdd;
	public float currentRot;
	private bool canInteract=true;
	public static HackerController instance;
	public CinemachineVirtualCamera cam1,cam2;
	private bool popupCoFinished = true;
	private RaycastHit hit;
	private Coroutine lastCorout;
	private void Start()
	{
		rotToAdd = MiniGamescreens.GetComponent<screensholder>().rotToAdd;
		currentRot = 0;

		Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit);
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
			StopCoroutine(lastCorout);
            popupCoFinished = true;
            if (lastCorout != null)
            {
                Debug.Log("last "+lastCorout);
            }
            
            lastCorout = StartCoroutine(popupDelay());
        }
		

    }
	public void Decrement(InputAction.CallbackContext callback)
	{
		if (callback.started&&canInteract)
		{
			//Debug.Log("decrement");
			MiniGamescreens.GetComponent<screensholder>().DoRotate(false);
			StopCoroutine(lastCorout);
            popupCoFinished = true;
            lastCorout = StartCoroutine(popupDelay());
        }
		

    }
	public void Interact(InputAction.CallbackContext callback)
	{
		if (callback.started)
		{
			MiniGame mg;
			if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
			{
				if (gameState == GameState.MiniGame)
				{
                    lastCorout = StartCoroutine(popupDelay());
                }
				else if (gameState == GameState.Popups)
				{
					popupIsActive = hit.transform.GetComponent<Screen>().FightPopup();
					if (!popupIsActive)
					{
						Debug.Log("startcour by space");
						gameState = GameState.MiniGame;
						canInteract = true;
                        lastCorout = StartCoroutine(popupDelay());
                    }

                }
                
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
		if (popupCoFinished)
		{
			popupCoFinished = false;
			RaycastHit courouhit;
			yield return new WaitForSeconds(0.6f);
			Debug.Log("cour 1");
			if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out courouhit))
			{
				Debug.Log("courou start");
				//Debug.Log("cour 2");
				//Debug.Log("name= "+courouhit.transform.name);

				yield return new WaitForSeconds(Random.Range(2, 5));
				canInteract = false;
				hit.transform.GetComponent<Screen>().displayPopUp();
				popupIsActive = true;
				gameState = GameState.Popups;
				Debug.Log("courou finishe");
			}
		}
		else
			yield return null;


        popupCoFinished = true;
    }


}
