using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance { get; private set; }


    [SerializeField] private Camera PlayerCam;
    [SerializeField] private GameObject hand;

    [Header("Gui Informations")]
    [SerializeField] private GameObject Pick_up;
    [SerializeField] private GameObject Use;

    private void Awake()
    {
        instance = this;
    }

    public void EnablePick_upGUI(bool statut)
    {
        Pick_up.SetActive(statut);
    }

    public void EnableUseGUI(bool statut)
    {
        Use.SetActive(statut);
    }

    public void EnableHand(bool statut)
    {
        if (!statut)
            hand.transform.localPosition = new Vector3(-1380, -400, 0);

        hand.SetActive(statut);
    }

    public void MoveHandWorldToScreenPosition(Vector3 worldPosition)
    {
        if (!hand.gameObject.activeSelf)
            hand.SetActive(true);

        Vector2 screenPos = PlayerCam.WorldToScreenPoint(worldPosition);

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.GetComponent<RectTransform>(), screenPos, PlayerCam, out Vector2 local))
            hand.transform.DOLocalMove(local, 0.2f).SetEase(Ease.InSine);
    }
}
