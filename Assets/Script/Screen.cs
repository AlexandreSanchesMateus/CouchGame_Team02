using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using TMPro;

public enum ScreenState
{
	MiniGame,
	Popups,
	Update,
	Hack,
	Setup
};

public class Screen : MonoBehaviour
{
	public ScreenState screenState;
	[Header("référence vers le game Object du mini jeu")]
	public GameObject miniGame;
	/*public Transform oldpos;
	public Transform newpos;*/

	//public IMinigame minigameInterface;

	public List<GameObject> popups;
	public List<GameObject> currentPopup = new List<GameObject>();
	private int currentPopupLife;
	public bool focus = false;
	Sequence mySequence;

	private List<int> firstPossibility = new List<int>(new int[4] {127, 192, 169, 198 });
	private List<int> code = new List<int>();

	private List<string> codeToUnlock = new List<string>();
	private List<string> currentCode = new List<string>();
	private int codeIndex;

	public Material gameMaterial;
	public Material SetupMatrial;


	private void Start()
	{
		gameMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;


		transform.GetChild(0).localScale = new Vector3(transform.GetChild(0).localScale.x, 0f, transform.GetChild(0).localScale.z);
		transform.GetChild(0).gameObject.SetActive(false);

		currentPopupLife = Random.Range(1, 3);

		screenState = ScreenState.MiniGame;
	}


	public void displayPopUp()
	{
		List<GameObject> popupsRandom = new List<GameObject>(popups);
		//Debug.Log("displaying popup");
		int n = Random.Range(3,7);
		mySequence = DOTween.Sequence();
		for (int j = 0; j < n; j++)
		{
				int i = Random.Range(0, popupsRandom.Count - 1);
				popupsRandom[i].SetActive(true);
				
				mySequence.Insert(0,popups[i].transform.DOScale(new Vector3(0, 0, 0), 0.1f));
				float scaleX = Random.Range(0.03f, 0.1f);
				mySequence.Append(popups[i].transform.DOScale(new Vector3(scaleX, 1, 0.01f), 0.2f).SetEase(Ease.OutBounce));
				mySequence.Append(popups[i].transform.DOScale(new Vector3(scaleX, 1, Random.Range(0.04f, 0.1f)), 0.2f).SetEase(Ease.OutBounce));
				if (popups[i].transform.localScale.x == 0)
				{
				
					switch(Random.Range(0, 3))
						{
						case 0:
						mySequence.Append(popups[i].transform.DOScale(new Vector3(0.05f, 1, 0.03f), 0.2f).SetEase(Ease.OutBounce));
						break;
						case 1:
						mySequence.Append(popups[i].transform.DOScale(new Vector3(0.02f, 1, 0.04f), 0.2f).SetEase(Ease.OutBounce));
						break;
						case 2:
						mySequence.Append(popups[i].transform.DOScale(new Vector3(0.03f, 1, 0.05f), 0.2f).SetEase(Ease.OutBounce));
						break;
						case 3:
						mySequence.Append(popups[i].transform.DOScale(new Vector3(0.1f, 1, 0.04f), 0.2f).SetEase(Ease.OutBounce));
						break;
					}
				}
				
			  
				//Debug.Log("i = " + i);
				//         Debug.Log("popupsRandom[i] = " + popupsRandom[i].name);
				currentPopup.Add(popupsRandom[i]);
				popupsRandom.RemoveAt(i);
				HackerController.instance.CamShake();
			
		}
		
		screenState = ScreenState.Popups;
	}
	public bool FightPopup()
	{

		if (currentPopup.Count > 0)
		{

			currentPopupLife--;
			if (currentPopupLife <= 0)
			{
				StartCoroutine(destroyPopup(currentPopup[0]));
				currentPopup.RemoveAt(0);
				currentPopupLife = Random.Range(1, 3);
			}
			if (currentPopup.Count > 0)
			{
				return true;
			}
		}
		screenState = ScreenState.MiniGame;
		return false;
	}
	IEnumerator destroyPopup(GameObject popup)
	{
		mySequence = DOTween.Sequence();
		mySequence.Append(popup.transform.DOScale(new Vector3(0, 1, 0), 0.2f).SetEase(Ease.OutBounce));
		//mySequence.Append(popup.transform.DOScale(new Vector3(scaleX, 1, Random.Range(0.04f, 0.1f)), 0.2f).SetEase(Ease.OutBounce));
		yield return new WaitForSeconds(0.2f);
		popup.SetActive(false);
	}

