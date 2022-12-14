using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanierDeBasket : MonoBehaviour
{
    [SerializeField] ParticleSystem vfx;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("basketBall"))
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
        }
    }
}
