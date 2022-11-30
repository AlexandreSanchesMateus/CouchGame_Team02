using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeurManager : MonoBehaviour
{
    [SerializeField]
    private List<InspectedObject> grabObj = new List<InspectedObject>();

    void Start()
    {
        foreach(InspectedObject other in grabObj)
        {
            other.OnGrab += OnGrabDisquette;
        }
    }

    private void OnGrabDisquette(InspectedObject source)
    {
        source.OnGrab -= OnGrabDisquette;
    }
}
