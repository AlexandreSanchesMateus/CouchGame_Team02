using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame2 : MonoBehaviour,IMinigame
{
	private float stickRotationInDeg;
	private GameObject color, selectedColor;
	public AudioClip SFXSelect, SFXHover;

	public bool interact(InputAction.CallbackContext ctx)
	{
		if(selectedColor != null)
        {
			int id = 0;
            switch (selectedColor.GetComponent<SimonColor>().color)
            {
				case SimonColor.CubeColor.Cyan:
					id = 4;
					break;
				case SimonColor.CubeColor.Blue:
					id = 3;
					break;
				case SimonColor.CubeColor.Yellow:
					id = 0;
					break;
				case SimonColor.CubeColor.Red:
					id = 1;
					break;
				case SimonColor.CubeColor.Magenta:
					id = 2;
					break;
				case SimonColor.CubeColor.Green:
					id = 5;
					break;
			}

			HackerController.instance.audioS.PlayOneShot(SFXSelect);

			Debug.Log("hackerId " + SimonPad.instance.hackeurId);
			Debug.Log("hackerPlay " + SimonPad.instance.hackeurPlay);
			SimonPad.instance.hackeurId = id;
            SimonPad.instance.hackeurPlay = true;
            SimonPad.instance.CheckPlayerEntry();
			Debug.Log("hackerId " + SimonPad.instance.hackeurId);
			Debug.Log("hackerPlay " + SimonPad.instance.hackeurPlay);
		}

		return false;
		
	}

	public void Move(InputAction.CallbackContext ctx)
	{
		Vector2 value = ctx.ReadValue<Vector2>().normalized;

		stickRotationInDeg = Mathf.Rad2Deg * Mathf.Atan2(-value.x, value.y);
		if (value.x > 0.5 || value.x < -0.5 || value.y > 0.5 || value.y < -0.5)
		{
			transform.rotation = Quaternion.Euler(0, 0, stickRotationInDeg);
		}
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.up * 3, out hit))
        {
			if(hit.transform.tag == "MiniGame2")
            {
				color = hit.transform.gameObject;
				if(selectedColor != color)
                {
					HackerController.instance.audioS.PlayOneShot(SFXHover);
					selectedColor = color;
				}
            }
        }
	}
}
