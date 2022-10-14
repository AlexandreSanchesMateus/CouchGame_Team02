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
            isOpen = true;
        }
        else
        {
            if(currentCode.Count < 3)
            {
                currentCode.Add(idKey);
                string toDisplay = "";

                for (int i = 0; i < 4 - currentCode.Count; i++)
                {
                    toDisplay += " _";
                }

                foreach(int i in currentCode)
                {
                    toDisplay += i.ToString();
                }
                displayCode.text = toDisplay;
            }
            else
            {
                currentCode.Add(idKey);
                Debug.Log("Vérification de : " + ListToString(currentCode) + " " + goodCode);
                if (ListToString(currentCode) == goodCode)
                {
                    Destroy(door);
                    Debug.Log("Pass");
                    foreach (GameObject key in keys)
                        key.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    Debug.Log("Don't pass");
                    currentCode.Clear();
                    displayCode.text = " _ _ _ _";
                }
            }
        }
    }

    public void OnReturn()
    {
        isOpen = false;
        idKey = 0;
        GUIhover.SetActive(false);
        Keypad.SetActive(false);
    }

    private string ListToString(List<int> list)
    {
        string toReturn = "";
        foreach (int i in list)
            toReturn += i.ToString();

        return toReturn;
    }
}
