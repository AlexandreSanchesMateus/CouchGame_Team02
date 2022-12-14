using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbianceBound : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot AMB_Snapshot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.ChangeAmbiance(AMB_Snapshot);
        }
    }
}
