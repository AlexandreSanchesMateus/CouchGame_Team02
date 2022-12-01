using DG.Tweening;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class LoadingSreen : MonoBehaviour, IMinigame
{
    public Transform pos1, pos2;
    private bool canInteract;
    [SerializeField] private AudioClip clip1, clip2;
    private AudioSource audioSource;
    private int numberRequired;
    private int currentNumber;
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(pos2.position,1f));
        mySequence.Append(transform.DOMove(pos1.position,1f));
        mySequence.SetLoops(-1, LoopType.Yoyo);
    }
    public bool interact(InputAction.CallbackContext callback)
    {
        if (canInteract && callback.action.name == "West" && callback.started)
        {
            audioSource.PlayOneShot(clip1);
            currentNumber++;
        }
        if (currentNumber >= numberRequired)
            return true;
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LoadingTrigger"))
        {
            audioSource.PlayOneShot(clip2);
            canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LoadingTrigger"))
        {
            canInteract = false;
        }
    }
    public void Move(InputAction.CallbackContext callback) { }
}
