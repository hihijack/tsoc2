using UnityEngine;
using System.Collections;

public class CMDHandler : MonoBehaviour
{
    string txtGetEI = "";
    void OnGUI()
    {
        if (GUILayout.Button("添加金钱"))
        {
           GameView.Inst.GetGold(1000);
        }

        txtGetEI = GUILayout.TextField(txtGetEI);
        if (GUILayout.Button("添加装备"))
        {
            int id = 0;
            if (int.TryParse(txtGetEI, out id))
            {
                if (id != 0)
                {
                    GameView.Inst.AddAEquipItemToBag(Hero.Inst, GameView.Inst.GenerateAEquipItem(id, EEquipItemQLevel.Normal));
                }
            }
        }

        if (GUILayout.Button("设置负重"))
        {
            Hero.Inst.Prop.LoadIncrease(30);
        }
    }
}
