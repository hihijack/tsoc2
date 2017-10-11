using UnityEngine;
using System.Collections;
using System;

public class UIDefaultDlg : MonoBehaviour
{
    public UILabel txtTip;
    public UIButton btnComfirm;

    public UISprite txuBG;

    public int border;

    Action onComfirm;

    public void Init(string tip, Action onComfirm)
    {
        txtTip.text = tip;

        txuBG.width = txtTip.width + border;
        txuBG.height = txtTip.height + border;

        btnComfirm.transform.localPosition = new Vector3(0f, -txuBG.height * 0.5f - 50, 0f);

        this.onComfirm = onComfirm;
        if (btnComfirm.onClick.Count == 0)
        {
            btnComfirm.onClick.Add(new EventDelegate(BtnClick_Comfirm));
        }
    }

    private void BtnClick_Comfirm()
    {
        if (onComfirm != null)
        {
            onComfirm();
        }
        DestroyObject(gameObject);
    }
}
