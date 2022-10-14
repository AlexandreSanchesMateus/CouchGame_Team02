using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screensholder : MonoBehaviour
{
    public MiniGame game;
    private float radius;
    public GameObject screenPrefabs;
    public int number;
    public int y;

    private float currentrot;
    public float rotToAdd;

    public List<GameObject> screens = new List<GameObject>();
    public List<MiniGame> minigames = new List<MiniGame>();
    public List<Material> mat = new List<Material>();
    private void Start()
    {
        
        radius = number*0.30f;
        rotToAdd = 360/number;
        for (int i = 0; i < number; i++)
        {
            if (i == 0)
            {
                currentrot = 90;
            }
            else
            {
                currentrot -= rotToAdd;
            }
            float angle = i * Mathf.PI * 2f / number;
            Vector3 newPos = new Vector3(transform.position.x+ Mathf.Cos(angle) * radius, transform.position.y + y, transform.position.z + Mathf.Sin(angle) * radius);
            GameObject screenGO = Instantiate(screenPrefabs, newPos, Quaternion.Euler(0,currentrot,0), transform);
            if(i==0)
            {
                screenGO.GetComponent<Screen>().game = minigames[0];
                //screenGO.GetComponent<Renderer>().material = mat[0].;
                //RenderTexture rT = new RenderTexture(256,256,8);
                //minigames[0].Cam.targetTexture = rT;
                //minigames[0].Cam.Render();
            }
            else
            {
                screenGO.GetComponent<Renderer>().material = mat[Random.Range(0, 2)];
            }
            screens.Add(screenGO);
        }
    }

}
