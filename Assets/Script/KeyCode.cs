using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyCode : MonoBehaviour, IInteractible
{
    [Header("Canvas")]
    [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Keypad;
    [SerializeField] private TextMeshProUGUI displayCode;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private string goodCode;
    [SerializeField] private GameObject door;

    private List<int> currentCode = new List<int>();
    private int idKey;
    private bool isOpen;
    private bool isVerif = false;


    void Start()
    {
        isOpen = false;
        idKey = 0;
        keys[idKey].GetComponent<Image>().color = Color.red;
        GUIhover.SetActive(false);
        Keypad.SetActive(false);
    }

    public void OnActions(Vector2 action)
    {
        if (isVerif)
            return;

        keys[idKey].GetComponent<Image>().color = Color.white;

        if (action == Vector2.up)
        {
            idKey -= 3;
        }
        else if (action == Vector2.down)
        {
            if (idKey == 0)
                idKey += 2;
            else
                idKey += 3;
        }
        else if (action == Vector2.right)
            idKey++;
        else if (action == Vector2.left)
            idKey--;

        idKey = Mathf.Clamp(idKey, 0, keys.Length - 1);
        keys[idKey].GetComponent<Image>().color = Color.red;
    }

    public void OnItemExit()
    {
        GUIhover.SetActive(false);
    }

    public void OnItemHover()
    {
        GUIhover.SetActive(true);
    }

    public void OnIteract()
    {
        if (!isOpen)
        {
            GUIhover.SetActive(false);
            Keypad.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
        }
        else
        {
            if(currentCode.Count < 3)
            {
                currentCode.Add(idKey);
                
                displayCode.text = DisplayCode();
            }
            else
            {
                currentCode.Add(idKey);
                displayCode.text = DisplayCode();
                StartCoroutine(Verification());
                
            }
        }
    }

    public void OnReturn()
    {
        isOpen = false;
        GUIhover.SetActive(false);
        Keypad.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
    }

    private string ListToString(List<int> list)
    {
        string toReturn = "";
        foreach (int i in list)
            toReturn += i.ToString();

        return toReturn;
    }

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



    private IEnumerator Verification() 
    {
        isVerif = true;
        yield return new WaitForSeconds(1);
        Debug.Log("V�rification de : " + ListToString(currentCode) + " " + goodCode);
        if (ListToString(currentCode) == goodCode)
        {
            Destroy(door);
            StartCoroutine(Delay());
            Debug.Log("Pass");
            foreach (GameObject key in keys)
                key.GetComponent<Image>().color = Color.green;
            displayCode.text = " G O O D";

        }
        else
        {
            Debug.Log("Don't pass");
            currentCode.Clear();
            displayCode.text = " _ _ _ _";
            isVerif = false;
        }
        
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1) ;
        PlayerControllerProto2.enablePlayerMovement = true;
        //yield return new WaitForSeconds(1);
        Keypad.SetActive(false);
    }
}
