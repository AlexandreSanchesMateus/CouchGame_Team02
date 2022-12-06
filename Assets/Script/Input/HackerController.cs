using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Video;

public class HackerController : MonoBehaviour
{
	public GameObject MiniGamescreens;
	public screensholder scrHold;
	public float rotToAdd;
	public float currentRot;
	public static HackerController instance;
	public CinemachineVirtualCamera cam1, cam2;
	private RaycastHit hit;
	private Coroutine lastCorout;
	private Screen screen;

	private Transform originalCamTransform;

	public GameObject setUp, loadGame;
	public Material setupMaterial, loadMaterial;
	private bool locked;

	private void Start()
	{
		originalCamTransform = cam1.transform;

		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
		loadGame.transform.parent.GetComponentInChildren<VideoPlayer>().SetDirectAudioMute(0, true);
		rotToAdd = MiniGamescreens.GetComponent<screensholder>().rotToAdd;
		currentRot = 0;
		if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
		{
			screen = hit.transform.GetComponent<Screen>();
			screen.screenState = ScreenState.Setup;
			screen.transform.GetChild(0).GetComponent<MeshRenderer>().material = setupMaterial;
			screen.LockScreen();
			scrHold.TurnOnScreen(true, screen.transform);
			locked = true;
			screen.GetComponent<Screen>().DisplayCode();
		}
	}
	private void Update()
	{
		Debug.DrawRay(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, Color.yellow);


	}

	public void Increment(InputAction.CallbackContext callback)
	{
		if (screen.screenState == ScreenState.MiniGame)
		{
			if (callback.started)
			{
				//Debug.Log("Increment");
				MiniGamescreens.GetComponent<screensholder>().DoRotate(true);
			}

			StopCoroutine(lastCorout);
			lastCorout = StartCoroutine(popupDelay());
		}
	}
	public void Decrement(InputAction.CallbackContext callback)
	{
		if (screen.screenState == ScreenState.MiniGame)
		{
			if (callback.started)
			{
				//Debug.Log("decrement");
				MiniGamescreens.GetComponent<screensholder>().DoRotate(false);
			}

			StopCoroutine(lastCorout);
			lastCorout = StartCoroutine(popupDelay());
		}
	}

	public void Interact(InputAction.CallbackContext callback)
	{
		if (callback.started)
		{
			if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
			{
				screen = hit.transform.GetComponent<Screen>();
				switch (screen.screenState)
				{
					case ScreenState.MiniGame:

						screen.miniGame.GetComponent<IMinigame>().interact(callback);

						break;
					case ScreenState.Popups:

						if (callback.action.name == "West")
						{
							screen.FightPopup();

							if (screen.currentPopup.Count <= 0)
								lastCorout = StartCoroutine(popupDelay());
						}
						break;
					case ScreenState.Load:

						if (loadGame.GetComponent<IMinigame>().interact(callback))
						{
							loadGame.transform.parent.GetComponentInChildren<VideoPlayer>().SetDirectAudioMute(0, true);
							screen.transform.GetChild(0).GetComponent<MeshRenderer>().material = screen.gameMaterial;
							screen.miniGame = screen.game;
							GetComponentsInChildren<screensholder>()[0].TurnOnScreen(false, screen.transform);
							screen.screenState = ScreenState.MiniGame;
						}
						break;
					case ScreenState.Hack:
						break;
					case ScreenState.Setup:

						if (locked)
						{
							if (screen.UnlockScreen(callback))
							{
								GetComponentsInChildren<screensholder>()[0].TurnOnScreen(false, screen.transform);
								StartCoroutine(EndSetup(screen));
								lastCorout = StartCoroutine(popupDelay());
								StartCoroutine(loadDelay());
							}
						}
						break;
					default:
						break;
				}
			}
		}
	}
	//Input du joysitck
	public void MoveInScreen(InputAction.CallbackContext callback)
	{
		if (Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit))
		{
			screen = hit.transform.GetComponent<Screen>();

			if (screen.screenState == ScreenState.MiniGame)
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
		screen = hit.transform.GetComponent<Screen>();
		Debug.Log("screen = " + hit.transform.name);
		if (screen.screenState == ScreenState.MiniGame && screen.currentPopup.Count <= 0)
			screen.displayPopUp();
	}

	IEnumerator EndSetup(Screen scr)
	{
		locked = false;
		yield return new WaitForSeconds(2f);
		scr.transform.GetChild(0).GetComponent<MeshRenderer>().material = scr.gameMaterial;
		scr.screenState = ScreenState.MiniGame;
	}
	public void CamShake()
	{
		Sequence newSequence = DOTween.Sequence();
		newSequence.Append(cam1.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true));
		newSequence.Append(cam1.transform.DOMove(originalCamTransform.position, 0.2f));
	}

	IEnumerator loadDelay()
	{
		yield return new WaitForSeconds(4f);
		Physics.Raycast(transform.position, transform.TransformDirection(cam1.transform.forward) * 2, out hit);
		Screen scr = hit.transform.GetComponent<Screen>();
		Debug.Log("screen = " + hit.transform.name);
		if (scr.screenState == ScreenState.MiniGame)
		{
			while (!scrHold.CanRotate)
			{

			}
			scr.screenState = ScreenState.Load;
			scr.transform.GetChild(0).GetComponent<MeshRenderer>().material = loadMaterial;
			scr.miniGame = loadGame;
			scrHold.TurnOffScreen(scr.transform);
			loadGame.transform.parent.GetComponentInChildren<VideoPlayer>().SetDirectAudioMute(0, false);
		}
	}
}

