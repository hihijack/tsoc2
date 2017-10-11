using UnityEngine;
using System.Collections;

public class CMDHandler : MonoBehaviour
{
    string txtGetEI = "";
    void OnGUI()
    {
        if (!Application.isEditor)
        {
            return;
        }
        if (GUILayout.Button("添加金钱"))
        {
           GameView.Inst.eiManager.GetGold(1000);
        }

        txtGetEI = GUILayout.TextField(txtGetEI);
        if (GUILayout.Button("添加装备"))
        {
            int id = 0;
            if (int.TryParse(txtGetEI, out id))
            {
                if (id != 0)
                {
                    GameView.Inst.DoAddAEquipToBag(GameView.Inst.eiManager.GenerateAEquipItem(id, EEquipItemQLevel.Normal));
                }
            }
        }

        if (GUILayout.Button("设置负重"))
        {
            Hero.Inst.Prop.LoadIncrease(30);
        }

        if (GUILayout.Button("test"))
        {
            Debug.LogError(NGUIToolsEx.GetUISize());
        }
    }
}
