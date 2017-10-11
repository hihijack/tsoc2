using UnityEngine;
using System.Collections;
using System;

public class UIHeroBag : MonoBehaviour {

    public GameObject gobjPreBagGridItem;
    public GameObject gobjGridBag;

    public GameObject gobjPreEquipItem;

    public GameObject gobjPartHelm;
    public GameObject gobjPartBreastplate;
    public GameObject gobjPartCuff;
    public GameObject gobjPartGlove;
    public GameObject gobjPartHand1;
    public GameObject gobjPartHand2;
    public GameObject gobjPartNeckLace;
    public GameObject gobjPartPants;
    public GameObject gobjPartShoe;
    public GameObject gobjPartShoulder;

    public GameObject gobjHeroItemsRoot;

    protected GameView gameView;

    public UILabel txtGold;

    public float xWinItemUesd = -150f;
    public UIButton btnItemUsed;
    public GameObject gobjWin;
    bool isSettingItemUsed = false;

    [NonSerialized]
    public UIVisibleCtl vctl;

    public void Init(GameView gv) 
    {
        this.gameView = gv;

        vctl = GetComponent<UIVisibleCtl>();

        CreateBagGrid();

        // 英雄身上的装备
        for (int i = 0; i < GameView.Inst.eiManager.itemsHasEquip.Count; i++)
        {
            EquipItem ei = GameView.Inst.eiManager.itemsHasEquip[i];
            GameObject gobjPartToEquip = GetEquipItemPartGobj(ei._Part);
            GameObject gobjItem = NGUITools.AddChild(gobjPartToEquip, gobjPreEquipItem);
            // 图标
            UISprite icon = gobjItem.GetComponent<UISprite>();
            if (ei.qLevel == EEquipItemQLevel.Legend)
            {
                icon.spriteName = ei.legendBaseData.icon;
            }
            else
            {
                icon.spriteName = ei.baseData.icon;
            }
            // 品质颜色
            UISprite usQLevel = Tools.GetComponentInChildByPath<UISprite>(gobjItem, "qlevel");
            usQLevel.color = ei.GetQLevelColor();

            UIButton btn = gobjItem.GetComponent<UIButton>();
            btn.data = ei;
            UIEquipItemOperControll opCtl = gobjItem.GetComponent<UIEquipItemOperControll>();
            opCtl.Init(ei, true);
        }
        // 背包里的物品
        for (int i = 0; i < GameView.Inst.eiManager.itemsInBag.Count; i++)
        {
            EquipItem eiInBag = GameView.Inst.eiManager.itemsInBag[i];
            AddAEquipItemToAGrid(eiInBag);
        }

        RefreshGold();

        //按键监听
        EventDelegate onPress = new EventDelegate(OnPressItem);
        foreach (Transform item in gobjHeroItemsRoot.transform)
        {
            AddEventTrigger(item.gameObject, onPress);
        }
    }

    internal void RefreshAddAItem(EquipItem eiInBag)
    {
        if (eiInBag == null)
        {
            return;
        }

        if (!RefreshEquipItemCount(eiInBag))
        {
            //创建新物品
            AddAEquipItemToAGrid(eiInBag);
        }
    }

    private void AddEventTrigger(GameObject target, EventDelegate onPress)
    {
        UIEventTrigger et = target.GetComponent<UIEventTrigger>();
        et.onPress.Add(onPress);
    }

    void BtnClick_ItemUsed() 
    {
        if (isSettingItemUsed)
        {
            gobjWin.transform.localPosition = new Vector3(0f, 0f, 0f);
            isSettingItemUsed = false;
        }
        else
        {
            gobjWin.transform.localPosition = new Vector3(xWinItemUesd, 0f, 0f);
            isSettingItemUsed = true;
            // 隐藏角色UI
            UIManager.Inst.CloseHeroInfo();
        }
    }

    public void ToItemUsedSetting(bool toSetting) 
    {
        if (toSetting)
        {
            if (!isSettingItemUsed)
            {
                isSettingItemUsed = true;
                gobjWin.transform.localPosition = new Vector3(xWinItemUesd, 0f, 0f);
                isSettingItemUsed = true;
            }
        }
        else
        {
            if (isSettingItemUsed)
            {
                gobjWin.transform.localPosition = new Vector3(0f, 0f, 0f);
                isSettingItemUsed = false;
            }
        }
        
    }

    public GameObject GetEquipItemPartGobj(EEquipPart part) 
    {
        GameObject gobjPartToEquip = null;
        switch (part)
        {
            case EEquipPart.Helm:
                gobjPartToEquip = gobjPartHelm;
                break;
            case EEquipPart.Necklace:
                gobjPartToEquip = gobjPartNeckLace;
                break;
            case EEquipPart.Breastplate:
                gobjPartToEquip = gobjPartBreastplate;
                break;
            case EEquipPart.Glove:
                gobjPartToEquip = gobjPartGlove;
                break;
            case EEquipPart.Pants:
                gobjPartToEquip = gobjPartPants;
                break;
            case EEquipPart.Shoe:
                gobjPartToEquip = gobjPartShoe;
                break;
            case EEquipPart.Hand1:
                gobjPartToEquip = gobjPartHand1;
                break;
            case EEquipPart.Hand2:
                gobjPartToEquip = gobjPartHand2;
                break;
            default:
                break;
        }
        return gobjPartToEquip;
    }

