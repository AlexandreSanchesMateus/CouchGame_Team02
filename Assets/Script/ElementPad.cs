using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ElementPad : MonoBehaviour, IInteractible
{
    public static ElementPad instance { get; set; }
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
    [SerializeField] private Sprite PopCoin;
    [SerializeField] private Sprite DogeCoins;
    [SerializeField] private Sprite SusCoins;
    [SerializeField] private Sprite PepeCoins;

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

    public List<Serveur> allServeur = new List<Serveur>();

    [Header("Audio")]
    [SerializeField] private AudioClip sucsess;
    [SerializeField] private AudioClip fail;
    [SerializeField] private AudioClip[] validations;
    [SerializeField] private AudioClip[] hover;
    [SerializeField] private AudioClip inputClip;
    [SerializeField] private AudioClip open;
    private AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        isOpen = false;
        idKeyBraq = 0;
        gameObject.layer = 2;
        audioSource = GetComponent<AudioSource>();
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

        audioSource.PlayOneShot(inputClip);
        /*idKeyBraq = Mathf.Clamp(idKeyBraq, 0, keys.Length - 1);*/
        GUIManager.instance.MoveHandWorldToScreenPosition(keys[idKeyBraq].transform.position);
    }

    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
    }

    public void OnItemHover()
    {
        audioSource.PlayOneShot(hover[Random.Range(0, hover.Length)]);
        GUIManager.instance.EnableUseGUI(true);
    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            audioSource.PlayOneShot(open);
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
        yield return new WaitForSeconds(0.5f);
        /*display.sprite = null;
        display.color = Color.green;*/
        GUIManager.instance.EnableHand(false);
        vcam.SetActive(false);

        LabyrinthManager.instance.InitLabyrintheScreen();
        EnigmeManager.instance.SuccessElementPad();

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
        //display.sprite = GetElementSprite(etapes[actualEtape].situations[idSituation].element);

        //Set goodkey
        key = currentSituation.goodkey;

        if (currentSituation.goodkey < 0)
        {
            key = previousElement[Mathf.Abs(currentSituation.goodkey) - 1];
        }
    }

    public void CheckInput(bool autoLoose = false)
    {
        
        if((braqHavePlayed && hackHavePlayed) || autoLoose)
        {
            

            if ((idKeyBraq == key && idKeyHacker == key) && !autoLoose)
            {
                lights[actualEtape].GetComponent<MeshRenderer>().material = greenMat;
                previousElement.Add(key);
                actualEtape++;
                if (actualEtape == 6)
                {
                    // Destroy(lockey);
                    audioSource.PlayOneShot(sucsess);
                    StartCoroutine(PanelComplet());
                    return;
                }
                audioSource.PlayOneShot(validations[actualEtape - 2]);
            }
            else
            {
                for (int i = actualEtape; i > -1; i--)
                {
                    lights[i].GetComponent<MeshRenderer>().material = redMat;
                }
                audioSource.PlayOneShot(fail);
                previousElement.Clear();
                actualEtape = 0;
                if (idKeyHacker == key)
                {
                    HackerController.instance.WrongAnswerLights();
                }
            }
            braqHavePlayed = false;
            hackHavePlayed = false;

            StopCoroutine(corout);
            UpdateSituationAndKey();
        }

        if(braqHavePlayed || hackHavePlayed)
        {
            if(!timerIsRunning)
                corout = StartCoroutine(cooldown());
        }
    }

    IEnumerator cooldown()
    {
        timerIsRunning = true;
        yield return new WaitForSeconds(time);
        timerIsRunning = false;
        CheckInput(true);
    }

    public void OnRightShoulder() { }

    public void OnHoldReturn() { }

    private Sprite GetElementSprite(ELEMENTS _element)
    {
        switch (_element)
        {
            case ELEMENTS.POP:
                return PopCoin;

            case ELEMENTS.DOGE:
                return DogeCoins;

            case ELEMENTS.SUS:
                return SusCoins;

            case ELEMENTS.PEPE:
                return PepeCoins;
        }

        return null;
    }

    public void InitLabyrintheScreen(Serveur other)
    {
        allServeur.Remove(other);
        if (allServeur.Count <= 0)
        {
            AudioManager.instance.IncreaseMusicLevel();
            gameObject.layer = 3;
            EnigmeManager.instance.OnLightEnable(true);
        }
    }
}
