using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class billboarding : MonoBehaviour
{
    [SerializeField] private Transform target;

    private AudioSource audioSource;

    [SerializeField] AudioClip feedback1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Sequence mySequence = DOTween.Sequence();
        audioSource.PlayOneShot(feedback1);
        transform.GetChild(0).DOScaleY(0, 0.5f).SetEase(Ease.InOutBack);

    }
    private void OnTriggerExit(Collider other)
    {
        transform.GetChild(0).DOScaleY(1, 0.5f).SetEase(Ease.OutBounce);
    }
}
