using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public enum ScreenState
{
	MiniGame,
	Popups,
	Update
};

public class Screen : MonoBehaviour
{
	public ScreenState screenState;
    [Header("référence vers le game Object du mini jeu")]
    public GameObject miniGame;

    
    //public IMinigame minigameInterface;
    
	public List<GameObject> popups;
    public List<GameObject> currentPopup = new List<GameObject>();
    private int currentPopupLife;
	public bool focus = false;
	Sequence mySequence;
    private void Start()
	{
        
        
        currentPopupLife = Random.Range(1, 3);

		screenState = ScreenState.MiniGame;
    }
	
	public void displayPopUp()
	{
		List<GameObject> popupsRandom = new List<GameObject>(popups);
        //Debug.Log("displaying popup");
        int n = Random.Range(2,4);
        mySequence = DOTween.Sequence();
        for (int j = 0; j < n; j++)
		{
			//Debug.Log(popupsRandom.Count);
			//Debug.Log("dans le for de display");
            if (popupsRandom.Count >= 1)
			{
                //Debug.Log("dans la condition qui display");
                //Debug.Log("ça rentre?");
                int i = Random.Range(0, popupsRandom.Count - 1);
                popupsRandom[i].SetActive(true);
                
                popups[i].transform.localScale = new Vector3(0, 0, 0);
				float scaleX = Random.Range(0.03f, 0.1f);
                mySequence.Append(popups[i].transform.DOScale(new Vector3(scaleX, 1,0.01f), 0.2f).SetEase(Ease.OutBounce));
				mySequence.Append(popups[i].transform.DOScale(new Vector3(scaleX, 1, Random.Range(0.04f, 0.1f)), 0.2f).SetEase(Ease.OutBounce));

                //Debug.Log("i = " + i);
                //         Debug.Log("popupsRandom[i] = " + popupsRandom[i].name);
                currentPopup.Add(popupsRandom[i]);
                popupsRandom.RemoveAt(i);
            }
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
				currentPopup[0].SetActive(false);
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

	
}
