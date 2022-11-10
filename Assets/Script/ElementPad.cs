using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ElementPad : MonoBehaviour, IInteractible
{
    public static ElementPad instance;
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
    private int idKeyBraq, key;
    public int idKeyHacker;
    private bool braqHavePlayed, isOpen;
    public bool hackHavePlayed;
    private int actualEtape;

    [SerializeField] private float time;
    private Coroutine corout;
    private bool timerIsRunning = false;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        isOpen = false;
        idKeyBraq = 0;
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isOpen || action == Vector2.zero)
            return;

        if (action.x > 0)
        {
            idKeyBraq++;
            if(idKeyBraq == 4)
            {
                idKeyBraq = 0;
            }
        }
        else if(action.x < 0)
        {
            idKeyBraq--;
            if(idKeyBraq < 0)
            {
                idKeyBraq = 3;
            }
        }

        /*idKeyBraq = Mathf.Clamp(idKeyBraq, 0, keys.Length - 1);*/
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKeyBraq].transform.position);
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
        if (!isOpen)
        {
            braqHavePlayed = false;
            hackHavePlayed = false;

            StartCoroutine(Delay());
            GUIManager.instance.EnableUseGUI(false);
            vcam.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;

            UpdateSituationAndKey();
            return;
        }

        // SI changement GD
        // currentSituation = etapes[actualEtape].situations[Random.Range(0, etapes[actualEtape].situations.Length - 1)];

        braqHavePlayed = true;
        CheckInput();
        
        
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
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKeyBraq].transform.position);
    }

    public void UpdateSituationAndKey()
    {
        //Set situation
        int idSituation = Random.Range(0, 3);
        currentSituation = etapes[actualEtape].situations[idSituation];
        display.text = etapes[actualEtape].situations[idSituation].element.ToString();

        //Set goodkey
        key = currentSituation.goodkey;

        if (currentSituation.goodkey < 0)
        {
            key = previousElement[Mathf.Abs(currentSituation.goodkey) - 1];
        }
    }

    public void CheckInput(bool autoLoose = false)
    {
        
        if(braqHavePlayed && hackHavePlayed || autoLoose)
        {
            StopCoroutine(corout);
            if (idKeyBraq == key && idKeyHacker == key && !autoLoose)
            {
                lights[actualEtape].GetComponent<MeshRenderer>().material = greenMat;
                previousElement.Add(key);
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
                for (int i = actualEtape; i > -1; i--)
                {
                    lights[i].GetComponent<MeshRenderer>().material = redMat;
                }
                previousElement.Clear();
                actualEtape = 0;
            }

            braqHavePlayed = false;
            hackHavePlayed = false;

            UpdateSituationAndKey();
        }
        else if(braqHavePlayed || hackHavePlayed)
        {
            if(!timerIsRunning)
                corout = StartCoroutine(cooldown());
        }
    }
    IEnumerator cooldown()
    {
        timerIsRunning = true;
        yield return new WaitForSeconds(time);
        CheckInput(true);
        timerIsRunning = false;
    }
}
