using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    public ACCESS rightWall;
    public ACCESS bottomWall;

    public enum ACCESS
    {
        BLOCK,
        HACKER,
        ROBBER,
        BOTH,
        EITHER
    }

    public Slot(Slot source)
    {
        this.rightWall = source.rightWall;
        this.bottomWall = source.bottomWall;
    }

    [SerializeField]
    public Slot()
    {
        rightWall = ACCESS.BLOCK;
        bottomWall = ACCESS.BLOCK;
    }

    public void IncrementAccess(bool isRightwall)
    {
        if (isRightwall)
            this.rightWall = (ACCESS)((this.rightWall.GetHashCode() + 1) % 5);
        else
            this.bottomWall = (ACCESS)((this.bottomWall.GetHashCode() + 1) % 5);
    }
}
