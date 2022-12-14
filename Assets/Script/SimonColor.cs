using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimonColor : MonoBehaviour
{
    public enum CubeColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Cyan,
        Magenta
    }
    public CubeColor color;

    public GameObject selected;
    public bool isHover, isSelected, wait;
    public Sequence sequence;

    public void Start()
    {
        isHover = false;
        isSelected = false;
        wait = true;

        
    }

    public void Update()
    {
        sequence = DOTween.Sequence();
        if (isHover && wait)
        {
            wait = false;
            sequence.Append(this.transform.DOScale(new Vector3(1.4f,1.4f,1f), 0.4f).SetEase(Ease.InOutQuad));
            sequence.Append(this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f).SetEase(Ease.InOutQuad)).OnComplete(() => wait = true);
        }
    }

}
