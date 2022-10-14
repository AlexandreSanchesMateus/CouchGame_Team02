using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject GUI;

    public void OnItemExit()
    {
        GUI.SetActive(false);
    }

    public void OnItemHover()
    {
        GUI.SetActive(true);
    }

    public void OnIteract()
    {
        Destroy(gameObject);
    }
}
