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
	private void Start()
	{
		rotToAdd = MiniGamescreens.GetComponent<screensholder>().rotToAdd;
		currentRot = 0;

		Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit);
		   
	}
	private void Update()
	{
		Debug.DrawRay(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, Color.yellow);
		if (popupCoFinished)
		{
			popupCoFinished = false;
			StartCoroutine(popupDelay(hit));
		}

	}
	
	public void Increment(InputAction.CallbackContext callback)
	{
		if(callback.started&&canInteract)
		{
		//Debug.Log("Increment");
			MiniGamescreens.GetComponent<screensholder>().DoRotate(true);
		}
		
	}
	public void Decrement(InputAction.CallbackContext callback)
	{
		if (callback.started&&canInteract)
		{
			//Debug.Log("decrement");
			MiniGamescreens.GetComponent<screensholder>().DoRotate(false);
		} 
	}
	public void Interact(InputAction.CallbackContext callback)
	{
		if (callback.started)
		{
			MiniGame mg;
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
			{
				if (gameState == GameState.MiniGame)
				{

					//	if (hit.transform.tag=="MiniGame"&&canInteract)
					//	{

					//		//mg = hit.transform.GetComponent<Screen>().FightPopup();


					//		//if (mg.TestWin())
					//		//    {
					//		//        Debug.Log("win");
					//		//    }
					//		//    else
					//		//    {
					//		//        Debug.Log("nop");
					//		//    }
					//	}
					//	else
					//	{
					//		Debug.Log("not game");
					//		StartCoroutine(Wait());
					//	}
				}
				else if (gameState == GameState.Popups)
				{
					if (popupIsActive)
						popupIsActive = hit.transform.GetComponent<Screen>().FightPopup();
					else
						gameState = GameState.MiniGame;
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
	IEnumerator Wait()
	{
		canInteract = false;
		yield return new WaitForSeconds(2);
		canInteract = true;
		Debug.Log("Stuck in ads");
	}
	
	IEnumerator popupDelay(RaycastHit hit)
	{
		yield return new WaitForSeconds(Random.Range(0,1));
		hit.transform.GetComponent<Screen>().displayPopUp();
		popupIsActive = true;
		gameState = GameState.Popups;
		popupCoFinished = true;
	}


}
