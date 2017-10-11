using UnityEngine;
using System.Collections;
using System;

public class UISellGrid : MonoBehaviour {

    private bool showingInfo = false; // 正在显示信息

    public GameObject gobjInfo;
    public GameObject gobjTxtTip;
    public UILabel txtPrice;

    UIEventTrigger et;

    void Awake()
    {
        et = GetComponent<UIEventTrigger>();
    }

    void Start()
    {
        et.onPress.Add(new EventDelegate(OnPressEvent));
    }

    private void OnPressEvent()
    {
        UIManager.Inst.OnDropEquipItemTo(gameObject, true);
    }

    private void ShowSellInfo(EquipItem ei) 
    {
       
        gobjTxtTip.SetActive(false);
        gobjInfo.SetActive(true);
        txtPrice.text = ei.GetTradePrice().ToString();
    }

    private void HideSellInfo() 
    {
        gobjTxtTip.SetActive(true);
        gobjInfo.SetActive(false);
    }

    public void ShowInfo(bool isShow, EquipItem eiToShow) 
    {
        if (showingInfo != isShow)
        {
            showingInfo = isShow;
            if (isShow)
            {
                ShowSellInfo(eiToShow);
            }
            else
            {
                HideSellInfo();
            }
        }
    }
}
