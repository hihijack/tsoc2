using UnityEngine;
using System.Collections;

public class CMDHandler : MonoBehaviour
{
    string txtGetEI = "";
    string txtToMap = "";
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
        txtToMap = GUILayout.TextField(txtToMap);
        if (GUILayout.Button("前往地图"))
        {
            if (!string.IsNullOrEmpty(txtToMap))
            {
                string[] sT = txtToMap.Split('_');
                int mapId = int.Parse(sT[0]);
                int gridId = int.Parse(sT[1]);
                GameMapBaseData mapTarget = GameDatas.GetGameMapBD(mapId);
                GameView.Inst.PlayerToMap(mapTarget, gridId);
            }
        }

        if (GUILayout.Button("test"))
        {
            Debug.LogError(NGUIToolsEx.GetUISize());
        }
    }
}
