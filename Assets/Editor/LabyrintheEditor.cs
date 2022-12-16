using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LabyrinthManager))]
public class LabyrintheEditor : Editor
{
    private LabyrinthManager myObject;

    public static string[] EXCLUDED_PROPERTIES = { "gridSize", "idPlayerSlot" };

    private void OnEnable()
    {
        this.myObject = (LabyrinthManager)this.target;
    }

    public override void OnInspectorGUI()
    {

        GUILayout.Label("Generale Settings");
        GUILayout.Label("Virtual Cam");
        myObject.vcam = (GameObject)EditorGUILayout.ObjectField(myObject.vcam, typeof(GameObject), true);

        GUILayout.Label("Audio Set Up");
        myObject.setUp = (AudioClip)EditorGUILayout.ObjectField(myObject.setUp, typeof(AudioClip), true);
        GUILayout.Label("Hacker move");
        myObject.SFXHackerMove = (AudioClip)EditorGUILayout.ObjectField(myObject.SFXHackerMove, typeof(AudioClip), true);
        GUILayout.Label("Robber move");
        myObject.SFXRobberMove = (AudioClip)EditorGUILayout.ObjectField(myObject.SFXRobberMove, typeof(AudioClip), true);
        GUILayout.Label("fail");
        myObject.SFXFail = (AudioClip)EditorGUILayout.ObjectField(myObject.SFXFail, typeof(AudioClip), true);
        GUILayout.Label("sucess");
        myObject.SFXSucess = (AudioClip)EditorGUILayout.ObjectField(myObject.SFXSucess, typeof(AudioClip), true);

        GUILayout.Label("indicator");
        myObject.indicator = (MeshRenderer)EditorGUILayout.ObjectField(myObject.indicator, typeof(MeshRenderer), true);

        GUILayout.Label("material");
        myObject.indicatorMat = (Material)EditorGUILayout.ObjectField(myObject.indicatorMat, typeof(Material), true);

        GUILayout.Label("Parent slot");
        myObject.chestParent = (Transform)EditorGUILayout.ObjectField(myObject.chestParent, typeof(Transform), true);

        GUILayout.Label("Begin Screen");
        myObject.beginScreen = (GameObject)EditorGUILayout.ObjectField(myObject.beginScreen, typeof(GameObject), true);

        GUILayout.Label("End Screen");
        myObject.sucsessScreen = (GameObject)EditorGUILayout.ObjectField(myObject.sucsessScreen, typeof(GameObject), true);

        GUILayout.Label("king");
        myObject.king = (GameObject)EditorGUILayout.ObjectField(myObject.king, typeof(GameObject), true);

        GUILayout.Space(20);
        GUILayout.Label("Ref Labyrinthe");

        GUILayout.Label("Labyrinthe");
        myObject.labyrinth = (ScrLabyrinth)EditorGUILayout.ObjectField(myObject.labyrinth, typeof(ScrLabyrinth), true);

        GUILayout.Space(5);

        GUILayout.Label("Grid Settings");
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Grid's wall Settings"))
        {
            LabyrintheWallEditor.InitWindow(myObject.labyrinth, myObject);
        }
        GUILayout.Space(10);

        if (myObject.labyrinth)
        {
            GUILayout.Label("Current labyrinthe :");
            GUILayout.Label("Total size : " + myObject.labyrinth.gridSizeX + " x " + myObject.labyrinth.gridSizeY);
            GUILayout.Label("Total of slot : " + myObject.labyrinth.grid.Count);
        }
        else
            GUILayout.Label("--- No labyrinthe saved ---");
    }
}
