using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    public void OnItemHover();
    public void OnItemExit();
    public void OnInteract();
    public void OnReturn();
    public void OnActions(Vector2 action, Vector2 joystick);
    public void OnRightShoulder();
    public void OnHoldReturn();
}
