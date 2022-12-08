using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAlarme : MonoBehaviour
{
    [SerializeField] Color basicColor;
    [SerializeField] Color alarmColor;
    private Light _light;
    
    void Start()
    {
        _light = GetComponent<Light>();
        EnigmeManager.instance.OnAlarmeEnable += LightChangeColor;
        _light.color = basicColor;
    }

    void LightChangeColor (float duration)
    {
        StartCoroutine(Lescouleurschangent(duration));
    }

    IEnumerator Lescouleurschangent(float duration)
    {
        _light.color = alarmColor;
        yield return new WaitForSeconds(duration);
        _light.color = basicColor;
    }
}
