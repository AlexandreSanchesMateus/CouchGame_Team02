using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class LabyrintheWallEditor : EditorWindow
{
    public GUISkin customSkin;

    private static List<Slot> editorGrid = new List<Slot>();
    private static int editorGridSizeX;
    private static int editorGridSizeY;

    private static int editorLabyrinthStartId = -1;
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
        
        if(LabyrinthManager.labyrinth != null)
        {
            editorGrid = new List<Slot>(LabyrinthManager.labyrinth.grid);
            editorGridSizeX = LabyrinthManager.labyrinth.gridSizeX;
            editorGridSizeY = LabyrinthManager.labyrinth.gridSizeY;
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

        if (GUILayout.Button("Resize") && (Xsize != editorGridSizeX || Ysize != editorGridSizeY) && EditorUtility.DisplayDialog("Warning", "This grid will be rezise to " + Xsize + " by " + Ysize + " .The old grid will be deleted.\n\nThis action can not be undo", "Yes, overwrite the data", "Cancel"))
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

        // SAVING FUNCTION
        if (GUILayout.Button("Save Labyrinthe"))
        {

            if(LabyrinthManager.labyrinth != null)
            {
                if (editorLabyrinthStartId != -1)
                    LabyrinthManager.labyrinth.idPlayerSlot = editorLabyrinthStartId;
                else
                    LabyrinthManager.labyrinth.idPlayerSlot = 0;

                LabyrinthManager.labyrinth.gridSizeX = editorGridSizeX;
                LabyrinthManager.labyrinth.gridSizeY = editorGridSizeY;
                LabyrinthManager.labyrinth.grid = new List<Slot>(editorGrid);

                LabyrinthManager.labyrinth.idStartLabyrinth = editorLabyrinthStartId;
                LabyrinthManager.labyrinth.idEndLabyrinth = editorLabyrinthEndID;
            }
            else
            {
                string path = EditorUtility.OpenFolderPanel("Saving current labyrinth", "", "");

                try
                {
                    string relativePath = path.Substring(path.IndexOf("Assets/"));

                    /* DirectoryInfo info = new DirectoryInfo(path);
                     FileInfo[] fileInfo = info.GetFiles();

                     bool exist = false;
                     foreach (FileInfo file in fileInfo)
                     {
                         if(file.Name == "labyrinth.asset")
                             ex
                     }*/

                    string assetName = "/labyrinth.asset";
                    if (!File.Exists(path + assetName) && !EditorUtility.DisplayDialog("", "A ScrLabyrinth file exist on the selected file. Do you want to overrwite the file or create a new one ?\n\nThis action can not be undo", "Yes, overrwrite the file", "No, create one"))
                    {
                        assetName = "/labyrinth" + 1 + ".asset";
                    }

                    ScrLabyrinth libAsset = ScriptableObject.CreateInstance<ScrLabyrinth>();

                    if (editorLabyrinthStartId != -1)
                        libAsset.idPlayerSlot = editorLabyrinthStartId;
                    else
                        libAsset.idPlayerSlot = 0;

                    libAsset.gridSizeX = editorGridSizeX;
                    libAsset.gridSizeY = editorGridSizeY;
                    libAsset.grid = new List<Slot>(editorGrid);

                    libAsset.idStartLabyrinth = editorLabyrinthStartId;
                    libAsset.idEndLabyrinth = editorLabyrinthEndID;

                    AssetDatabase.CreateAsset(libAsset, relativePath + assetName);
                    AssetDatabase.SaveAssets();

                    if (EditorUtility.DisplayDialog("Asset saved succesfully", "Do you want to set the manager with the data that have just been saved ?", "Yes", "Cancel"))
                        LabyrinthManager.labyrinth = libAsset;
                }
                catch (ArgumentOutOfRangeException)
                {
                    EditorUtility.DisplayDialog("Error - An error has occurred !", "The asset could not be saved at : " + path + "\n\nThe referenced path may not be complete or not in the 'Assets/' project file", "Ok");
                }
            }
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
                    if (id == editorLabyrinthStartId)
                        GUILayout.Box("S", GUILayout.Width(50), GUILayout.Height(50));
                    else if (id == editorLabyrinthEndID)
                        GUILayout.Box("E", GUILayout.Width(50), GUILayout.Height(50));
                    else if (GUILayout.Button("", "box", GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        if (isSetingStartPos)
                            editorLabyrinthStartId = id;
                        else if (isSetingEndPos)
                            editorLabyrinthEndID = id;
                    }
                }
                // SET THE WALL OF THE LABYRINTH
                else
                {

                    GUILayout.BeginVertical(GUILayout.ExpandWidth(false));

                    // IMAGE DE LA CASE
                    string indicator = "";
                    if (id == editorLabyrinthStartId)
                        indicator = "S";
                    else if (id == editorLabyrinthEndID)
                        indicator = "E";
                    GUILayout.Box(indicator, GUILayout.Width(50), GUILayout.Height(50));

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
