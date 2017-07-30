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
            UIButton btnIcon = Tools.GetComponentInChildByPath<UIButton>(gobjItem, "icon");
            btnIcon.data = eiItem;
            // name
            UILabel eiTxt = Tools.GetComponentInChildByPath<UILabel>(gobjItem, "txt_name");
            eiTxt.text = GameManager.gameView.GetEIName(eiItem);
            // pricce
            UILabel txtPrice = Tools.GetComponentInChildByPath<UILabel>(gobjItem, "btn_buy/txt_price");
            txtPrice.text = eiItem.GetTradePrice().ToString();
            
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
        if (GameManager.hero._Gold >= needPrice)
        {
            EquipItem eiInbag = null;
            bool canPile = false;
            if (GameManager.gameView.AddAEquipItemToBag(GameManager.hero, ei, out eiInbag, out canPile))
            {
                GameManager.hero._Gold -= needPrice;
                
                // 刷新背包物品
                if (canPile)
                {
                    UIManager.Inst.GetUIBag().RefreshEquipItemCount(eiInbag);
                }
                else
                {
                    UIManager.Inst.GetUIBag().AddAEquipItemToAGrid(ei);
                }
                
                // 刷新配置
                if (eiInbag != null)
                {
                    UIManager.Inst.uiMain.RefreshItemUsed(eiInbag);
                }
                

                GameManager.commonCPU.SaveEquipItems();

                // npc移除商品
                //npc.RemoveSells(ei);

                // 刷新UI
                //DestroyObject(UIButton.current.transform.parent.gameObject);
            }
            else
            {
                UIManager.Inst.GeneralTip("背包已满", Color.red);
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
