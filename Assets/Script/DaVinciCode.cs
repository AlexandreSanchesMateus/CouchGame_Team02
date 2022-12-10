using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class DaVinciCode : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject vcam;

    private AudioSource audioSource;
    [SerializeField] private AudioClip enterClip;
    [SerializeField] private AudioClip outClip;
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip inputClip;
    [SerializeField] private AudioClip validClip;

    [SerializeField] private GameObject[] arrow;
    private int idArrow = 0;

    private List<string> lettreFill;
    private List<List<string>> lettreObligatoire;

    [SerializeField] private MOTS mot;
    private string motATrouve;
    private List<List<string>> actualLettre;
    private string aAjouter;
    private char[] lettreCharAffiche;
    private int[] idPrecedent = new int[7] {0,0,0,0,0,0,0};

    [SerializeField] private TMP_Text text;
    private int colonne;
    private int lettre;

    private int idColonne = 0;
    private bool isOpen = false;
    private bool isValid = false;


    // Start is called before the first frame update
    void Start()
    {
        // Initialisation
        audioSource =  GetComponent<AudioSource>();
        audioSource.clip = enterClip;
        // Lettre de remplissage
        lettreFill = new List<string>() { "B ", "D ", "F ", "M ", "G ", "P", "H", "U", "J", "Q", "S"};

        // Lettre pour le mot
        lettreObligatoire = new List<List<string>>();
        lettreObligatoire.Add(new List<string> { "V", "R", "C", "L"});
        lettreObligatoire.Add(new List<string> { "N", "T", "O"});
        lettreObligatoire.Add(new List<string> { "R", "T", "A", "E", "O" });

        // Tableau de toutes les lettres choisis
        actualLettre = new List<List<string>>();
        actualLettre.Add(new List<string> { "A", "C", "S", "D", "P" });
        actualLettre.Add(new List<string> { "P", "O", "A", "E", "N" });
        actualLettre.Add(new List<string> { "L", "U", "R", "S", "I" });
        actualLettre.Add(new List<string> { "A", "E", "S", "Q", "T" });
        actualLettre.Add(new List<string> { "E", "S", "I", "E", "U", "A" });
        actualLettre.Add(new List<string> { "R", "I", "N", "U", "E", "G" });
        actualLettre.Add(new List<string> { "T", "R", "S", "E" });



        mot = (MOTS)Random.Range(0, 11);
        Debug.Log(mot);

        // Info sur lettre obligatoire pour le mot
        switch (mot) 
        {
            case MOTS.APLANIR:
                motATrouve = "APLANIR";
                colonne = 1;
                lettre = 0;
                break;

            case MOTS.APLATIR:
                motATrouve = "APLATIR";
                colonne = 1;
                lettre = 1;
                break;

            case MOTS.DESSERT:
                motATrouve = "DESSERT";
                colonne = 2;
                lettre = 0;
                break;

            case MOTS.DESERTS:
                motATrouve = "DESERTS";
                colonne = 2;
                lettre = 1;
                break;

            case MOTS.DOUCEUR:
                motATrouve = "DOUCEUR";
                colonne = 0;
                lettre = 2;
                break;

            case MOTS.DOULEUR:
                motATrouve = "DOULEUR";
                colonne = 0;
                lettre = 3;
                break;

            case MOTS.POISSON:
                motATrouve = "POISSON";
                colonne = 2;
                lettre = 4;
                break;

            case MOTS.POISONS:
                motATrouve = "POISONS";
                colonne = 1;
                lettre = 2;
                break;

            case MOTS.SERVAGE:
                motATrouve = "SERVAGE";
                colonne = 0;
                lettre = 0;
                break;

            case MOTS.SEVRAGE:
                motATrouve = "SEVRAGE";
                colonne = 0;
                lettre = 1;
                break;

            case MOTS.PARTIEL:
                motATrouve = "PARTIEL";
                colonne = 2;
                lettre = 3;
                break;

            case MOTS.PARTIAL:
                motATrouve = "PARTIAL";
                colonne = 2;
                lettre = 2;
                break;
        }
        ImplementRandomLetter();
    }

    void Update()
    {

    }

    // Recupére la letre obligatoire pour le mot
    string RecupLettreObli(int colonne, int lettre)
    {
        List<string> actualList = lettreObligatoire[colonne];
        return actualList[lettre];
    }

    // Affiche la liste de lettres actuel
    void ApplyText()
    {
        string afficher;
        if (lettreCharAffiche == null)
        {
            for(int index = 0; index < 7; index++)
            {
                aAjouter += actualLettre[index][0];
            }
            lettreCharAffiche = new char[aAjouter.Length];
            for(int i = 0; i < aAjouter.Length; i++)
            {
                lettreCharAffiche[i] = aAjouter[i];
            }
            afficher = new string(lettreCharAffiche);
            Debug.Log(afficher);
        }
        else
        {
            aAjouter = actualLettre[idColonne][idPrecedent[idColonne]];
            lettreCharAffiche[idColonne] = aAjouter[0];
            afficher = new string(lettreCharAffiche);
            Debug.Log(aAjouter[0]);
            Debug.Log(lettreCharAffiche[idColonne]);
            Debug.Log(afficher);
            Verif(afficher);

            //textAffiche[0] = actualText[colonne][lettre];
            //string ljl = textAffiche.Substring(1) + 'A' + textAffiche.Substring(1);
        }
        text.text = afficher;

    }

    // Complete le tableau delettre avec des aléatoires
    void ImplementRandomLetter()
    {
        actualLettre[colonne + 3].Add(RecupLettreObli(colonne, lettre));

        for(int i = 0; i < 7; i++)
        {
            List<string> list = actualLettre[i];
            int nbManquant = 7 - list.Count;
            for(int j = 0; j < nbManquant; j++)
            {
                actualLettre[i].Add(lettreFill[Random.Range(0, 10)]);
            }
        }
        ApplyText();
    }

    void Verif(string motATest)
    {
        if (motATest.Equals(motATrouve))
        {
            Debug.Log("Bien ouej");
        }
    }

    public void OnItemHover()
    {
        GUIManager.instance.EnableUseGUI(true);
    }

    public void OnItemExit()
    {
        GUIManager.instance.EnableUseGUI(false);
    }

    public void OnInteract()
    {
        if (!isOpen)
        {
            PlayAudio(enterClip);
            StartCoroutine(Delay());
            GUIManager.instance.EnableUseGUI(false);
            GUIManager.instance.MoveHandWorldToScreenPosition(arrow[idArrow].transform.position);
            vcam.SetActive(true);
            PlayerControllerProto2.enablePlayerMovement = false;
            isOpen = true;
        }
        else
        {
            if(idArrow < 7)
            {
                idPrecedent[idColonne]++;
                if(idPrecedent[idColonne] == 7)
                {
                    idPrecedent[idColonne] = 0;
                }
                ApplyText();
            }
            else if(idArrow >= 7)
            {
                idPrecedent[idColonne]--;
                if (idPrecedent[idColonne] == -1)
                {
                    idPrecedent[idColonne] = 6;
                }
                ApplyText();
            }
        }
    }

    public void OnReturn()
    {
        {
            PlayAudio(outClip);
            isOpen = false;
            GUIManager.instance.EnableUseGUI(false);
            GUIManager.instance.EnableHand(false);
            vcam.SetActive(false);
            PlayerControllerProto2.enablePlayerMovement = true;
            StopAllCoroutines();
        }
    }

    public void OnActions(Vector2 action, Vector2 joystick)
    {
        if (!isOpen || isValid || action == Vector2.zero)
            return;

        if (action == Vector2.up)
        {
            if (idArrow - 7 >= 0)
            {
                idArrow -= 7;
            }
            
        }
        else if (action == Vector2.down)
        {
            if (idArrow + 7 < 14)
            {
                idArrow += 7;
            }
        }
        else if (action == Vector2.right)
        {
            idArrow += 1;
            idColonne += 1;
            if (idArrow == 7)
            {
                idArrow = 0;
                idColonne = 0;
            }
            else if (idArrow == 14)
            {
                idArrow = 7;
                idColonne = 0;
            }
        }
        else if (action == Vector2.left)
        {
            idArrow -= 1;
            idColonne -= 1;
            if (idArrow == -1)
            {
                idArrow = 6;
                idColonne = 6;
            }
            else if (idArrow == 6)
            {
                idArrow = 13;
                idColonne = 6;
            }
        }

        PlayAudio(hoverClip);
        Debug.Log("id Arrow : "+idArrow);
        Debug.Log("id Colonne : "+idColonne);

        GUIManager.instance.MoveHandWorldToScreenPosition(arrow[idArrow].transform.position);
    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnRightShoulder()
    {
        throw new System.NotImplementedException();
    }

    public void OnHoldReturn()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        GUIManager.instance.MoveHandWorldToScreenPosition(arrow[idArrow].transform.position);
    }

    private enum MOTS
    {
        APLANIR,
        APLATIR,
        DESSERT,
        DESERTS,
        DOUCEUR,
        DOULEUR,
        POISSON,
        POISONS,
        SERVAGE,
        SEVRAGE,
        PARTIEL,
        PARTIAL
    }
}
