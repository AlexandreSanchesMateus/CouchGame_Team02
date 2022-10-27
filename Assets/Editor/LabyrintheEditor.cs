using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Labyrinthe))]
public class LabyrintheEditor : Editor
{
    private Labyrinthe myObject;

    public static string[] EXCLUDED_PROPERTIES = { "gridSize", "idPlayerSlot" };

    private void OnEnable()
    {
        this.myObject = (Labyrinthe)this.target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("GUI Settings");
        GUILayout.BeginHorizontal();
        GUILayout.Label("GUIhover");
        myObject.GUIhover = (GameObject)EditorGUILayout.ObjectField(myObject.GUIhover, typeof(GameObject), true);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("labirinthe");
        myObject.labirinthe = (GameObject)EditorGUILayout.ObjectField(myObject.GUIhover, typeof(GameObject), true);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.Label("Grid Settings");
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Grid's wall Settings"))
        {
            LabyrintheWallEditor.InitWindow();
        }
        GUILayout.Space(10);

        GUILayout.Label("Current labyrinthe :");
        GUILayout.Label("Total size : ");
        GUILayout.Label("Total of slot : " + Labyrinthe.grid.Count);
    }
}
