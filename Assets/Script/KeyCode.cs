using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyCode : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject vcam;

    [Header("Canvas")]
    [SerializeField] private TextMeshProUGUI displayCode;

    [Header("Code")]
    [SerializeField] private GameObject[] keys;
    [SerializeField] private string goodCode;
    [SerializeField] private GameObject door;

    [Header("Auio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip enterClip;
    [SerializeField] private AudioClip outClip;
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip inputClip;

    private List<int> currentCode = new List<int>();
    private int idKey;
    private bool isOpen;
    private bool isVerif = false;

    void Start()
    {
        isOpen = false;
        idKey = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = enterClip;
    }

    // Lorsque je joueur utilise l'object
    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isOpen || isVerif || action == Vector2.zero)
            return;

        if (action == Vector2.up)
        {
            if(idKey - 3 >= 0)
                idKey -= 3;
        }
        else if (action == Vector2.down)
        {
            if (idKey + 3 < keys.Length)
                idKey += 3;
        }
        else if (action == Vector2.right)
            idKey++;
        else if (action == Vector2.left)
            idKey--;

        PlayAudio(hoverClip);
        idKey = Mathf.Clamp(idKey, 0, keys.Length - 1);
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKey].transform.position);
    }

    // Lorsque je joueur arrete de regarder l'object
    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
    }

    // Lorsque je joueur regarde l'object
    public void OnItemHover()
    {
        GUIManager.instance.EnableUseGUI(true);
    }

    // Lorsque je joueur interagie avec l'object
    public void OnInteract()
    {
        
        // SI NON OUVERT : orienter caméra + désactiver input player
        if (!isOpen)
        {
            PlayAudio(enterClip);
            StartCoroutine(Delay());
            GUIManager.instance.EnableUseGUI(false);
            // GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKey].transform.position);
            vcam.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
        }
        // SI DEJA OUVERT : appuyer sur les touche
        else
        {
            PlayAudio(inputClip);
            switch (idKey)
            {
                case 9:
                    StartCoroutine(Verification());
                    break;
                case 11:
                    currentCode.Clear();
                    displayCode.text = " _ _ _ _";
                    break;
                default:
                    if(currentCode.Count < 4)
                    {
                        currentCode.Add(KeyToInt(idKey));
                        displayCode.text = DisplayCode();
                    }
                    break;
            }
        }
    }

    // Lo
    // rsque le joueur revient en arrière
    public void OnReturn()
    {
        PlayAudio(outClip);
        isOpen = false;
        GUIManager.instance.EnableUseGUI(false);
        GUIManager.instance.EnableHand(false);
        vcam.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
        StopAllCoroutines();
    }

    // Prépar la list d'int saisi en une chaine de caractère pour etre comparer au code de référence
    private string ListToString(List<int> list)
    {
        string toReturn = "";
        foreach (int i in list)
            toReturn += i.ToString();

        return toReturn;
    }

    // Convertie la liste int en string. Ajoute " _" s'il y a moins de 4 éléments dans la liste
    private string DisplayCode()
    {
        string toDisplay = "";

        for (int i = 0; i < 4 - currentCode.Count; i++)
        {
            toDisplay += " _";
        }

        foreach (int i in currentCode)
        {
            toDisplay += i.ToString();
        }

        return toDisplay;
    }

    // Converti l'id key en le chiffre réel du keycode
    // Ex : idKey = 0 correspond au nombre 1 du paver numérique
    private int KeyToInt(int id)
    {
        if (id == 10)
            return 0;
        return id + 1;
    }

    // Vérification du code saisi et de du code de référence
    private IEnumerator Verification() 
    {
        isVerif = true;
        yield return new WaitForSeconds(1);

        // CODE BON
        if (ListToString(currentCode) == goodCode)
        {
            displayCode.text = " G O O D";
            gameObject.layer = 0;
            Destroy(door);
            yield return new WaitForSeconds(1);
            vcam.SetActive(false);
            GUIManager.instance.EnableHand(false);
            yield return new WaitForSeconds(2);
            PlayerControllerProto2.enablePlayerMovement = true;
        }
        // CODE MAUVAIS
        else
        {
            Debug.Log("Don't pass");
            currentCode.Clear();
            displayCode.text = " _ _ _ _";
            isVerif = false;
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKey].transform.position);
    }
}
