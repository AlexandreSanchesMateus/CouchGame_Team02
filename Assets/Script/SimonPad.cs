using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SimonPad : MonoBehaviour, IInteractible
{

    [Header("Canvas")]
    [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Pad;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private List<Image> lights;
    [SerializeField] private Sprite unselected;
    [SerializeField] private Sprite selected;


    [Header("Correspondance Editor")]
    [Header("ID : Jaune: 0; Rouge: 1; Magenta: 2; Bleu: 3; Cyan: 4; Vert: 5")]
    public ColorSimon[] colors = { new ColorSimon(COLORS.ROUGE), new ColorSimon(COLORS.BLEU), new ColorSimon(COLORS.VERT), new ColorSimon(COLORS.JAUNE) };

    private ColorSimon currentColorText;
    private ColorSimon currentColor;

    //public static Vector2 hacker { set { hacker = value; OnActionsHacker();  } }

    private int idKey;
    private bool isOpen;
    private bool isValid;
    private int nbValid;
    private int idColor;

    void Start()
    {
        isOpen = false;
        idKey = 0;
        nbValid = 0;
        keys[idKey].GetComponent<Image>().sprite = selected;
        GUIhover.SetActive(false);
        Pad.SetActive(false);
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        keys[idKey].GetComponent<Image>().sprite = unselected;

        if (action == Vector2.right)
        {
            idKey++;
            if (idKey == colors.Length)
            {
                idKey = 0;
            }
        }
        else if (action == Vector2.left)
        {
            idKey--;
            if (idKey < 0)
            {
                idKey = colors.Length - 1;
            }
        }


        idKey = Mathf.Clamp(idKey, 0, keys.Length - 1);
        keys[idKey].GetComponent<Image>().sprite = selected;

    }

    private static void OnActionsHacker()
    {

    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            idColor = 0;
            GUIhover.SetActive(false);
            Pad.SetActive(true);
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

        if (isValid)
            return;

        StartCoroutine(ColorRotation());

        /*int braqueurId = currentColorText.idBraqueur;
        int hackeurId = currentColor.idHackeur;*/

        //if(idKeyHackeur == hackeurColor && idKeyBraqueur == braqueurColor)
        if (idKey == currentColorText.idBraqueur)
        {
            Debug.Log("Bon");
            lights[nbValid].color = Color.green;
            nbValid++;
            if (nbValid == 3)
            {

            }
            //StartCoroutine(ColorRotation());
        }
        else
        {
            for (int i = nbValid; i > -1; i--)
            {
                lights[i].color = Color.red;
            }
            nbValid = 0;
            StartCoroutine(ColorRotation());
        }



    }

    public void OnItemExit()
    {
        GUIhover.SetActive(false);
    }

    public void OnItemHover()
    {
        if (!isValid)
            GUIhover.SetActive(true);
    }

    public void OnReturn()
    {
        isOpen = false;
        Pad.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
    }

    private Color ChooseTextColor(ColorSimon color)
    {
        switch (color.color)
        {
            case COLORS.ROUGE:
                return Color.red;

            case COLORS.BLEU:
                return Color.blue;

            case COLORS.VERT:
                return Color.green;

            case COLORS.JAUNE:
                return Color.yellow;

            case COLORS.CYAN:
                return Color.cyan;

            case COLORS.MAGENTA:
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
        BLEU,
        ROUGE,
        VERT,
        JAUNE,
        CYAN,
        MAGENTA
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
}
