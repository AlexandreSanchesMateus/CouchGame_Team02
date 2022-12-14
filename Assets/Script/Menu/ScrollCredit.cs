using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrollCredit : MonoBehaviour
{
    Sequence ScrollingSequence;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject credit;

    private void OnEnable()
    {
        gameObject.transform.localPosition = new Vector3(0, -1177, 0);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (ScrollingSequence.IsActive())
            ScrollingSequence.Restart();
        else
        {
            ScrollingSequence = DOTween.Sequence();
            ScrollingSequence.Append(canvasGroup.DOFade(1f, 1f));
            ScrollingSequence.AppendInterval(2f);
            ScrollingSequence.Append(gameObject.transform.DOLocalMoveY(2612, 40).SetEase(Ease.Linear));
            ScrollingSequence.AppendCallback(ExitCredit);
            ScrollingSequence.Append(canvasGroup.DOFade(0f, 1f));
            ScrollingSequence.AppendCallback(EnableCredit);
        }
    }

    private void ExitCredit()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        buttons.SetActive(true);
    }

    public void OnBack()
    {
        ExitCredit();
        canvasGroup.DOFade(0f, 1f);
        Invoke("EnableCredit", 1f);
    }

    private void EnableCredit()
    {
        credit.SetActive(false);
    }
}
