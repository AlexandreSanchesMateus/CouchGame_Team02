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

    private static List<int> code = new List<int>(new int[4] {1, 2, 3, 4 });
    private List<int> currentCode = new List<int>(new int[4] {0, 0, 0, 0 });
    private int codeIndex;

    public Material gameMaterial;
    public Material SetupMatrial;


    private void Start()
	{
        gameMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        codeIndex = 0;

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

    public bool UnlockScreen(InputAction.CallbackContext callback)
    {
        
        switch (callback.action.name)
        {
            case "West":
                currentCode[codeIndex] = 1;
                break;
            case "North":
                currentCode[codeIndex] = 2;
                break;
            case "East":
                currentCode[codeIndex] = 3;
                break;
            case "South":
                currentCode[codeIndex] = 4;
                break;

            default:
                break;
        }
        codeIndex++;
        DisplayCode();
        if (codeIndex >= 4)
        {
            codeIndex = 0;
            if (code.SequenceEqual(currentCode))
            {
                Debug.Log("screen unlocked");
                transform.GetChild(0).GetComponent<MeshRenderer>().material = gameMaterial;
                return true;
            }
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

        HackerController.instance.setUp.transform.GetComponent<TextMeshPro>().text = toDisplay;
    }
}
