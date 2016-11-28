using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDropItems : MonoBehaviour {

    public UIButton btnClose;
    public UIGrid grid;
    public GameObject gobjPrebItem;

    public void Init(List<EquipItem> eis)
    {
        btnClose.onClick.Add(new EventDelegate(OnClose));
        for (int i = 0; i < eis.Count; i++)
        {
            EquipItem ei = eis[i];
            GameObject gobjItem = NGUITools.AddChild(grid.gameObject, gobjPrebItem);
            UIItemDrop uid = gobjItem.GetComponent<UIItemDrop>();
            uid.Init(grid,ei);
        }
        grid.Reposition();
    }


    void OnClose()
    {
        GameManager.gameView.OnCloseUIDrops(this);
    }
}
