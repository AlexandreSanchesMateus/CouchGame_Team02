using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LabyrintheWallEditor : EditorWindow
{
    public GUISkin customSkin;

    private static List<Labyrinthe.Slot> editorGrid = new List<Labyrinthe.Slot>();

    public static void InitWindow()
    {
        LabyrintheWallEditor window = GetWindow<LabyrintheWallEditor>();
        window.titleContent = new GUIContent("Labyrinthe Wall");
        window.maxSize = new Vector2(600, 600);
        window.minSize = window.maxSize;
        if(Labyrinthe.grid.Count == 0)
        {
            editorGrid.Clear();
            for (int i = 0; i < (int)Mathf.Pow(Labyrinthe.gridSize, 2); i++)
            {
                editorGrid.Add(new Labyrinthe.Slot());
            }
        }
        window.Show();
    }

    private void OnGUI()
    {
        GUI.skin = customSkin;
        GUILayout.Space(20);

        GUILayout.Label("Colors indicator :");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Gris -> BLOCK", GUILayout.ExpandWidth(true));
        GUILayout.Label("Rouge -> HACKER", GUILayout.ExpandWidth(true));
        GUILayout.Label("Bleu -> ROBBER", GUILayout.ExpandWidth(true));
        GUILayout.Label("Vert -> BOTH", GUILayout.ExpandWidth(true));
        GUILayout.Label("Violet -> EITHER", GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal("window", GUILayout.ExpandHeight(true));
        

        GUILayout.BeginVertical();
        int value = 0;
        for (int i = 0; i < Labyrinthe.gridSize; i++)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            for (int y = 0; y < Labyrinthe.gridSize; y++)
            {
                GUILayout.BeginVertical(GUILayout.ExpandWidth(false));
                GUILayout.Box("", GUILayout.Width(50), GUILayout.Height(50));
                if (GUILayout.Button("", editorGrid[i + y].bottomWall.ToString(), GUILayout.Width(50), GUILayout.Height(10)))
                {
                    editorGrid[0] = IncrementAccess(editorGrid[i + y], false);
                    Debug.Log("Ligne : " + i + " Colone : " + y + "Valeur : " + (y - 1) * Labyrinthe.gridSize + i);
                    //Debug.Log(editorGrid[i + y].rightWall.ToString() + " " + editorGrid[i + y].bottomWall.ToString());
                }
                GUILayout.EndVertical();
                if(GUILayout.Button("", editorGrid[i + y].rightWall.ToString(), GUILayout.Width(10), GUILayout.Height(50)))
                {
                    editorGrid[0] = IncrementAccess(editorGrid[i + y], true);
                    Debug.Log("Ligne : " + i + " Colone : " + y + "Valeur : " + value);
                    //Debug.Log(editorGrid[i + y].rightWall.ToString() + " " + editorGrid[i + y].bottomWall.ToString());
                }
                value ++;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();


        GUILayout.EndHorizontal();
        if (GUILayout.Button("Save"))
        {
            
        }
    }

    public Labyrinthe.Slot IncrementAccess(Labyrinthe.Slot source, bool rightWall)
    {
        Labyrinthe.Slot toReturn = new Labyrinthe.Slot(source);

        if(rightWall)
            toReturn.rightWall = (Labyrinthe.ACCESS)((source.rightWall.GetHashCode() + 1) % 5);
        else
            toReturn.bottomWall = (Labyrinthe.ACCESS)((source.bottomWall.GetHashCode() + 1) % 5);

        return toReturn;
    }

    /*private GUIStyle GetGUIStyle(Labyrinthe.ACCESS access)
    {
        GUIStyle toReturn = new GUIStyle(GUI.skin.button);

        switch (access)
        {
            case Labyrinthe.ACCESS.BLOCK:
                toReturn.normal.textColor = Color.grey;
                break;
            case Labyrinthe.ACCESS.HACKER:
                toReturn.normal.textColor = Color.grey;
                break;
            case Labyrinthe.ACCESS.ROBBER:
                toReturn.normal.textColor = Color.blue;
                break;
            case Labyrinthe.ACCESS.BOTH:
                toReturn.normal.textColor = Color.green;
                break;
            case Labyrinthe.ACCESS.EITHER:
                toReturn.normal.textColor = Color.red;
                break;
        }

        return toReturn;
    }*/
}
