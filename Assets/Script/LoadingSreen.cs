using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class LoadingSreen : MonoBehaviour, IMinigame
{
    public Transform pos1, pos2;
    private bool canInteract;
    private void OnEnable()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(pos2.position,1f));
        mySequence.Append(transform.DOMove(pos1.position,1f));
        mySequence.SetLoops(-1, LoopType.Yoyo);
        
    }
    public bool interact(InputAction.CallbackContext callback)
    {
        if (canInteract) 
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in");
        if (other.CompareTag("LoadingTrigger"))
        {
           
            canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("out");
        if (other.CompareTag("LoadingTrigger"))
        {
            
            canInteract = false;
        }
    }
    public void Move(InputAction.CallbackContext callback) { }s
}
