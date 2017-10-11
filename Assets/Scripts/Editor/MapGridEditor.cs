using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapGrid))]
public class MapGridEditor : Editor
{
	void OnSceneGUI()
	{
		MapGrid mg = target as MapGrid;
		Handles.color = Color.blue;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(mg.g_Id.ToString());

        if (mg.Type != EGridType.None)
        {
            sb.AppendLine(mg.Type.ToString());
            if (mg.Type == EGridType.ChangeMap)
            {
                sb.AppendLine(mg._ToMapId + "-" + mg._ToMapTargetGrid);
            }
        }
       
        Handles.Label(mg.transform.position, sb.ToString());
    }

	public override void OnInspectorGUI ()
	{
		MapGrid mg = target as MapGrid;
        EditorGUI.BeginChangeCheck();
        mg.g_Id = EditorGUILayout.IntField("ID", mg.g_Id);

        //Vector2 xyByPos = new Vector2(mg.transform.localPosition.x,  -1 * mg.transform.localPosition.y);
        //EditorGUILayout.Vector2Field("X/Y", xyByPos);
        //mg.x = (int)xyByPos.x;
        //mg.y = (int)xyByPos.y;

        //EditorGUILayout.LabelField("X/Y", mg.x + "," + mg.y);
		mg.g_Type = (EGridType)EditorGUILayout.EnumPopup("类型", mg.g_Type);
		if (mg.Type == EGridType.ChangeMap) 
		{
            mg.toMapId = EditorGUILayout.IntField("目标地图", mg.toMapId);
            mg.toMapTargetGrid = EditorGUILayout.IntField("目标格子", mg.toMapTargetGrid);
        }
        else if (mg.Type == EGridType.Tips)
        {
            mg.tips = EditorGUILayout.TextField("提示", mg.tips);
        }

        mg._surface = (EMGSurface)EditorGUILayout.EnumPopup("地表类型", mg._surface);

        mg.enableCreateAMon = EditorGUILayout.Toggle("允许刷怪", mg.enableCreateAMon);

        if (EditorGUI.EndChangeCheck())
		{
            if (mg.Type == EGridType.Block)
            {
                GameObject gobjBlock = new GameObject("item", typeof(SpriteRenderer));
                gobjBlock.transform.parent = mg.transform;
                gobjBlock.transform.localPosition = Vector3.zero;
                gobjBlock.tag = "MapItem";
                gobjBlock.layer = LayerMask.NameToLayer("MapGrid");
                SpriteRenderer sr = gobjBlock.GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "mapitem";
            }
            else if (mg.Type == EGridType.Tips)
            {
                if (Tools.GetGameObjectInChildByPathSimple(mg.gameObject, "item") == null)
                {
                    GameObject gobjTip = Tools.LoadResourcesGameObject(IPath.MapItems + "item_tip", mg.gameObject);
                    gobjTip.transform.localPosition = Vector3.zero;
                    gobjTip.name = "item";
                }
            }
			EditorUtility.SetDirty(target);
		}
	}
}

