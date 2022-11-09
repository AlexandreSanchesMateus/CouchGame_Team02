using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
		foreach (Transform item in transform)
		{
			if (item.tag == "Screen")
			{
				screens.Add(item);
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

    /*public void DoRotate(bool isposi)
	{
		if(CanRotate)
		{
			CanRotate = false;
			Sequence mySequence = DOTween.Sequence();
			if (isposi)
			{
				int next;
                for (int i = 0; i < screens.Count; i++)
				{
                    next = i + 1;
                    Debug.Log("i " + i);
                    Debug.Log("next " + next);
                    if (next > screens.Count - 1)
                    {
                        Debug.Log("putain de merde là c'est zero non,");
                        next = 0;
                        Debug.Log("next doi etre a 0 et la il est a : " + next);
                    }
                    
                    mySequence.Insert(0.4f, screens[i].DOMove(screenPos[next].position, 0.4f).SetEase(Ease.OutBounce).OnComplete(() => { CanRotate = true; }));
                    
					*//*if (i == lastScreenIndex)
					{
                        
                        

                        //mySequence.Insert(0, screens[i].DOMove(new Vector3(screens[i].position.x, screens[i].position.y + 0.5f, screens[i].position.z), 0.4f).SetEase(Ease.OutBounce));
                        //vrais déplacement

						//rotation
						*//*mySequence.Join(screens[i].DORotate(screenPos[next].rotation.eulerAngles, 0.2f));
						mySequence.Join(screens[i].DORotate(new Vector3(screens[i].transform.rotation.x, 189, screens[i].transform.rotation.z + 40), 0.4f).SetEase(Ease.OutBounce));
						mySequence.Append(screens[i].DORotate(new Vector3(screens[i].transform.rotation.x, 189, 0), 0.2f).SetEase(Ease.OutBounce));*//*
					}
					else
					{
						//mySequence.Insert(0,screens[i].DOMove(new Vector3(screens[i].position.x, screens[i].position.y + 0.5f, screens[i].position.z), 0.4f).SetEase(Ease.OutBounce));
						
						mySequence.Insert(0.4f,screens[i].DOMove(screenPos[next].position, 0.4f).SetEase(Ease.OutBounce).OnComplete(() => { CanRotate = true; }));
						//mySequence.Join(screens[i].DORotate(screenPos[next].rotation.eulerAngles, 0.2f));
					}
					//mySequence.Join(screens[i].DORotate(new Vector3(screens[i].transform.rotation.x, 189, screens[i].transform.rotation.z+40),0.4f).SetEase(Ease.OutBounce));
					//mySequence.Append(screens[i].DORotate(new Vector3(screens[i].transform.rotation.x, 189, 0), 0.2f).SetEase(Ease.OutBounce));*//*
				}
				if (lastScreenIndex == 4)
				{
					lastScreenIndex = 0;
					Debug.Log("why do this?");
				}
				else
				{
					lastScreenIndex--;
					Debug.Log("good boy");
				}
			}
			//else
			//{
			//	screens = new List<Transform>();
			//	screenPos = new List<Transform>();
			//	foreach (Transform item in transform)
			//	{
			//		if (item.tag == "Screen")
			//		{
			//			screens.Add(item);
			//			GameObject go = new GameObject("empty");
			//			go.transform.position = item.transform.position;
			//			go.transform.rotation = item.transform.rotation;
			//			screenPos.Add(go.transform);
			//		}
			//	}
			//	for (int i = 0; i < screens.Count; i++)
			//	{
			//		Debug.Log("i " + i);
			//		if ((i - 1) < 0)
			//		{
			//			screens[i].DOMove(screenPos[screens.Count - 1].position, 0.4f).SetEase(Ease.OutBounce);
			//			screens[i].DORotate(screenPos[screens.Count - 1].rotation.eulerAngles, 0.2f);
			//		}
			//		else
			//		{


			//			screens[i].DOMove(screenPos[i - 1].position, 0.4f).SetEase(Ease.OutBounce);
			//			screens[i].DORotate(screenPos[i - 1].rotation.eulerAngles, 0.2f);
			//		}
			//	}
			//	screens.Clear();
			//	screenPos.Clear();
			//}
		}
		
		
	}*/
}
	//public void DoRotate(bool isposi)
	//{
	//    if (isposi)
	//    {
	//        newRot += rotToAdd;
	//        Sequence mySequence = DOTween.Sequence();
	//        mySequence.Append(transform.DORotate(new Vector3(0, newRot - rotToAdd  -7, 0), 0.1f));
	//        mySequence.Append(transform.DORotate(new Vector3(0, newRot, 0), 0.5f).SetEase(Ease.OutBounce));
	//    }
	//    else
	//    {
	//        newRot -= rotToAdd;
	//        Sequence mySequence = DOTween.Sequence();
	//        mySequence.Append(transform.DORotate(new Vector3(0, newRot + rotToAdd + 7, 0), 0.1f));
	//        mySequence.Append(transform.DORotate(new Vector3(0, newRot, 0), 0.5f).SetEase(Ease.OutBounce));
	//    }
	//}



