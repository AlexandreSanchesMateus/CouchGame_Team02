using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] MeshRenderer actualRenderer;
    [SerializeField] Material basicMaterial;
    [SerializeField] Material alarmMaterial;

    void Start()
    {
        actualRenderer = GetComponent<MeshRenderer>();
        actualRenderer.material = basicMaterial;

        EnigmeManager.instance.OnAlarmeEnable += EnableAlarme;
    }

    // Update is called once per frame

    public void EnableAlarme(float duration)
    {
        StartCoroutine(AlarmeStart(duration));
    }

    IEnumerator AlarmeStart(float alarmDuration)
    {

        actualRenderer.material = alarmMaterial;
        yield return new WaitForSeconds(alarmDuration);
        actualRenderer.material = basicMaterial;
    }
}
