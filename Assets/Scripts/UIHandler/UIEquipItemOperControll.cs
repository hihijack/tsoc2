using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class UIEquipItemOperControll : MonoBehaviour
{
    [Tooltip("可以被拿起")]
    public bool mCanDrop;
    public static UIEquipItemOperControll curDropItem;
    GameView gameView;
    UIButton btn;
    UISprite curSprite;

    bool inSellGird = false;

    public EquipItem mEquipItem;

    public void Init(EquipItem ei, bool canDrop)
    {
        mEquipItem = ei;
        mCanDrop = canDrop;
    }

	// Use this for initialization
	void Awake () {
        gameView = GameObject.FindGameObjectWithTag("CPU").GetComponent<GameView>();
        curSprite = GetComponent<UISprite>();
    }

    void OnPress(bool pressed)
    {
        if (UICamera.currentKey == KeyCode.Mouse0)
        {
            if (pressed)
            {
                if (curDropItem == null)
                {
                    //拿起
                    if (mCanDrop)
                    {
                        UIManager.Inst.HideEquipItemInfo();
                        curDropItem = this;
                        curSprite.alpha = 0.5f;
                        UICursor.Set(curSprite.atlas, curSprite.spriteName);
                        //背包中，显示可以装备格子
                        UIHeroBag uiBag = UIManager.Inst.GetUIBag();
                        if (uiBag != null)
                        {
                            uiBag.DisableGrid(mEquipItem);
                        }
                    }
                }
                else
                {
                    UIManager.Inst.OnDropEquipItemTo(gameObject, true);
                }
            }
        }
        else if (UICamera.currentKey == KeyCode.Mouse1)
        {
            if (pressed)
            {
                if (mEquipItem.IsInBag())
                {
                    //右键一个装备。使用道具
                    GameView.Inst.OnStartUseItem(mEquipItem);
                }
            }
        }

    }

    /// <summary>
    /// btn接口
    /// </summary>
    /// <param name="isOver"></param>
    void OnHover(bool isOver)
    {
        if (mEquipItem == null)
        {
            return;
        }

        if (isOver)
        {
            UIManager.Inst.ShowEquipItemInfo(mEquipItem, NGUIToolsEx.GetUIPos(GameView.Inst.cameraUI.transform, transform));
        }
        else
        {
            UIManager.Inst.HideEquipItemInfo();
        }
    }

    //void Update() 
    //{
        //if (curDropItem != null)
        //{
        //    GameObject gobjTouch = UICamera.hoveredObject;
        //    if (gobjTouch != null)
        //    {
        //        if (gobjTouch.CompareTag("SellGrid"))
        //        {
        //            if (!inSellGird)
        //            {
        //                inSellGird = true;
        //                OnToSellGrid();
        //            }
        //        }
        //        else
        //        {
        //            if (inSellGird)
        //            {
        //                inSellGird = false;
        //                OnLeaveSellGrid();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (inSellGird)
        //        {
        //            inSellGird = false;
        //            OnLeaveSellGrid();
        //        }
        //    }
        //}
        //else
        //{
        //    if (inSellGird)
        //    {
        //        inSellGird = false;
        //        OnLeaveSellGrid();
        //    }
        //}
    //}

    void OnToSellGrid() 
    {
        //UISellGrid usg = UIManager.Inst.GetUITrade().sellGrid;
        //usg.ShowInfo(true, gameView.GetEIDataInBtnGobj(mGobjDraggedItem));
    }

    void OnLeaveSellGrid() 
    {
        UISellGrid usg = UIManager.Inst.GetUITrade().sellGrid;
        usg.ShowInfo(false, null);
    }

    // 复原
    public void Recover()
    {
        UICursor.Clear();
        curSprite.alpha = 1f;
        curDropItem = null;

        UIManager.Inst.RecoverBagGridDisable();
    }
}
