using UnityEngine;
using System.Collections;

public class UIEquipItemInfo : MonoBehaviour {

    public int borderV = 40;
    public int borderH = 40;
    public int offsetHeight = 20;
    public int offsetToScreen = 10;
    public UILabel txt;
    public UISprite bg;

    EquipItem ei;

    public UIVisibleCtl vctl;

    void Awake()
    {
        vctl = GetComponent<UIVisibleCtl>();
    }

    /// <summary>
    /// itemPos:NGUI屏幕坐标
    /// </summary>
    /// <param name="equipitem"></param>
    /// <param name="itemPos"></param>
    public void Refresh(EquipItem equipitem, Vector2 itemPos) 
    {
        this.ei = equipitem;
        txt.text = GameView.Inst.eiManager.GetEquipItemDesc(ei);

        bg.width = txt.width + borderH * 2;
        bg.height = txt.height + borderV * 2;

        int top = (int)itemPos.y + offsetHeight + bg.height;
        float right = itemPos.x + bg.width * 0.5f;
        float left = itemPos.x - bg.width * 0.5f;
        Vector2 uiSize = NGUIToolsEx.GetUISize();
        Vector3 locPos = Vector3.zero;
        if (top > uiSize.y / 2)
        {
            //溢出,显示在下方
            locPos = itemPos + new Vector2(0f, -offsetHeight - bg.height * 0.5f);
        }
        else
        {
            //显示在上方
            locPos = itemPos + new Vector2(0f, offsetHeight + bg.height * 0.5f);
        }

        float rightToScreenRight = uiSize.x * 0.5f - offsetToScreen - right;
        if (rightToScreenRight < 0)
        {
            //左移
            locPos = locPos + new Vector3(rightToScreenRight, 0f, 0f);
        }

        float leftToScreenLeft = uiSize.x * -0.5f + offsetToScreen - left;
        if (leftToScreenLeft > 0)
        {
            //右移
            locPos = locPos + new Vector3(leftToScreenLeft, 0f, 0f);
        }

        transform.localPosition = locPos;
    }
}
