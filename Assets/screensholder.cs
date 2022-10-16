using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class screensholder : MonoBehaviour
{
    //public MiniGame game;
    private float radius;
    public GameObject screenPrefabs;
    public int number;

    private float currentrot;
    private float newRot;
    [HideInInspector]public float rotToAdd;

    public List<GameObject> screens = new List<GameObject>();
    public List<MiniGame> minigames = new List<MiniGame>();
    public List<Material> scamAd = new List<Material>();
    public List<Material> MiniGamesMat = new List<Material>();
    private void Start()
    {
        
        radius = number*0.30f;
        rotToAdd = 360/number;
        newRot = 0;
        for (int i = 0; i < number; i++)
        {
            if (i == 0)
            {
                currentrot = 90;
            }
            else
            {
                currentrot -= rotToAdd;
            }
            float angle = i * Mathf.PI * 2f / number;
            Vector3 newPos = new Vector3(transform.position.x+ Mathf.Cos(angle) * radius, transform.position.y, transform.position.z + Mathf.Sin(angle) * radius);
            GameObject screenGO = Instantiate(screenPrefabs, newPos, Quaternion.Euler(0,currentrot,0), transform);
            if(i==0)
            {
                screenGO.GetComponent<Screen>().game = minigames[0];
            }
            else
            {
                
                screenGO.GetComponent<Renderer>().material = scamAd[Random.Range(0, scamAd.Count)];
            }
            screens.Add(screenGO);
        }
    }
    public void DoRotate(bool isposi)
    {
        if (isposi)
        {
            newRot += rotToAdd;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DORotate(new Vector3(0, newRot - rotToAdd  -7, 0), 0.1f));
            mySequence.Append(transform.DORotate(new Vector3(0, newRot, 0), 0.5f));
        }
        else
        {
            newRot -= rotToAdd;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DORotate(new Vector3(0, newRot + rotToAdd + 7, 0), 0.1f));
            mySequence.Append(transform.DORotate(new Vector3(0, newRot, 0), 0.5f));
        }
    }
}
        


