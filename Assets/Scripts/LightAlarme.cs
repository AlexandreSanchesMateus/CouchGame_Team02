using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAlarme : MonoBehaviour
{
    [SerializeField] Color basicColor;
    [SerializeField] Color alarmColor;
    private Light _light;

    private float defautIntensity;
    
    void Start()
    {
        _light = GetComponent<Light>();
        EnigmeManager.instance.OnAlarmeEnable += LightChangeColor;
        EnigmeManager.instance.OnLightEnable += EnableLight;
        _light.color = basicColor;
        defautIntensity = _light.intensity;
    }

    void LightChangeColor (float duration)
    {
        StartCoroutine(Lescouleurschangent(duration));
    }

    void EnableLight(bool active)
    {
        if (active)
            _light.intensity = defautIntensity;
        else
            _light.intensity = 0;
    }

    IEnumerator Lescouleurschangent(float duration)
    {
        _light.color = alarmColor;
        yield return new WaitForSeconds(duration);
        _light.color = basicColor;
    }
}
