using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public bool isGood;
    public bool isInside;
    public bool TestWin() 
    {
       if(isGood)

            return true;
       else
            return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("cube"))
        {
            isGood = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cube"))
        {
            isGood = false;
        }
    }

}
