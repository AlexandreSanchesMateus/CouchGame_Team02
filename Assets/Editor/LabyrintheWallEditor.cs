using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LabyrintheWallEditor : EditorWindow
{
    public GUISkin customSkin;

    private static List<Slot> editorGrid = new List<Slot>();
    private static int editorGridSizeX;
    private static int editorGridSizeY;

    private static int editorPlayerPosId = -1;
    private static int editorLabyrinthEndID = -1;

    private static int Xsize;
    private static int Ysize;

    private bool isSetingStartPos = false;
    private bool isSetingEndPos = false;

    public static void InitWindow()
    {
        LabyrintheWallEditor window = GetWindow<LabyrintheWallEditor>();
        window.titleContent = new GUIContent("Labyrinthe Wall");
        window.minSize = new Vector2(800, 630);
        
        if(Labyrinthe.grid.Count != 0)
        {
            editorGrid = new List<Slot>(Labyrinthe.grid);
            editorGridSizeX = Labyrinthe.gridSizeX;
            editorGridSizeY = Labyrinthe.gridSizeY;
        }
        else
        {
            editorGrid = new List<Slot>();
            editorGridSizeX = 7;
            editorGridSizeY = 6;

            for (int i = 0; i < editorGridSizeX * editorGridSizeY; i++)
                editorGrid.Add(new Slot());
        }

        Xsize = editorGridSizeX;
        Ysize = editorGridSizeY;

        window.Show();
    }

    private void OnGUI()
    {
        GUI.skin = customSkin;

        GUILayout.BeginHorizontal();


        // ---------- ACTION SECTION ---------- //

        GUILayout.BeginVertical("window", GUILayout.Width(150));

        // RESIZE FUNCTION
        GUILayout.Label("Grid Size X");
        Xsize = EditorGUILayout.IntField(Xsize);
        GUILayout.Label("Grid Size Y");
        Ysize = EditorGUILayout.IntField(Ysize);

        if (GUILayout.Button("Resize"))
        {
            editorGridSizeX = Xsize;
            editorGridSizeY = Ysize;

            editorGrid.Clear();
            for (int i = 0; i < editorGridSizeX * editorGridSizeY; i++)
                editorGrid.Add(new Slot());
        }
        GUILayout.Space(5);

        // SET THE PLAYER POS OR SET THE LABYRINTH END
        if (GUILayout.Button("Set Start Pos"))
        {
            isSetingStartPos = !isSetingStartPos;
            isSetingEndPos = false;
        }
        if (GUILayout.Button("Set End Pos"))
        {
            isSetingEndPos = !isSetingEndPos;
            isSetingStartPos = false;
        }

        // INFORMATIONS
        GUILayout.Space(10);
        GUILayout.Label("BLOCK : prevent the player to get out of the grid. Do not restart the labyrinth.", "BLOCK");
        GUILayout.Label("HACKER : only the hacker can pass through this wall. Otherwise, the labyrinth restart.", "HACKER");
        GUILayout.Label("ROBBER : only the robber can pass through this wall. Otherwise, the labyrinth restart.", "ROBBER");
        GUILayout.Label("BOTH : the hacker and the robber can pass through.", "BOTH");
        GUILayout.Label("EITHER : none of the players can pass through. Restart the labyrinth went trigger.", "EITHER");

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save Labyrinthe"))
        {
            Labyrinthe.grid = new List<Slot>(editorGrid);
            Labyrinthe.gridSizeX = editorGridSizeX;
            Labyrinthe.gridSizeY = editorGridSizeY;
            Debug.Log("Labyrinthe save succesfully");
        }

        GUILayout.EndVertical();

        // ---------- GRID SECTION ---------- //

        GUILayout.BeginVertical("window", GUILayout.ExpandHeight(true));
        for (int y = 0; y < editorGridSizeY; y++)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            for (int x = 0; x < editorGridSizeX; x++)
            {
                int id = y * editorGridSizeX + x;

                // SET THE PLAYER POS OR SET THE LABYRINTH END
                if (isSetingStartPos || isSetingEndPos)
                {
                    if (GUILayout.Button("", "setpos", GUILayout.Width(50), GUILayout.Height(50)))
                        Debug.Log("Set position to " + id);
                }
                // SET THE WALL OF THE LABYRINTH
                else
                {

                    GUILayout.BeginVertical(GUILayout.ExpandWidth(false));

                    // IMAGE DE LA CASE
                    GUILayout.Box("", GUILayout.Width(50), GUILayout.Height(50));

                    // BOUTON DU MUR DU DESSOUS
                    if (y != editorGridSizeY - 1)
                    {
                        if (GUILayout.Button("", editorGrid[id].bottomWall.ToString(), GUILayout.Width(50), GUILayout.Height(10)))
                            editorGrid[id].IncrementAccess(false);
                    }

                    GUILayout.EndVertical();

                    // BOUTON DU MUR DE DROITE
                    if (x != editorGridSizeX - 1)
                    {
                        if (GUILayout.Button("", editorGrid[id].rightWall.ToString(), GUILayout.Width(10), GUILayout.Height(50)))
                            editorGrid[id].IncrementAccess(true);

                    }
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }
}
