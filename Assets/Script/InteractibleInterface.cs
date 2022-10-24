using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    public void OnItemHover();
    public void OnItemExit();
    public void OnIteract();
    public void OnReturn();
    public void OnActions(Vector2 action);
}
