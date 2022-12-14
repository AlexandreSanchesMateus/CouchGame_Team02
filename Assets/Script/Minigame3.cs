using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Minigame3 : MonoBehaviour , IMinigame
{
	private int key, lastKey;
	[SerializeField] private Material selected, notSelected;
	[SerializeField] private List<GameObject> buttons;
	public AudioClip SFXSelect, SFXHover;

	private bool haveInput = false;

	public void Start()
	{
		key = 0;
		lastKey = 0;
		foreach(GameObject g in buttons)
        {
			g.transform.GetComponent<MeshRenderer>().material = notSelected;
        }
	}

	public bool interact(InputAction.CallbackContext callback)
	{
		HackerController.instance.audioS.PlayOneShot(SFXSelect);
		buttons[lastKey].transform.GetComponent<MeshRenderer>().material = notSelected;
		buttons[key].transform.GetComponent<MeshRenderer>().material = selected;
		lastKey = key;

		ElementPad.instance.idKeyHacker = key;
		ElementPad.instance.hackHavePlayed = true;
		ElementPad.instance.CheckInput();
		return false;
	}

	public void Move(InputAction.CallbackContext callback)
	{
		if (haveInput && callback.ReadValue<Vector2>().magnitude < 0.5f) haveInput = false;

		if (haveInput || !callback.performed || callback.ReadValue<Vector2>().magnitude < 0.5f) return;
		HackerController.instance.audioS.PlayOneShot(SFXHover);
		haveInput = true;

		Sequence sequence = DOTween.Sequence();

		Vector2 val = callback.ReadValue<Vector2>().normalized;
		if (val.x != 0)
        {
			Debug.Log(val.x);
			if (val.x > 0)
			{
				sequence.Append(buttons[key].transform.DOScale(new Vector3(2f, 2f, 2f),0.4f));
				key = (key + 1 > 3)? key = 0 : ++key;
				sequence.Join(buttons[key].transform.DOScale(new Vector3(3f, 3f, 3f), 0.4f));
			}
			else if (val.x < 0)
			{
				sequence.Append(buttons[key].transform.DOScale(new Vector3(2f, 2f, 2f), 0.4f));
				key = (key - 1 < 0) ? key = 3 : --key;
				sequence.Join(buttons[key].transform.DOScale(new Vector3(3f, 3f, 3f), 0.4f));
			}
        }
	}
}
