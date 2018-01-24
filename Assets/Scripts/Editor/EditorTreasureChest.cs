using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemTreasureChest))]
public class EditorTreasureChest : Editor
{
    public override void OnInspectorGUI()
    {
        ItemTreasureChest item = target as ItemTreasureChest;
        EditorGUI.BeginChangeCheck();

        if (string.IsNullOrEmpty(item.guid))
        {
            EditorGUILayout.LabelField("无GUID");
        }
        else
        {
            EditorGUILayout.LabelField("GUID:" + item.guid);
        }
        EditorGUILayout.LabelField("掉落");

        item.drop = EditorGUILayout.TextArea(item.drop);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}