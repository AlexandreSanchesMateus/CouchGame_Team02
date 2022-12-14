using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class SimonPad : MonoBehaviour, IInteractible
{
    public static SimonPad instance;

    [SerializeField] private GameObject vcam, door;

    [Header("Canvas")]
    /* [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Pad;*/

    [SerializeField] private TextMeshProUGUI display;

    [SerializeField] private GameObject[] keys;

    //[SerializeField] private List<Image> lights;
    [SerializeField] private List<GameObject> lights;
    [SerializeField] private Material redMat, greenMat;


    /*[SerializeField] private Sprite unselected;
    [SerializeField] private Sprite selected;*/


    [Header("Correspondance Editor")]
    [Header("ID : Jaune: 0; Rouge: 1; Magenta: 2; Bleu: 3; Cyan: 4; Vert: 5")]
    public ColorSimon[] colors = { new ColorSimon(COLORS.RED), new ColorSimon(COLORS.BLUE), new ColorSimon(COLORS.GREEN), new ColorSimon(COLORS.YELLOW) };

    private ColorSimon currentColorText, currentColor;

    [Header("Audio")]
    [SerializeField] private AudioClip sucsess;
    [SerializeField] private AudioClip fail;
    [SerializeField] private AudioClip validate;
    [SerializeField] private AudioClip[] hover;
    [SerializeField] private AudioClip inputClip;
    [SerializeField] private AudioClip open;
    private AudioSource audioSource;

    //public static Vector2 hacker { set { hacker = value; OnActionsHacker();  } }

    public int hackeurId;
    public bool braqueurPlay, hackeurPlay;

    private int braqueurId;
    private bool isOpen;
    private int nbValid, idColor;

    public float valeurScale;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        isOpen = false;
        braqueurId = 0;
        nbValid = 0;

        audioSource = GetComponent<AudioSource>();
        // keys[idKey].GetComponent<Image>().sprite = selected;
        // GUIhover.SetActive(false);
        // Pad.SetActive(false);
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isOpen || action == Vector2.zero)
            return;

        // keys[idKey].GetComponent<Image>().sprite = unselected;

        if (action == Vector2.right)
        {
            braqueurId++;
            if (braqueurId == colors.Length)
            {
                braqueurId = 0;
            }
        }
        else if (action == Vector2.left)
        {
            braqueurId--;
            if (braqueurId < 0)
            {
                braqueurId = colors.Length - 1;
            }
        }

        audioSource.PlayOneShot(inputClip);
        braqueurId = Mathf.Clamp(braqueurId, 0, keys.Length - 1);
        // keys[idKey].GetComponent<Image>().sprite = selected;
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[braqueurId].transform.position);
    }

    public void CheckPlayerEntry()
    {

        if(braqueurPlay && hackeurPlay)
        {
            if (hackeurId == currentColor.idHackeur && braqueurId == currentColorText.idBraqueur)
            {
                Debug.Log("Bon");
                // lights[nbValid].color = Color.green;
                lights[nbValid].GetComponent<MeshRenderer>().material = greenMat;
                audioSource.PlayOneShot(validate);
                nbValid++;
                if (nbValid == 3)
                {
                    StartCoroutine(PanelComplet());
                }
            }
            else
            {
                audioSource.PlayOneShot(fail);

                if (braqueurId == currentColorText.idBraqueur)
                {
                    HackerController.instance.WrongAnswerLights();
                }
            }

            MiniGame2.instance.lastColor.transform.GetChild(0).gameObject.SetActive(false);
            MiniGame2.instance.lastColor.transform.DOScale(new Vector3(1, 1, 1), 0.2f);

            braqueurPlay = false;
            hackeurPlay = false;

            StartCoroutine(ColorRotation());
        }
    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            audioSource.PlayOneShot(open);
            idColor = 0;
            // GUIhover.SetActive(false);
            GUIManager.instance.EnableUseGUI(false);
            GUIManager.instance.ScaleHand(valeurScale, valeurScale, valeurScale);
            // Pad.SetActive(true);
            StartCoroutine(Delay());
            vcam.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
            idColor = Random.Range(0, colors.Length - 1);
            currentColorText = colors[idColor];
            idColor = Random.Range(0, colors.Length - 1);
            currentColor = colors[idColor];

            display.text = currentColorText.color.ToString();
            display.color = ChooseTextColor(currentColor);

            return;
        }

        braqueurPlay = true;
        CheckPlayerEntry();
        //if(idKeyHackeur == hackeurColor && idKeyBraqueur == braqueurColor)

    }

    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
        // GUIhover.SetActive(false);
    }

    public void OnItemHover()
    {
        audioSource.PlayOneShot(hover[Random.Range(0, hover.Length)]);
        GUIManager.instance.EnableUseGUI(true);
        // GUIhover.SetActive(true);
    }

    public void OnReturn()
    {
        isOpen = false;
        for (int i = nbValid - 1; i > -1; i--)
        {
            // lights[i].color = Color.red;
            lights[i].GetComponent<MeshRenderer>().material = redMat;
        }
        nbValid = 0;
        // Pad.SetActive(false);
        vcam.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
        GUIManager.instance.EnableHand(false);
    }

    private Color ChooseTextColor(ColorSimon color)
    {
        switch (color.color)
        {
            case COLORS.RED:
                return Color.red;

            case COLORS.BLUE:
                return Color.blue;

            case COLORS.GREEN:
                return Color.green;

            case COLORS.YELLOW:
                return Color.yellow;

            case COLORS.LIGHTBLUE:
                return Color.cyan;

            case COLORS.PINK:
                return Color.magenta;

        }
        return Color.white;            

    }


    [System.Serializable]
    public struct ColorSimon
    {
        public COLORS color;
        public int idBraqueur;
        public int idHackeur;

        public ColorSimon(COLORS color)
        {
            this.color = color;
            idBraqueur = 0;
            idHackeur = 0;
        }
    }

    public enum COLORS
    {
        BLUE,
        RED,
        GREEN,
        YELLOW,
        LIGHTBLUE,
        PINK
    }

    private IEnumerator ColorRotation()
    {
        for(int i = 0; i < Random.Range(3, Random.Range(4, 6)); i++)
        {
            idColor = Random.Range(0, 3);
            currentColorText = colors[idColor];
            idColor = Random.Range(0, 3);
            currentColor = colors[idColor];

            display.text = currentColorText.color.ToString();
            display.color = ChooseTextColor(currentColor);

            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log(currentColorText.idBraqueur);
        Debug.Log(currentColor.idBraqueur);
    }

    private IEnumerator PanelComplet()
    {
        //AudioSpeaker.instance.AlarmIntensite();
        audioSource.PlayOneShot(sucsess);

        //Destroy(door);
        EnigmeManager.instance.SuccessSimon();
        GUIManager.instance.EnableHand(false);
        vcam.SetActive(false);
        gameObject.layer = 0;
        yield return new WaitForSeconds(2);
        PlayerControllerProto2.enablePlayerMovement = true;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[braqueurId].transform.position);
    }

    public void OnRightShoulder() { }

    public void OnHoldReturn() { }
}
