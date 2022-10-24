using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject GUI;

    public void OnActions(Vector2 action)
    {
        throw new System.NotImplementedException();
    }

    public void OnItemExit()
    {
        GUI.SetActive(false);
    }

    public void OnItemHover()
    {
        GUI.SetActive(true);
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }

    public void OnReturn()
    {
        throw new System.NotImplementedException();
    }
}
