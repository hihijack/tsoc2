using UnityEngine;
using System.Collections;
using System;

public class UIGirlTip : MonoBehaviour
{
    public UILabel txt;
    public UIButton btnNext;
    ItemGrilTip item;
    int curIndex = 0;

    internal void Init(ItemGrilTip item)
    {
        this.item = item;
        btnNext.onClick.Add(new EventDelegate(BtnShowNext));
        ShowNextNode();
    }

    private void BtnShowNext()
    {
        ShowNextNode();
    }

    private void ShowNextNode()
    {
        if (curIndex < item.tips.Length)
        {
            string tip = item.tips[curIndex];
            txt.text = txt.text + tip + "\n\n";
            curIndex++;
        }
        else
        {
            //结束
            UIManager.Inst.CloseUIGirlTip();
            GameView.Inst.ToCGMode(false);
            item.OnGirlTipEnd();
        }
    }
}
