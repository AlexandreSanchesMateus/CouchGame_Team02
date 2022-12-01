using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeath : MonoBehaviour
{
    [SerializeField] float delayBeforeDeath;
    void Start()
    {
        GameObject.Destroy(this, delayBeforeDeath);
    }


}