    public void AddAEquipItemToAGrid(EquipItem ei)
    {
        int gridid = ei.BagGridId;
        GameObject gobjGridToPut = Tools.GetGameObjectInChildByPathSimple(gobjGridBag, gridid.ToString());
        if (gobjGridToPut != null)
        {
            GameObject gobjEI = NGUITools.AddChild(gobjGridToPut, gobjPreEquipItem);
            // 图标
            UISprite icon = gobjEI.GetComponent<UISprite>();
            icon.spriteName = ei.GetIcon();
            // 品质颜色
            UISprite usQLevel = Tools.GetComponentInChildByPath<UISprite>(gobjEI, "qlevel");
            usQLevel.color = ei.GetQLevelColor();

            // 堆叠个数
            UILabel txtCount = Tools.GetComponentInChildByPath<UILabel>(gobjEI, "txt_count");
            if (ei.count > 1)
            {
                txtCount.gameObject.SetActive(true);
                txtCount.text = "x" + ei.count.ToString();
            }
            else
            {
                txtCount.gameObject.SetActive(false);
            }

            UIButton btn = gobjEI.GetComponent<UIButton>();
            btn.data = ei;
            UIEquipItemOperControll operCtl = gobjEI.GetComponent<UIEquipItemOperControll>();
            operCtl.Init(ei, true);
        }
    }

    /// <summary>
    /// 刷新堆叠个数
    /// </summary>
    /// <param name="ei"></param>
    public bool RefreshEquipItemCount(EquipItem ei) 
    {
        bool refrsh = false;
        GameObject gobjEI = GetGobjOfAEquipItem(ei);
        if (gobjEI != null)
        {
            refrsh = true;
            UILabel txtCount = Tools.GetComponentInChildByPath<UILabel>(gobjEI, "txt_count");
            if (ei.count > 1)
            {
                txtCount.gameObject.SetActive(true);
                txtCount.text = "x" + ei.count.ToString();
            }
            else
            {
                txtCount.gameObject.SetActive(false);
            }
        }
        return refrsh;
    }

    /// <summary>
    /// 移除身上或背包里一件物品
    /// </summary>
    /// <param name="ei"></param>
    public void RemoveAEquipItem(EquipItem ei) 
    {
        GameObject gobjEI = GetGobjOfAEquipItem(ei);
        if (gobjEI != null)
        {
            DestroyObject(gobjEI);
        }
    }

    public GameObject GetGobjOfAEquipItem(EquipItem ei) 
    {
        GameObject gobjEI = null;
        if (ei.BagGridId > 0)
        {
            // 在背包里
            GameObject gobjGridToPut = Tools.GetGameObjectInChildByPathSimple(gobjGridBag, ei.BagGridId.ToString());
            if (gobjGridToPut != null && gobjGridToPut.transform.childCount > 0)
            {
                gobjEI = gobjGridToPut.transform.GetChild(0).gameObject;
            }
        }
        else
        {
            // 在身上
            gobjEI = GetEquipItemPartGobj(ei._Part).transform.GetChild(0).gameObject;
        }
        return gobjEI;
    }

    public void RefreshGold()
    {
        txtGold.text = GameView.Inst.eiManager._Gold.ToString();
    }

    protected void CreateBagGrid()
    {
        for (int i = 1; i <= 28; i++)
        {
            GameObject gobjGridItem = NGUITools.AddChild(gobjGridBag, gobjPreBagGridItem);
            gobjGridItem.name = i.ToString();
            UIEventTrigger eT = gobjGridItem.GetComponent<UIEventTrigger>();
            eT.onPress.Add(new EventDelegate(OnPressItem));
        }
        UIGrid ug = gobjGridBag.GetComponent<UIGrid>();
        ug.Reposition();
    }

    private void OnPressItem()
    {
        UIManager.Inst.OnDropEquipItemTo(UIEventTrigger.current.gameObject, true);
    }

