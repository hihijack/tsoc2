using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapEditorMenuTool 
{
	[MenuItem ("Tools/DB/ConnectToDB")]
	static void InitData()
	{
        GameManager.ConTODB();
	}

    [MenuItem("Tools/DB/CloseConToDB")]
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

    [MenuItem("Tools/删除存档")]
    static void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("删除存档");
    }

    [MenuItem("Tools/删除装备存档")]
    static void DeleteEquip()
    {
        string fileName = Application.persistentDataPath + "/eiinbag.dat";
        string fileName2 = Application.persistentDataPath + "/eihasequip.dat";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        if (File.Exists(fileName2))
        {
            File.Delete(fileName2);
        }
    }

    [MenuItem("Tools/场景/主场景 #1")]
    static void OpenSceneMain()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene("Assets/Scenes/main.unity");
    }

    [MenuItem("Tools/场景/UI #2")]
    static void OpenSceneUI()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene("Assets/Scenes/UI.unity");
    }

    [MenuItem("Tools/场景/地图 #3")]
    static void OpenSceneMap()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene("Assets/Scenes/maps.unity");
    }

    [MenuItem("GameObject/Map/设为Block", false, 1)]
    static void SetMapBlock()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            MapGrid grid = item.GetComponent<MapGrid>();
            Transform tfItem = grid.transform.FindChild("item");
            if (grid != null && tfItem == null)
            {
                grid.Type = EGridType.Block;
                GameObject gobjBlock = new GameObject("item", typeof(SpriteRenderer));
                gobjBlock.transform.parent = grid.transform;
                gobjBlock.transform.localPosition = Vector3.zero;
                gobjBlock.tag = "MapItem";
                gobjBlock.layer = LayerMask.NameToLayer("MapGrid");
                SpriteRenderer sr = gobjBlock.GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "mapitem";
            }
        }
       
    }

    [MenuItem("GameObject/Map/设为NoneBlock", false, 2)]
    static void SetMapNoneBlock()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            MapGrid mg = item.GetComponent<MapGrid>();
            if (mg != null)
            {
                mg.Type = EGridType.Block;
                GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(item, "grid");
                if (gobjGrid != null)
                {
                    Object.DestroyImmediate(gobjGrid);
                }
            }
            EditorUtility.SetDirty(item);
        }
    }

    [MenuItem("GameObject/Map/查找相同贴图", false, 3)]
    static void FindSameAltas()
    {
        if (Selection.activeGameObject != null)
        {
            SpriteRenderer sr = Selection.activeGameObject.GetComponent<SpriteRenderer>();
            Sprite txu = sr.sprite;
            List<Object> selects = new List<Object>();
            
            foreach (Transform child in Selection.activeTransform.parent)
            {
                SpriteRenderer srChild = child.GetComponent<SpriteRenderer>();
                if (srChild != null && srChild.sprite == txu)
                {
                    selects.Add(child.gameObject);
                }
            }
            Selection.objects = selects.ToArray();
        }
    }

    [MenuItem("Assets/添加到格子")]
    static void AddToGrid()
    {
        //GameObject prefSelect = GameObject.Instantiate(Selection.activeObject) as GameObject;
        //prefSelect.transform.parent 
    }
}

