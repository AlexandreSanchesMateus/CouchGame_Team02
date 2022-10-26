using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public enum ScreenState
{
	MiniGame,
	Popups,
	Update
};

public class Screen : MonoBehaviour
{
	public ScreenState screenState;
    [Header("r�f�rence vers le game Object du mini jeu")]
    public GameObject miniGame;
    public Transform oldpos;
    public Transform newpos;

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
                //Debug.Log("�a rentre?");
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
				HackerController.instance.CamShake();
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

}
