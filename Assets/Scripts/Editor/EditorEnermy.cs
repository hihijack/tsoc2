using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enermy))]
public class EditorEnermy : Editor
{
    public override void OnInspectorGUI()
    {
        Enermy enermy = target as Enermy;
        if (string.IsNullOrEmpty(enermy.guid))
        {
            EditorGUILayout.LabelField("无GUID");
        }
        else
        {
            EditorGUILayout.LabelField("GUID:" + enermy.guid);
        }
        enermy.monsterId = EditorGUILayout.IntField("怪物ID:", enermy.monsterId);
        enermy.refresh = EditorGUILayout.Toggle("是否刷新:", enermy.refresh);
        enermy.needInitInStart = EditorGUILayout.Toggle("自动初始化:", enermy.needInitInStart);
        EditorGUILayout.LabelField("掉落");
        enermy.drops = EditorGUILayout.TextArea(enermy.drops);
    }
}