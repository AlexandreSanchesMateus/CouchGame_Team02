using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
public class Screen : MonoBehaviour
{
	public MiniGame game;
	public List<GameObject> popups;
    private List<GameObject> currentpopup = new List<GameObject>();
    private int currentPopupLife;
	
	private void Start()
	{
		if (game == null)
		{
			gameObject.tag = "ads";  
		}
		else
		{
			gameObject.tag = "MiniGame";
		}
        currentPopupLife = Random.Range(1, 3);
    }
	
	public void displayPopUp()
	{
		List<GameObject> popupsRandom =popups;
		
		int n = Random.Range(2,4);
		for (int j = 0; j < n; j++)
		{
            Debug.Log(popupsRandom.Count);

            if (popupsRandom.Count >= 1)
			{
				Debug.Log("ça rentre?");
                int i = Random.Range(0, popupsRandom.Count - 1);
                popupsRandom[i].SetActive(true);
                //Debug.Log("i = " + i);
                //         Debug.Log("popupsRandom[i] = " + popupsRandom[i].name);
                currentpopup.Add(popupsRandom[i]);
                popupsRandom.RemoveAt(i);
            }
            


		}
	}
	public bool FightPopup()
	{
		if (currentpopup.Count > 0)
		{
			currentPopupLife--;
			if (currentPopupLife <= 0)
			{
				currentpopup[0].SetActive(false);
				currentpopup.Remove(currentpopup[0]);
				currentPopupLife = Random.Range(1, 3);
			}
			return true;
		}
		return false;
	}
}
