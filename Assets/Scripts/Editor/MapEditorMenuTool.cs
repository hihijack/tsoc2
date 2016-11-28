using UnityEditor;
using UnityEngine;

public class MapEditorMenuTool 
{
	[MenuItem ("DB/ConnectToDB")]
	static void InitData()
	{
        GameManager.ConTODB();
	}

    [MenuItem("DB/CloseConToDB")]
    static void CloseConToDB()
    {
        GameManager.CloseConToDB();
    }

    [MenuItem("GameObject/SetGrid/AddBlock")]
    static void AddBlock()
    {
        GameObject[] gobjs = Selection.gameObjects;
        for (int i = 0; i < gobjs.Length; i++)
        {
            GameObject gobj = gobjs[i];
            MapGrid mg = gobj.GetComponent<MapGrid>();
            if (mg != null)
            {
                mg.Type = EGridType.Block;
                GameObject gobjBlock = new GameObject("item", typeof(SpriteRenderer));
                gobjBlock.transform.parent = mg.transform;
                gobjBlock.transform.localPosition = Vector3.zero;
                gobjBlock.tag = "MapItem";
                gobjBlock.layer = LayerMask.NameToLayer("MapGrid");
                SpriteRenderer sr = gobjBlock.GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "mapitem";
            }
        }
    }
}

