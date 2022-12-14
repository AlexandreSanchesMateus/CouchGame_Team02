using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class startCutScene : MonoBehaviour
{
    private bool hasplayed = false;
    [SerializeField] private GameObject cutscene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasplayed && EnigmeManager.instance.lastegnimedone)
        {
            hasplayed = true;
            cutscene.SetActive(true);
            StartCoroutine(waitToload());
        }
    }
    private IEnumerator waitToload()
    {
        yield return new WaitForSeconds(17f);
        SceneManager.LoadScene("MainMenu");
    }
}
