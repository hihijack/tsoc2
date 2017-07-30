using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
public class UIEquipItemOperControll : MonoBehaviour
{
    public static GameObject mGobjDraggedItem;
    GameView gameView;
    UIButton btn;
    UISprite curSprite;

    bool inSellGird = false;

	// Use this for initialization
	void Awake () {
        gameView = GameObject.FindGameObjectWithTag("CPU").GetComponent<GameView>();
        btn = GetComponent<UIButton>();
        curSprite = GetComponent<UISprite>();
	}

    void OnPress(bool pressed)
    {
        UIManager.Inst.OnBtnPress(btn,pressed);
        if (!pressed)
        {
            OnDrop();
        }
    }

    void Update() 
    {
        if (mGobjDraggedItem != null)
        {
            GameObject gobjTouch = UICamera.hoveredObject;
            if (gobjTouch != null)
            {
                if (gobjTouch.CompareTag("SellGrid"))
                {
                    if (!inSellGird)
                    {
                        inSellGird = true;
                        OnToSellGrid();
                    }
                }
                else
                {
                    if (inSellGird)
                    {
                        inSellGird = false;
                        OnLeaveSellGrid();
                    }
                }
            }
            else
            {
                if (inSellGird)
                {
                    inSellGird = false;
                    OnLeaveSellGrid();
                }
            }
        }
        else
        {
            if (inSellGird)
            {
                inSellGird = false;
                OnLeaveSellGrid();
            }
        }
    }

    void OnToSellGrid() 
    {
        UISellGrid usg = UIManager.Inst.GetUITrade().sellGrid;
        usg.ShowInfo(true, gameView.GetEIDataInBtnGobj(mGobjDraggedItem));
    }

    void OnLeaveSellGrid() 
    {
        UISellGrid usg = UIManager.Inst.GetUITrade().sellGrid;
        usg.ShowInfo(false, null);
    }

