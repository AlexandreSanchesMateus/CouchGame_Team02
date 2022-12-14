using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnigmeManager : MonoBehaviour
{
    public static EnigmeManager instance { get; private set; }
    [SerializeField] private AudioClip vaultOpen;
    [SerializeField] private AudioClip vaultMecanisme;
    [SerializeField] private Material[] matLampe;

    public bool lastegnimedone = false;


    // Prototype
    public delegate void AlarmLisener(float duration);
    // d�claration de la variable
    public AlarmLisener OnAlarmeEnable;

    public delegate void LightLisener(bool status);
    public LightLisener OnLightEnable;


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
        OnLightEnable += MaterialEnable;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 8.47f, gameObject.transform.localPosition.z);
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SuccessKeypade()
    {
        AudioManager.instance.IncreaseMusicLevel();
        UpdateVaultPosition(9.94f);
    }

    public void SuccessSimon()
    {
        AudioManager.instance.IncreaseMusicLevel();
        UpdateVaultPosition(11.38f, true);
    }

    public void SuccessElementPad()
    {
        AudioManager.instance.IncreaseMusicLevel();
        
        UpdateVaultPosition(12.64f);
    }

    private void UpdateVaultPosition(float PosY, bool lightOn = false)
    {
        OnAlarmeEnable(7);
        Sequence VaultUp = DOTween.Sequence();
        VaultUp.AppendInterval(1.6f);
        VaultUp.AppendCallback(() => audioSource.PlayOneShot(vaultMecanisme));
        VaultUp.Join(gameObject.transform.DOLocalMoveY(PosY, 4.8f).SetEase(Ease.Linear));

        if (lightOn)
            VaultUp.AppendCallback(() => OnLightEnable(false));
    }

    private void MaterialEnable(bool active)
    {
        if(active)
            foreach(Material other in matLampe)
            {
                other.EnableKeyword("_EMISSION");
            }
        else
            foreach (Material other in matLampe)
            {
                other.DisableKeyword("_EMISSION");
            }
    }
}
