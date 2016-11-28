using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapGridHelper))]
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
		MapGridHelper mgh = target as MapGridHelper;
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

            if (mg.Type != EGridType.None)
            {
                if (itemId > 0)
                {
                    Handles.Label(child.position, mg.g_Id + "\n" + mg.Type + "\n" + itemId);
                }
                else
                {
                    Handles.Label(child.position, mg.g_Id + "\n" + mg.Type);
                }
            }
            else
            {
                Handles.Label(child.position, mg.g_Id.ToString());
            }
		}
	}
}
