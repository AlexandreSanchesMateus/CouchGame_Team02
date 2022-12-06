using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnigmeManager : MonoBehaviour
{
    public static EnigmeManager instance { get; private set; }
    [SerializeField] private AudioClip vaultOpen;
    [SerializeField] private AudioClip vaultMecanisme;

    private AudioSource audioSource;

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
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SuccessKeypade()
    {
        UpdateVaultPosition(10.6f);
        //gameObject.transform.DOLocalMoveY( 10.6f, 1);
    }

    public void SuccessSimon()
    {
        UpdateVaultPosition(12f);
        //gameObject.transform.DOLocalMoveY(12f, 1);
    }

    public void SuccessCoffre()
    {
        UpdateVaultPosition(12.5f);
        //gameObject.transform.DOLocalMoveY(12.5f, 1);
    }

    public void SuccessElement()
    {
        Debug.Log("Sucess");
    }

    private void UpdateVaultPosition(float PosY)
    {
        Sequence VaultUp = DOTween.Sequence();
        VaultUp.AppendInterval(1.6f);
        VaultUp.AppendCallback(() => audioSource.PlayOneShot(vaultMecanisme));
        VaultUp.AppendInterval(5);
        VaultUp.AppendCallback(() => audioSource.PlayOneShot(vaultOpen));
        VaultUp.Join(gameObject.transform.DOLocalMoveY(PosY, 3).SetEase(Ease.Linear));
    }
}
