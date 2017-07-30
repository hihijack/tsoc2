using UnityEngine;
using System.Collections;

public class UIItemDrop : MonoBehaviour {

    public UISprite icon;
    public UILabel txtName;

    EquipItem ei;
    UIGrid grid;

    public void Init(UIGrid grid, EquipItem ei)
    {
        this.ei = ei;
        this.grid = grid;
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
        strName = GameManager.gameView.GetEIName(ei);
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
            GameManager.gameView.GetGold(ei.count);
            gameObject.SetActive(false);
            // 拾取成功
            grid.Reposition();
        }
        else
        {
            if (GameManager.gameView.AddAEquipItemToBag(GameManager.gameView._MHero, ei))
            {
                gameObject.SetActive(false);
                // 拾取成功
                grid.Reposition();

                UIManager.Inst.uiMain.RefreshItemUsed(ei);

                GameManager.commonCPU.SaveEquipItems();
            }
            else
            {
                UIManager.Inst.GeneralTip("背包已满", Color.red);
            }
        }
        
    }


}
