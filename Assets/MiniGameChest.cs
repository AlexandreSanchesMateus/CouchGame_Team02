using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MiniGameChest : MonoBehaviour, IMinigame 
{
    [SerializeField] private string code;
    private bool isWin;
    public TextMeshPro text;
    private AudioSource audioSource;
    [SerializeField] private AudioClip feedback1, feedback2, feedback3, feedback4;
    [SerializeField] private AudioClip win, lose;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public bool interact(InputAction.CallbackContext callback)
    {
        if (!isWin)
            switch (callback.action.name)
            {
                case "West":
                text.text += " ";
                    audioSource.PlayOneShot(feedback1);
                    break;
                case "South":
                text.text += ".";
                    audioSource.PlayOneShot(feedback2);
                    break;
                case "East":
                text.text += "-";
                    audioSource.PlayOneShot(feedback3);
                    break;
                case "North":
                    audioSource.PlayOneShot(feedback4);
                    if (text.text.Length>0)
                    text.text = text.text.Substring(0, text.text.Length - 1);
                break;
                default:
                break;
            }

        if (text.text == code)
        {
            isWin = true;
            return true;
        }
        return false;
    }
    private void Update()
    {
        if(isWin)
        {
            //audioSource.PlayOneShot(win);
            text.text = "Jean Cérien";
           // text.color = Color.green;
        }
        if (text.text.Length > 12)
        {
            //audioSource.PlayOneShot(lose);
            text.text="";
        }
    }
    public void Move(InputAction.CallbackContext callback) { }
}
