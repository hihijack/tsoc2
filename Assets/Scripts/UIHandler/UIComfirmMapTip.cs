using UnityEngine;
using System.Collections;
using System;

public class UIComfirmMapTip : MonoBehaviour
{
    public UIButton btnComfirm;
    public UILabel txtTip;

    string tip;

    public void Init(string tip)
    {
        this.tip = tip;

        if (btnComfirm.onClick.Count == 0)
        {
            btnComfirm.onClick.Add(new EventDelegate(OnBtn_MapTipComfirm));
        }
    }

    private void OnBtn_MapTipComfirm()
    {
        UIManager.Inst.ShowDefaultDlg(tip, null);
    }

    private void Close()
    {
        DestroyObject(gameObject);
    }
}
