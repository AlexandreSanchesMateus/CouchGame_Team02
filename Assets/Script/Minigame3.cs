using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame3 : MonoBehaviour , IMinigame
{
	private int key;
	[SerializeField] private Material selected, notSelected;
	[SerializeField] private List<GameObject> buttons;

	public void Start()
	{
		key = 0;
		foreach(GameObject g in buttons)
        {
			g.transform.GetComponent<MeshRenderer>().material = notSelected;
        }
		buttons[key].transform.GetComponent<MeshRenderer>().material = selected;
	}

	public bool interact(InputAction.CallbackContext callback)
	{
		Debug.Log(key);
		ElementPad.instance.idKeyHacker = key;
		ElementPad.instance.hackHavePlayed = true;
		ElementPad.instance.CheckInput();
		return false;
	}

	public void Move(InputAction.CallbackContext callback)
	{
		Vector2 val = callback.ReadValue<Vector2>();

		if (callback.started && val.x != 0)
        {
			Debug.Log(val.x);
			if (val.x > 0)
			{
				Debug.Log("yes");
				buttons[key].transform.GetComponent<MeshRenderer>().material = notSelected;
				key = (key + 1 > 3)? key = 0 : ++key;
				buttons[key].transform.GetComponent<MeshRenderer>().material = selected;
			}
			else if (val.x < 0)
			{
				buttons[key].transform.GetComponent<MeshRenderer>().material = notSelected;
				key = (key - 1 < 0) ? key = 3 : --key;
				buttons[key].transform.GetComponent<MeshRenderer>().material = selected;
			}
        }

	}
}
