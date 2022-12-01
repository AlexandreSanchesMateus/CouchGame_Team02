using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnigmeManager : MonoBehaviour
{
    public static EnigmeManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 9.12f, gameObject.transform.localPosition.z);
    }

    public void SuccessKeypade()
    {
        gameObject.transform.DOLocalMoveY( 10.6f, 1);
    }

    public void SuccessSimon()
    {
        gameObject.transform.DOLocalMoveY(12f, 1);
    }

    public void SuccessCoffre()
    {
        gameObject.transform.DOLocalMoveY(12.5f, 1);
    }

    public void SuccessElement()
    {
        Debug.Log("Sucess");
    }
}
