using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Unity.VisualScripting;

public class screensholder : MonoBehaviour
{
    //public MiniGame game;
    private float radius;
    public GameObject screenPrefabs;
    private float currentrot;
    private float newRot;

    [Range(0, 20)]
    public int number;


    [HideInInspector] public float rotToAdd;

    public List<Transform> screens = new List<Transform>();
    //public List<MiniGame> minigames = new List<MiniGame>();
    public List<Material> scamAd = new List<Material>();
    private List<Transform> screenPos = new List<Transform>();

    public bool CanRotate;
    
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
        if(CanRotate)
        {
            if (isposi)
            {
                screens = new List<Transform>();
                screenPos = new List<Transform>();
                Sequence mySequence = DOTween.Sequence();
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
                for (int i = 0; i < screens.Count; i++)
                {
                    Debug.Log("i " + i);
                    if ((i + 1) > (screens.Count - 1))
                    {
                        

                        mySequence.Append(screens[i].DOMove(screenPos[0].position, 0.4f).SetEase(Ease.OutBounce));
                        mySequence.Append(screens[i].DORotate(screenPos[0].rotation.eulerAngles, 0.2f).OnComplete(() => { CanRotate = true; }));
                        Debug.Log("oui");
                        Debug.Log("dans oui  " + i);
                    }
                    else
                    {
                        mySequence.Append(screens[i].DOMove(screenPos[i + 1].position, 0.4f).SetEase(Ease.OutBounce));
                        mySequence.Append(screens[i].DORotate(screenPos[0].rotation.eulerAngles, 0.2f).OnComplete(() => { CanRotate = true; }));
                    }
                }
                screens.Clear();
                screenPos.Clear();
            }
            else
            {
                screens = new List<Transform>();
                screenPos = new List<Transform>();
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
                for (int i = 0; i < screens.Count; i++)
                {
                    Debug.Log("i " + i);
                    if ((i - 1) < 0)
                    {
                        screens[i].DOMove(screenPos[screens.Count - 1].position, 0.4f).SetEase(Ease.OutBounce);
                        screens[i].DORotate(screenPos[screens.Count - 1].rotation.eulerAngles, 0.2f);
                    }
                    else
                    {


                        screens[i].DOMove(screenPos[i - 1].position, 0.4f).SetEase(Ease.OutBounce);
                        screens[i].DORotate(screenPos[i - 1].rotation.eulerAngles, 0.2f);
                    }
                }
                screens.Clear();
                screenPos.Clear();
            }
        }
        
        
    }
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