    void OnDrop()
    {
        if (mGobjDraggedItem == null)
        {
            return;
        }
        // 取当前格子
        GameObject gobjTouch = UICamera.hoveredObject;
        if (gobjTouch != null)
        {
            if (gobjTouch.CompareTag("EIBagGrid"))
            {
                // 放置到背包空格中
                mGobjDraggedItem.transform.parent = gobjTouch.transform;
                mGobjDraggedItem.transform.localPosition = Vector3.zero;

                EquipItem ei = gameView.GetEIDataInBtnGobj(mGobjDraggedItem);
                int gridIdTo = int.Parse(gobjTouch.name);
                gameView.MoveAEquipItemToBag(GameManager.hero, ei, gridIdTo);
                GameManager.commonCPU.SaveEquipItems();
                Recover();
            }

            else if (gobjTouch.CompareTag("EIEquipGrid"))
            {
                mGobjDraggedItem.transform.parent = gobjTouch.transform;
                mGobjDraggedItem.transform.localPosition = Vector3.zero;

                EquipItem ei = gameView.GetEIDataInBtnGobj(mGobjDraggedItem);
                EEquipPart partTo = gobjTouch.GetComponent<HeroItemGrid>().part;

                gameView.MoveAEquipItemToEquip(GameManager.hero, ei, partTo);
                GameManager.commonCPU.SaveEquipItems();
                Recover();
            }
            else if (gobjTouch.CompareTag("EquipItem"))
            {
                
                if (gobjTouch != mGobjDraggedItem)
                {

                    EquipItem ei = gameView.GetEIDataInBtnGobj(mGobjDraggedItem);
                    EquipItem eiTo = gameView.GetEIDataInBtnGobj(gobjTouch);

                    bool succed = false;

                    //gameView.
                    // 装备移到背包
                    if (ei._Part != EEquipPart.None && eiTo._Part == EEquipPart.None)
                    {
                        EEquipPart partOri = ei._Part;
                        int gridIdTo = eiTo.bagGridId;
                        if (gridIdTo > 0)
                        {
                            gameView.MoveAEquipItemToBag(GameManager.hero, ei, gridIdTo);
                            gameView.MoveAEquipItemToEquip(GameManager.hero, eiTo, partOri);
                            succed = true;
                        }
                       
                    }
                    // 背包移到背包
                    else if (ei._Part == EEquipPart.None && eiTo._Part == EEquipPart.None)
                    {
                        int gridOri = ei.bagGridId;
                        int gridIdTo = eiTo.bagGridId;
                        if (gridOri > 0 && gridIdTo > 0)
                        {
                            gameView.MoveAEquipItemToBag(GameManager.hero, ei, gridIdTo);
                            gameView.MoveAEquipItemToBag(GameManager.hero, eiTo, gridOri);
                            succed = true;
                        }
                      
                    }
                    // 背包移到装备栏
                    else if(ei._Part == EEquipPart.None && eiTo._Part != EEquipPart.None)
                    {
                        int gridOri = ei.bagGridId;
                        EEquipPart partTo = eiTo._Part;
                        gameView.MoveAEquipItemToBag(GameManager.hero, eiTo, gridOri);
                        gameView.MoveAEquipItemToEquip(GameManager.hero, ei, partTo);
                        succed = true;
                    }
                   
                    if (succed)
                    {
                        // 互调位置
                        Transform tfMDragParent = mGobjDraggedItem.transform.parent;
                        mGobjDraggedItem.transform.parent = gobjTouch.transform.parent;
                        gobjTouch.transform.parent = tfMDragParent;
                        gobjTouch.transform.localPosition = Vector3.zero;
                        mGobjDraggedItem.transform.localPosition = Vector3.zero;
                        GameManager.commonCPU.SaveEquipItems();
                    }

                    Recover();
                }
                else
                {
                    Recover();
                }
            }
            else if (gobjTouch.CompareTag("SellGrid"))
            {
                // 出售
                EquipItem ei = gameView.GetEIDataInBtnGobj(mGobjDraggedItem);
                
                // UI背包移除
                UIManager.Inst.GetUIBag().RemoveAEquipItem(ei);
                // UI人物外观更新
                GameManager.gameView.UpdateOnChangeEquip(ei, false);
                // 添加金钱
                int price = ei.GetTradePrice();
                GameManager.hero._Gold += price;

                // 移除装备数据
                GameManager.gameView.RemoveEquipItem(ei);
                // 保存装备信息
                GameManager.commonCPU.SaveEquipItems();

                // 回到原位
                Recover();
            }
            else if (gobjTouch.CompareTag("DestroyGrid"))
            {
                // 摧毁
                EquipItem ei = gameView.GetEIDataInBtnGobj(mGobjDraggedItem);
                // UI背包移除
                UIManager.Inst.GetUIBag().RemoveAEquipItem(ei);
                // 移除装备数据
                GameManager.gameView.RemoveEquipItem(ei);
                // 移除配置
                UIManager.Inst.uiMain.ClearItemUsed(ei);
                // 保存装备信息
                GameManager.commonCPU.SaveEquipItems();

                // 回到原位
                Recover();
            }
            else if (gobjTouch.CompareTag("UseGrid"))
            {
                // 配置道具
                EquipItem ei = gameView.GetEIDataInBtnGobj(mGobjDraggedItem);
                if (ei.baseData.useType != EEquipItemUseType.None)
                {
                    ItemUesdGrid iug = gobjTouch.GetComponent<ItemUesdGrid>();
                    int index = iug.index;
                    GameManager.hero.SetItemUsed(index, ei.baseData.id);
                    GameManager.commonCPU.SaveItemUesd();
                    iug.SetEquipItem(ei);
                    // UI
                    iug.RefershUI();
                }

                Recover();
            }
            else
            {
                // 回到原位
                Recover();
            }
        }
        else
        {
            // 回到原位
            Recover();
        }
    }

    // 回到原位
    void Recover()
    {
        UICursor.Clear();
        curSprite.alpha = 1f;
        mGobjDraggedItem = null;

        UIManager.Inst.RecoverBagGridDisable();
    }

    void OnDrag(Vector2 delta)
    {
        UIManager.Inst.HideEquipItemInfo();
        mGobjDraggedItem = gameObject;
        curSprite.alpha = 0.5f;
        UICursor.Set(curSprite.atlas, curSprite.spriteName);
    }

    

    
}
