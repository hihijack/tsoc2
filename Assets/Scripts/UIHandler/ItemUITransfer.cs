using UnityEngine;
using System.Collections;
using System;

public class ItemUITransfer : MonoBehaviour
{
    public GameMapBaseData map;
    public UILabel txtName;
    public UIButton btn;
    public void Init(GameMapBaseData map)
    {
        this.map = map;
        txtName.text = map.name;
        btn.onClick.Add(new EventDelegate(OnBtnClick));
    }

    private void OnBtnClick()
    {
        GameView.Inst.OnTransferToMap(map);
        UIManager.Inst.CloseUITransfer();
    }
}
