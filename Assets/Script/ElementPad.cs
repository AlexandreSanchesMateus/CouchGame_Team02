using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ElementPad : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject vcam;

    // [SerializeField] private GameObject GUIhover;
    //[SerializeField] private GameObject Pad;

    [Header("General")]
    [SerializeField] private GameObject lockey;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private GameObject[] keys;

    [Header("Led")]
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private GameObject[] lights;

    [Header("Réponses")]
    [SerializeField] private List<Etapes> etapes;

    private Situation currentSituation;

    private List<int> previousElement = new List<int>();
    private int idKey;
    private bool isOpen;
    private int actualEtape;


    void Start()
    {
        isOpen = false;
        idKey = 0;
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isOpen || action == Vector2.zero)
            return;

        if (action == Vector2.right)
        {
            idKey++;
            if(idKey == 4)
            {
                idKey = 0;
            }
        }
        else if(action == Vector2.left)
        {
            idKey--;
            if(idKey < 0)
            {
                idKey = 3;
            }
        }

        idKey = Mathf.Clamp(idKey, 0, keys.Length - 1);
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKey].transform.position);
    }

    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
    }

    public void OnItemHover()
    {
        GUIManager.instance.EnableUseGUI(true);
    }

    public void OnInteract()
    {
        int idSituation = 0;
        if (!isOpen)
        {
            StartCoroutine(Delay());
            GUIManager.instance.EnableUseGUI(false);
            vcam.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
            idSituation = Random.Range(0, 3);
            currentSituation = etapes[actualEtape].situations[idSituation];
            display.text = etapes[actualEtape].situations[idSituation].element.ToString();
            return;
        }

        // SI changement GD
        // currentSituation = etapes[actualEtape].situations[Random.Range(0, etapes[actualEtape].situations.Length - 1)];
        int key = currentSituation.goodkey;

        if(currentSituation.goodkey < 0)
        {
            key = previousElement[Mathf.Abs(currentSituation.goodkey) - 1];
        }

        if (idKey == key)
        {
            lights[actualEtape].GetComponent<MeshRenderer>().material = greenMat;
            previousElement.Add(idKey);
            actualEtape++;
            if (actualEtape == 6)
            {
                // Destroy(lockey);
                StartCoroutine(PanelComplet());
                display.text = "Well done";
                return;
            }
        }
        else
        {
            for(int i = actualEtape; i > -1; i--)
            {
                lights[i].GetComponent<MeshRenderer>().material = redMat;
            }
            previousElement.Clear();
            actualEtape = 0;
        }
        
        idSituation = Random.Range(0, 3);
        currentSituation = etapes[actualEtape].situations[idSituation];
        display.text = etapes[actualEtape].situations[idSituation].element.ToString();
    }

    public void OnReturn()
    {
        isOpen = false;
        vcam.SetActive(false);
        GUIManager.instance.EnableHand(false);
        PlayerControllerProto2.enablePlayerMovement = true;
    }

    private IEnumerator PanelComplet()
    {
        GUIManager.instance.EnableHand(false);
        vcam.SetActive(false);
        gameObject.layer = 0;
        yield return new WaitForSeconds(2);
        PlayerControllerProto2.enablePlayerMovement = true;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKey].transform.position);
    }

}
