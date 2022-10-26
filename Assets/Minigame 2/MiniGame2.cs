using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGame2 : MonoBehaviour,IMinigame
{
    private float stickRotationInDeg;
    private GameObject color, selectedColor;
    
    public bool interact(InputAction.CallbackContext ctx)
    {
       
        return false;
        
        //if (stickRotationInDeg < 0)
        //    stickRotationInDeg += 360;

        //if (stickRotationInDeg == 0)
        //    return false;


        //    if (stickRotationInDeg > 330.0f || stickRotationInDeg < 30.0f)
        //        Debug.Log("1");
        //    else if (stickRotationInDeg > 30.0f && stickRotationInDeg < 90.0f)
        //    Debug.Log("2");
        //    else if (stickRotationInDeg > 90.0f && stickRotationInDeg < 150.0f)
        //        Debug.Log("3");
        //    else if (stickRotationInDeg > 150.0f && stickRotationInDeg < 210.0f)
        //        Debug.Log("4");
        //    else if (stickRotationInDeg > 210.0f && stickRotationInDeg < 270.0f)
        //        Debug.Log("5");
        //    else if (stickRotationInDeg > 270.0f && stickRotationInDeg < 330.0f)
        //        Debug.Log("6");

        //if (lastColor != color)
        //    {
        //        lastColor = color;
        //    }
        //return false;

    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log("interact");
        Vector2 value = ctx.ReadValue<Vector2>().normalized;
        Debug.Log("x : " + value.x + " y : " + value.y);

        stickRotationInDeg = Mathf.Rad2Deg * Mathf.Atan2(-value.x, value.y);
        if (value.x > 0.5 || value.x < -0.5 || value.y > 0.5 || value.y < -0.5)
        {
            transform.rotation = Quaternion.Euler(0, 0, stickRotationInDeg);
        }

    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
