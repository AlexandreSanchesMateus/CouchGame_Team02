using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeurManager : MonoBehaviour
{
    [SerializeField]
    private InspectedObject grabObj;

    void Start()
    {
        grabObj.OnGrab += OnGrabDisquette;
    }

    private void OnGrabDisquette(InspectedObject source)
    {
        source.OnGrab -= OnGrabDisquette;
        gameObject.layer = LayerMask.GetMask("Default");
        Debug.Log(source.name + " a été retirer.");
    }
}
