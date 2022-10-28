using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.Progress;
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
	public List<Transform> screenPos = new List<Transform>();
	
	public bool CanRotate;

	public Vector3 frontScreen;

	private void Start()
	{
		CanRotate = true;
        Sequence turnONSequence = DOTween.Sequence();
        foreach (Transform item in transform)
		{
			if (item.tag == "Screen")
			{
				screens.Add(item);
				TurnOnScreen(turnONSequence,item);
				GameObject go = new GameObject("empty");
				go.transform.position = item.transform.position;
				go.transform.rotation = item.transform.rotation;
				screenPos.Add(go.transform);
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
					if (screens[next].position == screenPos[0].position)
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
					if (screens[next].position == screenPos[0].position)
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
	public void TurnOnScreen(Sequence turnONSequence, Transform item)
	{
        
        turnONSequence.Insert(0, item.transform.GetChild(0).DOScale(new Vector3(0, 0.2f, 1), 0f));
		turnONSequence.Append(item.transform.GetChild(0).DOScale(new Vector3(1, 0.2f, 1), 0.2f).SetEase(Ease.OutBounce));
		turnONSequence.Append(item.transform.GetChild(0).DOScale(new Vector3(1, 1f, 1), 0.2f).SetEase(Ease.OutBounce));
	}
}




