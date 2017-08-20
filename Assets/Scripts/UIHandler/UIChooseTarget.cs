using UnityEngine;
using System.Collections;
using System;

public class UIChooseTarget : MonoBehaviour
{
    public UIButton btnCancel;
    public UIButton btnComfirm;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Init()
    {
        btnCancel.onClick.Add(new EventDelegate(BtnCancel));
        btnComfirm.onClick.Add(new EventDelegate(BtnComfirm));
    }

    private void BtnComfirm()
    {
        GameView.Inst.OnComfirmUseItemToTarget();
        Close();
    }

    private void BtnCancel()
    {
        GameView.Inst.OnCancelUseItem();
        Close();
    }

    internal void Close()
    {
        SetVisible(false);
    }

    internal void SetVisible(bool v)
    {
        gameObject.SetActive(v);
    }
}