	public void LockScreen()
	{
		codeToUnlock = new List<string>();
		code = new List<int>();
		code.Add(firstPossibility[Random.Range(0, 3)]);
		for(int i = 0; i<3; i++)
		{
			code.Add(Random.Range(0, 254));
		}

		Debug.Log(code[0] + " " + code[1] + " " + code[2] + " " + code[3]);
		codeToUnlock = TranslateCode(code);
		Debug.Log(codeToUnlock[0]+" "+ codeToUnlock[1] + " " + codeToUnlock[2] + " " + codeToUnlock[3]);
		currentCode = new List<string>();
		codeIndex = 0;
	}

	public bool UnlockScreen(InputAction.CallbackContext callback)
	{
		
		switch (callback.action.name)
		{
			case "West":
				currentCode.Add("X");
				break;
			case "North":
				currentCode.Add("Y");
				break;
			case "East":
				currentCode.Add("B");
				break;
			case "South":
				currentCode.Add("A");
				break;

			default:
				break;
		}

		codeIndex++;
		DisplayCode();
		if (codeIndex >= 4)
		{
			codeIndex = 0;
			if (codeToUnlock.SequenceEqual(currentCode))
			{
				Debug.Log("screen unlocked");
				transform.GetChild(0).GetComponent<MeshRenderer>().material = gameMaterial;
				return true;
			}
			currentCode = new List<string>();
			DisplayCode();
		}
		return false;
	}

	public void DisplayCode()
	{
		string toDisplay = "";

		for (int j = 0; j < 4 - codeIndex; j++)
		{
			toDisplay += "_ ";
		}

		for (int j = 0; j < codeIndex; j++)
		{
			toDisplay += currentCode[j];
			toDisplay += " ";
		}

		HackerController.instance.setUp.transform.GetComponentInChildren<TextMeshPro>().text = toDisplay;
	}

	public List<string> TranslateCode(List<int> codeToTrans)
    {
		List<string> translatedCode = new List<string>();

		for(int i = 0; i<4; i++)
        {

            switch (i)
            {
				case 0:
                    switch (codeToTrans[0])
                    {
						case 127:
							translatedCode.Add("A");
							break;
						case 192:
							translatedCode.Add("B");
							break;
						case 169:
							translatedCode.Add("X");
							break;
						case 198:
							translatedCode.Add("Y");
							break;
					}
					break;

				case 1:
					if(codeToTrans[1]/100 >= 1)
                    {
						translatedCode.Add("X");
                    }
					else if (codeToTrans[1] / 10 >= 1)
                    {
						translatedCode.Add("B");
					}
					else
					{
						translatedCode.Add("A");
					}
					break;

				case 2:
					int firstDigit;
					if(codeToTrans[2] / 100 >= 1)
						firstDigit = (codeToTrans[2] / 100) % 10;
					else if(codeToTrans[2] / 10 >= 1)
						firstDigit = (codeToTrans[2] / 10) % 10;
					else
						firstDigit = codeToTrans[2] % 10;

					if (firstDigit == 0 && codeToTrans[2] % 2 == 0)
                    {
						translatedCode.Add("A");
					}
					else if (firstDigit != 0 && codeToTrans[2] % 2 != 0)
					{
						translatedCode.Add("B");
					}
					else if (firstDigit == 0 && codeToTrans[2] % 2 != 0)
					{
						translatedCode.Add("X");
					}
					else if (firstDigit != 0 && codeToTrans[2] % 2 == 0)
					{
						translatedCode.Add("Y");
					}
					break;

				case 3:
					int somme = codeToTrans[3] % 10 + (codeToTrans[3]/10) % 10 + (codeToTrans[3] / 100) % 10;

					if(somme<= 5)
                    {
						translatedCode.Add("A");
					}
					else if (somme >= 16)
					{
						translatedCode.Add("B");
					}
					else if(somme%2 == 0)
					{
						translatedCode.Add("X");
					}
					else if(somme % 2 != 0)
					{
						translatedCode.Add("Y");
					}
					break;
			}
        }
		return translatedCode;
    }
}
