using UnityEngine;
using System.Collections;
using System;

public class UITransfer : MonoBehaviour
{
    public GameObject gobjItem;
    public UIGrid grid;
    public UIButton btnClose;

    public void Init()
    {
        string strMapHasActive = GameView.Inst.GetTransferActivedSaved();
        if (!string.IsNullOrEmpty(strMapHasActive))
        {
            string[] strMapIds = strMapHasActive.Split('&');
            for (int i = 0; i < strMapIds.Length; i++)
            {
                if (string.IsNullOrEmpty(strMapIds[i]))
                {
                    continue;
                }

                int mapId = int.Parse(strMapIds[i]);
                GameObject gobjItemTransfer = NGUITools.AddChild(grid.gameObject, gobjItem);
                GameMapBaseData map = GameDatas.GetGameMapBD(mapId);
                ItemUITransfer itemUI = gobjItemTransfer.GetComponent<ItemUITransfer>();
                itemUI.Init(map);
            }
            grid.Reposition();
        }
   
        btnClose.onClick.Add(new EventDelegate(OnClose));
        gobjItem.SetActive(false);
    }

    private void OnClose()
    {
        UIManager.Inst.CloseUITransfer();
    }
}
