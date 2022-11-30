using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject GUI;

    public void OnActions(Vector2 action, Vector2 joystick) { }

    public void OnItemExit()
    {
        GUIManager.instance.EnablePick_upGUI(false);
    }

    public void OnItemHover()
    {
        GUIManager.instance.EnablePick_upGUI(true);
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }

    public void OnReturn() { }

    public void OnRightShoulder() { }

    public void OnHoldReturn() { }
}
