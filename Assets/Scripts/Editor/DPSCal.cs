using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;

public class DPSCal : EditorWindow
{
    [MenuItem("数值工具/DPS计算器")]
    static void AddWindow()
    {
        GetWindow<DPSCal>(true, "DPS计算器");
    }
    #region 属性
    int level = 1;
    int valStr;
    int valAgi;
    int valInt;
    float wponIAS = IConst.BaseIAS;
    float ds = IConst.BaseDS;
    float dsDamage = 2f;
    //int atkPerStr = IConst.ATK_PHY_PER_STR;
    float iasPerAgi = IConst.IAS_PERCENT_PER_AGI;
    float dps;
    float ias;
    int wponDmg;
    bool isDualWield;//是否双持
    AllotType allotType;
    int arm;
    int hpMax;
    enum AllotType
    {
        Man,
        Str,
        Agi
    }
    #endregion
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("等级", GUILayout.Width(30));
        level = EditorGUILayout.IntField(level);
        EditorGUILayout.LabelField("配点方式", GUILayout.Width(30));
        allotType = (AllotType)EditorGUILayout.EnumPopup(allotType);
        // 自动配点
        if (allotType == AllotType.Str)
        {
            valStr = IConst.BASE_STR + (level - 1) * 5;
            valAgi = IConst.BASE_AGI;
        }
        else if (allotType == AllotType.Agi)
        {
            valAgi = IConst.BASE_AGI + (level - 1) * 5;
            valStr = IConst.BASE_STR;
        }
        EditorGUILayout.LabelField("力量", GUILayout.Width(30));
        valStr = EditorGUILayout.IntField(valStr);
        EditorGUILayout.LabelField("敏捷", GUILayout.Width(30));
        valAgi = EditorGUILayout.IntField(valAgi);
        EditorGUILayout.LabelField("精神力", GUILayout.Width(30));
        valInt = EditorGUILayout.IntField(valInt);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("额外攻速每敏捷", GUILayout.Width(105));
        iasPerAgi = EditorGUILayout.FloatField(iasPerAgi);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("武器伤害", GUILayout.Width(60));
        wponDmg = EditorGUILayout.IntField(wponDmg);
        EditorGUILayout.LabelField("主武器攻速", GUILayout.Width(60));
        wponIAS = EditorGUILayout.FloatField(wponIAS);
        EditorGUILayout.LabelField("致命率", GUILayout.Width(75));
        ds = EditorGUILayout.FloatField(ds);
        EditorGUILayout.LabelField("致命伤害", GUILayout.Width(60));
        dsDamage = EditorGUILayout.FloatField(dsDamage);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("双持", GUILayout.Width(60));
        isDualWield = EditorGUILayout.Toggle(isDualWield);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("护甲", GUILayout.Width(60));
        arm = EditorGUILayout.IntField(arm);
        EditorGUILayout.LabelField("生命", GUILayout.Width(60));
        hpMax = EditorGUILayout.IntField(hpMax);
        GUILayout.EndHorizontal();
        //计算
        ias = wponIAS * (1 + valAgi * iasPerAgi / 100f);
        if (isDualWield)
        {
            ias *= 1.15f;
        }
        float damage = wponDmg * (100 + valStr) / 100;
        dps = ias * damage * ds * (dsDamage + 1 / ds - 1);
        int def = 0;
        float defOff = arm * 0.03f / (arm * 0.03f + 1);
        def = Mathf.RoundToInt((hpMax / (1 - defOff)));
        //结果
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("攻速:" + ias);
        sb.AppendLine("DPS:" + dps.ToString("F1"));
        sb.AppendLine("攻击力:" + damage);
        sb.AppendLine("坚韧:" + def);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(sb.ToString(), GUILayout.Width(75), GUILayout.Height(80));
        GUILayout.EndHorizontal();
        
    }
}
