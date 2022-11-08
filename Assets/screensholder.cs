using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.Progress;
using System;
//using Unity.VisualScripting;

public class screensholder : MonoBehaviour
{
	public GameObject screenPrefabs;

	[Range(0, 20)]
	public int number;


	[HideInInspector] public float rotToAdd;

	public List<Transform> screens = new List<Transform>();
	//public List<MiniGame> minigames = new List<MiniGame>();
	public List<Material> scamAd = new List<Material>();
	private GameObject firstScreen = null;
	
	public bool CanRotate;
    
	public Vector3 frontScreen;

	private void Start()
	{
		CanRotate = true;
        
        foreach (Transform item in transform)
		{
			if (item.tag == "Screen")
			{
				screens.Add(item);
                if (firstScreen == null)
                {
					TurnOnScreen(true, item);

                    firstScreen = new GameObject("empty");
                    firstScreen.transform.position = item.transform.position;
                    firstScreen.transform.rotation = item.transform.rotation;
                }

            }
		}
		Debug.Log(screens.Count);


	}

	public void DoRotate(bool isposi)
	{
		if (CanRotate)
		{
			CanRotate = false;
			Sequence mySequence = DOTween.Sequence();
			if (isposi)
			{
				int next;
				for (int i = 0; i < screens.Count; i++)
				{
					next = i + 1;
					if (next > screens.Count - 1)
					{
						next = 0;
					}
					if (screens[next].position == firstScreen.transform.position)
					{
						Debug.Log("ayay");
						mySequence.Insert(0, screens[i].DORotate(new Vector3(-12, 189, 77), 0.2f));
						mySequence.Insert(0.2f, screens[i].DORotate(new Vector3(0, 189, 0), 0.3f).SetEase(Ease.OutBounce));

					}
					mySequence.Insert(0, screens[i].DOMove(screens[next].position, 0.4f).OnComplete(() => { CanRotate = true; }));
				}
			}
			else
			{
				int next;
				for (int i = 0; i < screens.Count; i++)
				{
					next = i - 1;
					if (next < 0)
					{
						next = screens.Count - 1;
					}
					if (screens[next].position == firstScreen.transform.position)
					{
						Debug.Log("ayay");
						mySequence.Insert(0, screens[i].DORotate(new Vector3(-12, 189, -77), 0.2f));
						mySequence.Insert(0.2f, screens[i].DORotate(new Vector3(0, 189, 0), 0.3f).SetEase(Ease.OutBounce));

					}
					mySequence.Insert(0, screens[i].DOMove(screens[next].position, 0.4f).OnComplete(() => { CanRotate = true; }));
				}
			}
		}
	}
	public void TurnOnScreen(bool onlyFirstScreen, Transform firstScreen)
    {
        Sequence turnONSequence = DOTween.Sequence();

		if (onlyFirstScreen)
		{
            
            turnONSequence.Append(firstScreen.transform.GetChild(0).DOScale(new Vector3(0, 0.2f, 1), 0f).OnComplete(() => { firstScreen.GetChild(0).gameObject.SetActive(true); }));
            turnONSequence.Append(firstScreen.transform.GetChild(0).DOScale(new Vector3(1, 0.2f, 1), 0.2f).SetEase(Ease.OutBounce));
            turnONSequence.Append(firstScreen.transform.GetChild(0).DOScale(new Vector3(1, 1f, 1), 0.2f).SetEase(Ease.OutBounce));
        }
		else
		{
			foreach (Transform item in transform)
			{
				if (item.tag == "Screen")
				{
                    if(item != firstScreen)
					{
                        turnONSequence.Append(item.transform.GetChild(0).DOScale(new Vector3(0, 0.2f, 1), 0f).OnComplete(() => { item.GetChild(0).gameObject.SetActive(true); }));
						turnONSequence.Append(item.transform.GetChild(0).DOScale(new Vector3(1, 0.2f, 1), 0.2f).SetEase(Ease.OutBounce));
						turnONSequence.Append(item.transform.GetChild(0).DOScale(new Vector3(1, 1f, 1), 0.2f).SetEase(Ease.OutBounce));
					}
				}
			}
		}
	}
}




