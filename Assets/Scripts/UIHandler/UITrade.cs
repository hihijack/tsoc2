using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITrade : MonoBehaviour {

    public UIGrid gridItems;
    public UIButton btnClose;

    public UISellGrid sellGrid;

    ItemNPC npc;

    public void Init(ItemNPC npc) 
    {
        this.npc = npc;

        List<EquipItem> eis = npc.sells;

        for (int i = 0; i < eis.Count; i++)
		{
            EquipItem eiItem = eis[i];
            GameObject gobjItem = Tools.AddNGUIChild(gridItems.gameObject, IPath.UI + "item_trade");
            // icon
            UISprite eiIcon = Tools.GetComponentInChildByPath<UISprite>(gobjItem, "icon");
            eiIcon.spriteName = eiItem.GetIcon();
            //UIButton btnIcon = Tools.GetComponentInChildByPath<UIButton>(gobjItem, "icon");
            UIEquipItemOperControll operCtl = eiIcon.GetComponent<UIEquipItemOperControll>();
            operCtl.Init(eiItem, false);
            // name
            UILabel eiTxt = Tools.GetComponentInChildByPath<UILabel>(gobjItem, "txt_name");
            eiTxt.text = GameView.Inst.eiManager.GetEIName(eiItem);
            // pricce
            UILabel txtPrice = Tools.GetComponentInChildByPath<UILabel>(gobjItem, "btn_buy/txt_price");
            txtPrice.text = eiItem.GetTradePrice().ToString();

            ////库存
            //UILabel txtCount = Tools.GetComponentInChildByPath<UILabel>(gobjItem, "txt_count");
            //txtCount.text = "库存:" + eiItem.count;

            UIButton btnBuy = Tools.GetComponentInChildByPath<UIButton>(gobjItem, "btn_buy");
            btnBuy.onClick.Add(new EventDelegate(BtnClick_Buy));
            btnBuy.data = eiItem;
		}

        btnClose.onClick.Add(new EventDelegate(BtnClick_Close));
    }

    void BtnClick_Buy() 
    {
        EquipItem ei = UIButton.current.data as EquipItem;
        int needPrice = ei.GetTradePrice();
        if (GameView.Inst.eiManager._Gold >= needPrice)
        {
            EquipItem eiClone = ei.Clone();
            if (GameView.Inst.DoAddAEquipToBag(eiClone))
            {
                GameView.Inst.eiManager._Gold -= needPrice;
                
                //// npc移除商品
                //npc.RemoveSells(ei);
                //// 刷新UI
                //DestroyObject(UIButton.current.transform.parent.gameObject);
            }
        }
        else
        {
            // 金钱不足
            UIManager.Inst.GeneralTip("金钱不足", Color.red);
        }
    }

    void BtnClick_Close() 
    {
        UIManager.Inst.CloseUITrade();
        UIManager.Inst.ToggleUI_Bag();
    }
}
