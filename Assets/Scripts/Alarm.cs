using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] float AlarmSpeed;
    private float rotY;

    void Update()
    {
        rotY += AlarmSpeed;
        transform.localRotation = Quaternion.Euler(0, rotY, 0);
    }
}