    /// <summary>
    /// 根据当前选择装备禁用装备栏与背包格子
    /// </summary>
    /// <param name="eiSelect"></param>
    public void DisableGrid(EquipItem eiSelect)
    {
        // 不可装备部位禁用.
        // 全部禁用
        SetAGridEnable(gobjPartHelm, false);
        SetAGridEnable(gobjPartBreastplate, false);
        SetAGridEnable(gobjPartCuff, false);
        SetAGridEnable(gobjPartGlove, false);
        SetAGridEnable(gobjPartHand1, false);
        SetAGridEnable(gobjPartHand2, false);
        SetAGridEnable(gobjPartNeckLace, false);
        SetAGridEnable(gobjPartPants, false);
        SetAGridEnable(gobjPartShoe, false);
        SetAGridEnable(gobjPartShoulder, false);

        switch (eiSelect.baseData.type)
        {
            case EEquipItemType.Helm:
                SetAGridEnable(gobjPartHelm, true);
                break;
            case EEquipItemType.Necklace:
                SetAGridEnable(gobjPartNeckLace, true);
                break;
            //case EEquipItemType.Shoulder:
            //    SetAGridEnable(gobjPartShoulder, true);
            //    break;
            case EEquipItemType.Breastplate:
                SetAGridEnable(gobjPartBreastplate, true);
                break;
            //case EEquipItemType.Cuff:
            //    SetAGridEnable(gobjPartCuff, true);
            //    break;
            case EEquipItemType.Glove:
                SetAGridEnable(gobjPartGlove, true);
                break;
            case EEquipItemType.Pants:
                SetAGridEnable(gobjPartPants, true);
                break;
            case EEquipItemType.Shoe:
                SetAGridEnable(gobjPartShoe, true);
                break;
            case EEquipItemType.WeaponOneHand:
                {
                    EquipItem eiHasEquipInHand1 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand1);
                    if (eiHasEquipInHand1 == null || eiHasEquipInHand1.baseData.type != EEquipItemType.WeaponTwoHand)
                    {
                        SetAGridEnable(gobjPartHand1, true);
                        SetAGridEnable(gobjPartHand2, true);
                    }
                }
                break;
            case EEquipItemType.WeaponTwoHand:
                {
                    EquipItem eiHasEquipInHand1 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand1);
                    EquipItem eiHasEquipInHand2 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand2);
                    if (eiHasEquipInHand1 == null && eiHasEquipInHand2 == null)
                    {
                        SetAGridEnable(gobjPartHand1, true);
                    }
                }
                
                break;
            case EEquipItemType.Shield:
                SetAGridEnable(gobjPartHand2, true);
                break;
            default:
                break;
        }
        //如果从装备栏中选中-->背包里不可替换装备禁用

        if (eiSelect._Part != EEquipPart.None)
        {
            // 遍历背包，过滤不可替换装备
            // 不同类型不可替换
            for (int i = 0; i < GameView.Inst.eiManager.itemsInBag.Count; i++)
            {
                EquipItem eiInBag = GameView.Inst.eiManager.itemsInBag[i];
                if (eiInBag.baseData.type != eiSelect.baseData.type)
                {
                    GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobjGridBag, eiInBag.BagGridId.ToString());
                    SetAGridEnable(gobjGrid, false);
                }
                //if (!gameView.CanEquip(eiInBag, eiSelect._Part))
                //{
                //    GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobjGridBag, eiInBag.bagGridId.ToString());
                //    SetAGridEnable(gobjGrid, false);
                //}
            }
        }
    }

    /// <summary>
    /// 恢复禁用状态
    /// </summary>
    public void RecoverGridDisable()
    {
        SetAGridEnable(gobjPartHelm, true);
        SetAGridEnable(gobjPartBreastplate, true);
        SetAGridEnable(gobjPartCuff, true);
        SetAGridEnable(gobjPartGlove, true);
        SetAGridEnable(gobjPartHand1, true);
        SetAGridEnable(gobjPartHand2, true);
        SetAGridEnable(gobjPartNeckLace, true);
        SetAGridEnable(gobjPartPants, true);
        SetAGridEnable(gobjPartShoe, true);
        SetAGridEnable(gobjPartShoulder, true);

        for (int i = 0; i < GameView.Inst.eiManager.itemsInBag.Count; i++)
        {
            EquipItem eiInBag = GameView.Inst.eiManager.itemsInBag[i];
            GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(gobjGridBag, eiInBag.BagGridId.ToString());
            SetAGridEnable(gobjGrid, true);
        }

    }

    void SetAGridEnable(GameObject gobj, bool isEnable) 
    {
        if (gobj == null)
        {
            return;
        }
        gobj.GetComponent<Collider>().enabled = isEnable;
        UISprite bg = gobj.GetComponent<UISprite>();

        if (gobj.transform.childCount > 0)
        {
            UISprite eiIcon = gobj.transform.GetChild(0).GetComponent<UISprite>();
            eiIcon.GetComponent<Collider>().enabled = isEnable;
            if (isEnable)
            {
                eiIcon.color = Color.white;
            }
            else
            {
                eiIcon.color = Color.gray;
            }
        }
        else
        {
            if (isEnable)
            {
                bg.alpha = 1f;
            }
            else
            {
                bg.alpha = 0.5f;
            }
        }
        
    }

    public void SetWinPosInTrade() 
    {
        GameObject gobjWin = Tools.GetGameObjectInChildByPathSimple(gameObject, "win");
        gobjWin.transform.localPosition = new Vector3(0f, 158f, 0f);
    }

    public void Close()
    {
        UIManager.Inst.CloseUIBag();
    }
}
