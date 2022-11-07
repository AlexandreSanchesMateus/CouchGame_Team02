using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class LabyrintheWallEditor : EditorWindow
{
    public GUISkin customSkin;

    private static ScrLabyrinth myScrLabyrinth;

    private static List<Slot> editorGrid = new List<Slot>();
    private static int editorGridSizeX;
    private static int editorGridSizeY;

    private static int editorLabyrinthStartID = -1;
    private static int editorLabyrinthEndID = -1;

    private static int Xsize;
    private static int Ysize;

    private bool isSetingStartPos = false;
    private bool isSetingEndPos = false;

    private static string assetName = "new_labyrinth";

    public static void InitWindow(ScrLabyrinth source = null)
    {
        LabyrintheWallEditor window = GetWindow<LabyrintheWallEditor>();
        window.titleContent = new GUIContent("Labyrinthe Wall");
        window.minSize = new Vector2(800, 650);

        
        if(source != null)
        {
            myScrLabyrinth = source;
            assetName = source.name;

            editorGrid = new List<Slot>();
            foreach (Slot cell in source.grid)
                editorGrid.Add(new Slot(cell));

            editorGridSizeX = source.gridSizeX;
            editorGridSizeY = source.gridSizeY;

            editorLabyrinthStartID = source.idStartLabyrinth;
            editorLabyrinthEndID = source.idEndLabyrinth;
        }
        else
        {
            editorGrid.Clear();
            editorGridSizeX = 7;
            editorGridSizeY = 6;

            editorLabyrinthStartID = -1;
            editorLabyrinthEndID = -1;

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
        GUILayout.Label("Grid Size (X , Y)");
        GUILayout.BeginHorizontal();
        Xsize = Mathf.Clamp(EditorGUILayout.IntField(Xsize), 0, 15);
        Ysize = Mathf.Clamp(EditorGUILayout.IntField(Ysize), 0, 15);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Resize") && (Xsize != editorGridSizeX || Ysize != editorGridSizeY) && EditorUtility.DisplayDialog("Warning", "This grid will be rezise to " + Xsize + " by " + Ysize + " .The old grid will be deleted.\n\nThis action can not be undo", "Yes, overwrite the data", "Cancel"))
        {
            editorLabyrinthStartID = -1;
            editorLabyrinthEndID = -1;

            editorGridSizeX = Xsize;
            editorGridSizeY = Ysize;

            editorGrid.Clear();
            for (int i = 0; i < editorGridSizeX * editorGridSizeY; i++)
                editorGrid.Add(new Slot());
        }
        GUILayout.Space(10);

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

        // LOAD FUNCTION
        if(GUILayout.Button("Open file"))
        {
            isSetingStartPos = false;
            isSetingEndPos = false;

            string path = EditorUtility.OpenFilePanel("Open file", "", "asset");
            try
            {
                ScrLabyrinth file = (ScrLabyrinth)AssetDatabase.LoadAssetAtPath(path.Substring(path.IndexOf("Assets/")), typeof(ScrLabyrinth));

                if (file)
                    InitWindow(file);
                else
                    throw new InvalidCastException();
            }
            catch (Exception errorCode)
            {
                EditorUtility.DisplayDialog("Can not open file", "File could not be open. The file need to be located in the Assets folder and of type 'ScrLabyrinth'. " + path + "\n\nError code : " + errorCode.Message, "Ok");
            }
        }

        // SAVING FUNCTION
        if (GUILayout.Button("Save " + assetName))
        {
            isSetingStartPos = false;
            isSetingEndPos = false;

            if (myScrLabyrinth != null)
            {
                if (editorLabyrinthStartID != -1)
                    myScrLabyrinth.idPlayerSlot = editorLabyrinthStartID;

                myScrLabyrinth.gridSizeX = editorGridSizeX;
                myScrLabyrinth.gridSizeY = editorGridSizeY;
                myScrLabyrinth.grid = new List<Slot>(editorGrid);

                myScrLabyrinth.idStartLabyrinth = editorLabyrinthStartID;
                myScrLabyrinth.idEndLabyrinth = editorLabyrinthEndID;

                if (myScrLabyrinth != LabyrinthManager.labyrinth && EditorUtility.DisplayDialog("Asset saved succesfully", "Do you want to set the manager with the data that have just been saved ?", "Yes", "Cancel"))
                    LabyrinthManager.labyrinth = myScrLabyrinth;
            }
            else
            {
                string path = EditorUtility.OpenFolderPanel("Saving current labyrinth", "", "");

                try
                {
                    string relativePath = path.Substring(path.IndexOf("Assets/"));
                    ScrLabyrinth libAsset = null;

                    if (File.Exists(path + "/" + assetName + ".asset"))
                    {
                        if (EditorUtility.DisplayDialog("File existing", "A ScrLabyrinth file exist in the selected file. Do you want to overwite the file ?\n\nThis action can not be undo", "Yes, overrwrite the file", "Cancel"))
                        {
                            libAsset = (ScrLabyrinth)AssetDatabase.LoadAssetAtPath(relativePath + "/" + assetName + ".asset", typeof(ScrLabyrinth));
                            SetLabyrinthData(ref libAsset);
                            Debug.Log("Asset overwrite : " + path + "/" + assetName + ".asset");
                        }
                    }
                    else
                    {
                        Debug.Log("Asset created : " + path + "/" + assetName + ".asset");
                        libAsset = ScriptableObject.CreateInstance<ScrLabyrinth>();
                        SetLabyrinthData(ref libAsset);
                        AssetDatabase.CreateAsset(libAsset, relativePath + "/" + assetName + ".asset");
                        AssetDatabase.SaveAssets();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    EditorUtility.DisplayDialog("Error - An error has occurred !", "The asset could not be saved at : " + path + "\n\nThe referenced path may not be complete or not in the 'Assets/' project file", "Ok");
                }
            }
        }

        if (myScrLabyrinth == null)
        {
            GUILayout.Label("Asset name");
            assetName = GUILayout.TextField(assetName);
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
                    if (id == editorLabyrinthStartID)
                        GUILayout.Box("S", GUILayout.Width(50), GUILayout.Height(50));
                    else if (id == editorLabyrinthEndID)
                        GUILayout.Box("E", GUILayout.Width(50), GUILayout.Height(50));
                    else if (GUILayout.Button("", "box", GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        if (isSetingStartPos)
                            editorLabyrinthStartID = id;
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
                    if (id == editorLabyrinthStartID)
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

    private void SetLabyrinthData(ref ScrLabyrinth toApply)
    {
        if (editorLabyrinthStartID != -1)
            toApply.idPlayerSlot = editorLabyrinthStartID;

        toApply.gridSizeX = editorGridSizeX;
        toApply.gridSizeY = editorGridSizeY;

        toApply.grid = new List<Slot>();
        foreach (Slot cell in editorGrid)
            toApply.grid.Add(new Slot(cell));

        toApply.idStartLabyrinth = editorLabyrinthStartID;
        toApply.idEndLabyrinth = editorLabyrinthEndID;

        myScrLabyrinth = toApply;

        if (toApply != LabyrinthManager.labyrinth && EditorUtility.DisplayDialog("Asset saved succesfully", "Do you want to set the manager with the data that have just been saved ?", "Yes", "Cancel"))
            LabyrinthManager.labyrinth = toApply;
    }
}
