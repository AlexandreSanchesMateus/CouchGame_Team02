using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactItems : MonoBehaviour
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private ParticleSystem vfx;
    [SerializeField] private float magnitude = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > magnitude)
        {
            if (sfx != null)
                sfx.Play();
            if (vfx != null)
                vfx.Play();
        }
    }
}
