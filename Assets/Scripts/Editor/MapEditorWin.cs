using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapEditorWin : EditorWindow {

    [MenuItem("MapEditor/地图编辑器")]
	static void AddWindow()
    {
        GetWindow<MapEditorWin>(true, "地图编辑器");
    }

    int width;
    int height;
    string mapName;
    Sprite baseTxu;
    
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("名字", GUILayout.Width(30));
        mapName = EditorGUILayout.TextField(mapName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("宽", GUILayout.Width(30));
        width = EditorGUILayout.IntField(width);
        EditorGUILayout.LabelField("高", GUILayout.Width(30));
        height = EditorGUILayout.IntField(height);
        GUILayout.EndHorizontal();
        baseTxu = EditorGUILayout.ObjectField(baseTxu, typeof(Sprite), true) as Sprite;
        if (GUILayout.Button("生成"))
        {
            GameObject gobjMap = new GameObject();
            gobjMap.name = mapName;
            gobjMap.AddComponent<MapGridHelper>();
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject gobjGrid = Tools.LoadResourcesGameObject("Prefabs/grid");
                    gobjGrid.transform.parent = gobjMap.transform;
                    gobjGrid.transform.localPosition = new Vector3(x, -1 * y, 0f);
                    gobjGrid.name = index.ToString();
                    MapGrid mg = gobjGrid.GetComponent<MapGrid>();
                    mg.g_Id = index;
                    if (baseTxu != null)
                    {
                        mg.SetTxu(baseTxu);
                    }
                    index++;
                }
            }
        }
    }
}
