using UnityEngine;
using UnityEditor;
using System;

public class MapGridEditorWin : EditorWindow
{
    [MenuItem("MapEditor/格子编辑器")]
    static void AddWindow()
    {
        GetWindow<MapGridEditorWin>(true, "地图格子编辑器");
    }

    Sprite txuBlock;
    string enermyName;
    void OnGUI()
    {
        GameObject[] gobjSelects = Selection.gameObjects;
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("已选择:" + gobjSelects.Length, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        txuBlock = EditorGUILayout.ObjectField(txuBlock, typeof(Sprite), true) as Sprite;
        if (GUILayout.Button("障碍格子"))
        {
            SetMapBlock();
        }
        if (GUILayout.Button("空格子"))
        {
            SetGridNone();
        }

        if (GUILayout.Button("不可用格子"))
        {
            SetGridNoUse();
        }

        enermyName = EditorGUILayout.TextField("怪物名字:", enermyName);
        if (GUILayout.Button("添加敌人"))
        {
            AddAEnermy(enermyName);
        }

        if (GUILayout.Button("添加宝箱"))
        {
            AddATC();
        }

        if (GUILayout.Button("门"))
        {
            AddDoor();
        }
    }

    private void SetGridNoUse()
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
                    gobjGrid.SetActive(false);
                }
                SpriteRenderer sr = mg.GetComponent<SpriteRenderer>();
                sr.sprite = null;
            }
        }
    }

    private void AddDoor()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            GameObject gobjItem = Resources.Load<GameObject>(IPath.MapItems + "door");
            if (gobjItem != null)
            {
                gobjItem = Instantiate(gobjItem);
                gobjItem.name = "item";
                gobjItem.transform.parent = item.transform;
                gobjItem.transform.localPosition = Vector3.zero;
                ItemDoor door = gobjItem.GetComponent<ItemDoor>();
                door.GenGUID();
            }
        }
    }

    private void AddATC()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            GameObject gobjItem = Resources.Load<GameObject>(IPath.MapItems + "treasurechest");
            if (gobjItem != null)
            {
                gobjItem = Instantiate(gobjItem);
                gobjItem.name = "item";
                gobjItem.transform.parent = item.transform;
                gobjItem.transform.localPosition = Vector3.zero;
                ItemTreasureChest itemTC = gobjItem.GetComponent<ItemTreasureChest>();
                itemTC.GenGUID();
            }
        }
    }

    private void AddAEnermy(string enermyName)
    {
        if (string.IsNullOrEmpty(enermyName))
        {
            return;
        }
        foreach (GameObject item in Selection.gameObjects)
        {
            GameObject gobjItem = Resources.Load<GameObject>(IPath.Items + enermyName);
            if (gobjItem != null)
            {
                gobjItem = Instantiate(gobjItem);
                gobjItem.name = enermyName;
                gobjItem.transform.parent = item.transform;
                gobjItem.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void SetGridNone()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            MapGrid grid = item.GetComponent<MapGrid>();
            GameObject gobjItem = grid.GetItemGobj();
            if (grid != null && gobjItem != null)
            {
                grid.Type = EGridType.None;
                DestroyImmediate(gobjItem);
            }
        }
    }

    void SetMapBlock()
    {
        foreach (GameObject item in Selection.gameObjects)
        {
            MapGrid grid = item.GetComponent<MapGrid>();
            GameObject gobjBlock = Tools.GetGameObjectInChildByPathSimple(grid.gameObject, "item");
            if (grid != null)
            {
                grid.Type = EGridType.Block;
                if (gobjBlock == null)
                {
                    gobjBlock = new GameObject("item", typeof(SpriteRenderer));
                    gobjBlock.transform.parent = grid.transform;
                    gobjBlock.transform.localPosition = Vector3.zero;
                    gobjBlock.tag = "MapItem";
                    gobjBlock.layer = LayerMask.NameToLayer("MapGrid");
                }
                SpriteRenderer sr = gobjBlock.GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "mapitem";
                sr.sprite = txuBlock;
            }
           
        }
    }
}