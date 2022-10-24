using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ElementPad : MonoBehaviour, IInteractible
{

    [Header("Canvas")]
    [SerializeField] private GameObject GUIhover;
    [SerializeField] private GameObject Pad;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private GameObject lockey;

    private List<int> previousElement = new List<int>();
    private int idKey;
    private bool isOpen;

    void Start()
    {
        isOpen = false;
        idKey = 0;
        keys[idKey].GetComponent<Image>().color = Color.grey;
        GUIhover.SetActive(false);
        Pad.SetActive(false);
    }
    public void OnActions(Vector2 action)
    {
        keys[idKey].GetComponent<Image>().color = Color.white;

        if(action == Vector2.right)
        {
            idKey++;
        }
        else if(action == Vector2.left)
        {
            idKey--;
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
        GUIhover.SetActive(true);
    }

    public void OnInteract()
    {
        throw new System.NotImplementedException();
    }

    public void OnReturn()
    {
        isOpen = false;
        GUIhover.SetActive(false);
        Pad.SetActive(false);
        PlayerControllerProto2.enablePlayerMovement = true;
    }
}
