﻿using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameMap))]
public class MapGridHelperEditor : Editor {

	Transform thisTf;
	int mapWidth;
	int mapHeight;

	void OnSceneGUI()
	{
		ShowInfoInScene();
	}

	public override void OnInspectorGUI ()
	{
  //      mapWidth = EditorGUILayout.IntField("宽", mapWidth);
  //      mapHeight = EditorGUILayout.IntField("高", mapHeight);

  //      if(GUILayout.Button("生成地表"))
		//{
		//	MapGridHelper mgh = target as MapGridHelper;
		//	thisTf = mgh.transform;
		//	int index = 0;
		//	for (int y = 0; y < mapHeight; y++) {
		//		for (int x = 0; x < mapWidth; x++) {
		//			GameObject gobjGrid = Tools.LoadResourcesGameObject("Prefabs/grid");
		//			gobjGrid.transform.parent = thisTf;
  //                  gobjGrid.transform.localPosition = new Vector3(x, -1 * y, 0f);
		//			gobjGrid.name = index.ToString();
		//			MapGrid mg = gobjGrid.GetComponent<MapGrid>();
		//			mg.g_Id = index;
		//			index ++;
		//		}
		//	}
		//}
	}
	void ShowInfoInScene()
	{
		GameMap mgh = target as GameMap;
		thisTf = mgh.transform;
		Handles.color = Color.blue;
		foreach (Transform child in thisTf) 
		{
			MapGrid mg = child.GetComponent<MapGrid>();
            if (mg == null)
            {
                continue;
            }
            int itemId = 0;
            if (mg.Type == EGridType.ChangeMap)
            {
                itemId = mg._ToMapId;
            }

            string desc = "";
            if (mg.Type != EGridType.None)
            {
                if (itemId > 0)
                {
                    desc = mg.g_Id + "\n" + mg.Type + "\n" + itemId;
                }
                else
                {
                    desc = mg.g_Id + "\n" + mg.Type;
                }
            }
            else
            {
                desc = mg.g_Id.ToString();
            }
            if (mg.Surface != EMGSurface.Normal)
            {
                desc = desc + "\n" + mg.Surface;
            }
            Handles.Label(child.position, desc);
        }
	}
}
