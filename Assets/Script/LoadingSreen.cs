using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadingSreen : MonoBehaviour, IMinigame
{
    public Transform pos1, pos2;
    private bool canInteract;
    [SerializeField] private AudioClip clip1, clip2;
    private AudioSource audioSource;
    public int numberRequired;
    private int currentNumber;
    public GameObject slider;
    public float actualSliderValue;

    private bool boolsfx;
    private void OnEnable()
    {
        slider.transform.localScale = new Vector3(0, 1, 1);
        actualSliderValue = 0;
        audioSource = GetComponent<AudioSource>();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(pos2.position,1f));
        mySequence.Append(transform.DOMove(pos1.position,1f));
        mySequence.SetLoops(-1, LoopType.Yoyo);
    }
    public bool interact(InputAction.CallbackContext callback)
    {
        Sequence newSequence = DOTween.Sequence();

        if (canInteract && callback.action.name == "West" && callback.started)
        {
            if (!boolsfx)
            {
                audioSource.PlayOneShot(clip1);
                boolsfx = true;
            }
            else
            {
                audioSource.PlayOneShot(clip2);
                boolsfx = false;
            }

            
            currentNumber++;
            actualSliderValue += (1 / (float)numberRequired);
            newSequence.Append(slider.transform.DOScale(new Vector3(Mathf.Clamp(actualSliderValue,0,1), 1f, 1f), 0.25f).SetEase(Ease.OutCirc));
        }
        if (currentNumber >= numberRequired)
        {
            StartCoroutine(Delay());
            currentNumber = 0;
            return true;
        }
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LoadingTrigger"))
        {

            canInteract = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LoadingTrigger"))
        {
            /*if (!boolsfx)
            {
                audioSource.PlayOneShot(clip1);
                boolsfx = true;
            }
            else
            {
                audioSource.PlayOneShot(clip2);
                boolsfx = false;
            }*/
            canInteract = false;
        }
    }
    public void Move(InputAction.CallbackContext callback) { }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        slider.transform.localScale = new Vector3(0, 1, 1);
        actualSliderValue = 0;
    }
}
