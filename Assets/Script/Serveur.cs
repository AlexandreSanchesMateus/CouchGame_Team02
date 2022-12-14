using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serveur : MonoBehaviour
{
    [SerializeField]
    private InspectedObject grabObj;
    [SerializeField] private AudioClip[] pullOut;

    private AudioSource audioSource;

    void Start()
    {
        grabObj.OnGrab += OnGrabDisquette;
        if(ElementPad.instance)
            ElementPad.instance.allServeur.Add(this);
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnGrabDisquette(InspectedObject source)
    {
        source.OnGrab -= OnGrabDisquette;
        gameObject.layer = LayerMask.GetMask("Default");
        audioSource.PlayOneShot(pullOut[Random.Range(0, pullOut.Length)]);
        if (ElementPad.instance)
            ElementPad.instance.InitLabyrintheScreen(this);
    }
}
