using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour
{
    [SerializeField] private Transform pointOfView;
    public Vector3 getPOF { get { return pointOfView.position; }}
}
