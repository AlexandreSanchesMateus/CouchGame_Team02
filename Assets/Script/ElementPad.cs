using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.ProBuilder.Shapes;

public class ElementPad : MonoBehaviour, IInteractible
{

    [Header("Canvas")]
    [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Pad;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private GameObject lockey;
    [SerializeField] private List<Image> lights;

    [Header("Réponses")]
    [SerializeField] private List<Etapes> etapes;

    private Situation currentSituation;

    private List<int> previousElement = new List<int>();
    private int idKey;
    private bool isOpen;
    private int actualEtape;
    private bool isValid = false;


    void Start()
    {
        isOpen = false;
        idKey = 0;
        keys[idKey].GetComponent<Image>().color = Color.grey;
        GUIhover.SetActive(false);
        Pad.SetActive(false);
    }
    public void OnActions(Vector2 action, Vector2 joystick)
    {
        keys[idKey].GetComponent<Image>().color = Color.white;

        if(action == Vector2.right)
        {
            idKey++;
            if(idKey == 4)
            {
                idKey = 0;
            }
        }
        else if(action == Vector2.left)
        {
            idKey--;
            if(idKey < 0)
            {
                idKey = 3;
            }
        }

        idKey = Mathf.Clamp(idKey, 0, keys.Length - 1);
        keys[idKey].GetComponent<Image>().color = Color.grey;
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

    public void OnInteract()
    {
        int idSituation = 0;
        if (!isOpen)
        {
            GUIhover.SetActive(false);
            Pad.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
            idSituation = Random.Range(0, 3);
            currentSituation = etapes[actualEtape].situations[idSituation];
            display.text = etapes[actualEtape].situations[idSituation].element.ToString();
            return;
        }

        if (isValid)
        {
            return;
        }

        // SI changement GD
        // currentSituation = etapes[actualEtape].situations[Random.Range(0, etapes[actualEtape].situations.Length - 1)];
        int key = currentSituation.goodkey;

        if(currentSituation.goodkey < 0)
        {
            key = previousElement[Mathf.Abs(currentSituation.goodkey) - 1];
        }

        if (idKey == key)
        {
            lights[actualEtape].color = Color.green;
            previousElement.Add(idKey);
            actualEtape++;
            if (actualEtape == 6)
            {
                Destroy(lockey);
                StartCoroutine(Delay());
                display.text = "Well done";
                isValid = true;
                return;
            }
        }
        else
        {
            for(int i = actualEtape; i > -1; i--)
            {
                lights[i].color = Color.red;
            }
            previousElement.Clear();
            actualEtape = 0;
        }
        
        idSituation = Random.Range(0, 3);
        currentSituation = etapes[actualEtape].situations[idSituation];
        display.text = etapes[actualEtape].situations[idSituation].element.ToString();
    }

    public void OnReturn()
    {
        isOpen = false;
        Pad.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        PlayerControllerProto2.enablePlayerMovement = true;
        //yield return new WaitForSeconds(1);
        Pad.SetActive(false);
    }

}
