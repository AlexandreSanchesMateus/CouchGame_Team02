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
        GUILayout.Label("GUI Settings");

        GUILayout.BeginHorizontal();
        GUILayout.Label("GUIhover");
        myObject.GUIhover = (GameObject)EditorGUILayout.ObjectField(myObject.GUIhover, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("labyrinth GUI");
        myObject.labirinthe = (GameObject)EditorGUILayout.ObjectField(myObject.GUIhover, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("labyrinth");
        LabyrinthManager.labyrinth = (ScrLabyrinth)EditorGUILayout.ObjectField(LabyrinthManager.labyrinth, typeof(ScrLabyrinth), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("Grid Settings");
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Grid's wall Settings"))
        {
            LabyrintheWallEditor.InitWindow(LabyrinthManager.labyrinth);
        }
        GUILayout.Space(10);

        if (LabyrinthManager.labyrinth)
        {
            GUILayout.Label("Current labyrinthe :");
            GUILayout.Label("Total size : " + LabyrinthManager.labyrinth.gridSizeX + " x " + LabyrinthManager.labyrinth.gridSizeY);
            GUILayout.Label("Total of slot : " + LabyrinthManager.labyrinth.grid.Count);
        }
        else
            GUILayout.Label("--- No labyrinthe saved ---");
    }
}
