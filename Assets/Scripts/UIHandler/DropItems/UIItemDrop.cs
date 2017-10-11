using UnityEngine;
using System.Collections;

public class UIItemDrop : MonoBehaviour {

    public UISprite icon;
    public UILabel txtName;

    EquipItem ei;

    UIDropItems uiDropItems;

    public void Init(UIDropItems ui, EquipItem ei)
    {
        this.ei = ei;
        this.uiDropItems = ui;
        // icon
        string iconname = "";
        if (ei.qLevel == EEquipItemQLevel.Legend)
        {
            iconname = ei.legendBaseData.icon;
        }
        else
        {
            iconname = ei.baseData.icon;
        }
        icon.spriteName = iconname;
        
        // name
        string strName = "";
        strName = GameView.Inst.eiManager.GetEIName(ei);
        if (ei.count > 1)
        {
            strName = strName + "x" + ei.count;
        }
        txtName.text = strName;

        UIButton btn = GetComponent<UIButton>();
        btn.onClick.Add(new EventDelegate(OnGet));
    }

    //点击拾取
    void OnGet()
    {
        if (ei.baseData.id == 11) //金币
        {
            GameView.Inst.eiManager.GetGold(ei.count);
            gameObject.SetActive(false);
            // 拾取成功
            uiDropItems.grid.Reposition();
            uiDropItems.OnGet();
        }
        else
        {
            if (GameView.Inst.DoAddAEquipToBag(ei))
            {
                gameObject.SetActive(false);
                // 拾取成功
                uiDropItems.grid.Reposition();
                uiDropItems.OnGet();
                //UIManager.Inst.uiMain.RefreshItemUsed(ei);
            }
            else
            {
                UIManager.Inst.GeneralTip("背包已满", Color.red);
            }
        }
       
    }
}
