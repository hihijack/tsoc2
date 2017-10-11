using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIDropItems : MonoBehaviour {

    public UIButton btnClose;
    public UIGrid grid;
    public GameObject gobjPrebItem;

    int count;
    int maxCount;
    public void Init(List<EquipItem> eis)
    {
        btnClose.onClick.Add(new EventDelegate(OnClose));
        for (int i = 0; i < eis.Count; i++)
        {
            EquipItem ei = eis[i];
            GameObject gobjItem = NGUITools.AddChild(grid.gameObject, gobjPrebItem);
            UIItemDrop uid = gobjItem.GetComponent<UIItemDrop>();
            uid.Init(this,ei);
        }
        grid.Reposition();
        maxCount = eis.Count;
    }


    void OnClose()
    {
        GameManager.gameView.OnCloseUIDrops(this);
    }

    internal void OnGet()
    {
        count++;
        if (count == maxCount)
        {
            //完全拾取
            OnClose();
        }
    }
}
