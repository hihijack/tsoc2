using UnityEngine;
using System.Collections;

public class ItemUesdGrid : MonoBehaviour {

    public int index;
    EquipItem ei;

    public UISprite usIcon;
    public UILabel txtCount;

    void Start() 
    {
        UIButton btn = GetComponent<UIButton>();
        btn.onClick.Add(new EventDelegate(BtnClick_UseItem));
    }

    public void SetEquipItem(EquipItem ei) 
    {
        this.ei = ei;
    }

    public EquipItem GetEquipItem() 
    {
        return ei;
    }

    public void RefershUI() 
    {
        if (ei != null)
        {
            usIcon.spriteName = ei.baseData.icon;
            txtCount.text = "x" + ei.count;
        }
        else
        {
            usIcon.spriteName = "btn_bg";
            txtCount.text = "";
        }
    }

    void BtnClick_UseItem() 
    {
        if (UIManager.Inst.HasUI())
        {
            return;
        }

        if (ei != null && ei.count > 0)
        {
            GameManager.gameView.OnStartUseItem(ei);
            // 刷新UI
            RefershUI();
        }
    }
}
