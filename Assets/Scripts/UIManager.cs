using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager: MonoBehaviour {

    public UIMain uiMain;

    public GameObject g_UIRootMain;
    public GameObject g_UIRootSecond;
    public GameObject g_UIRootDlg;

    private GameObject g_GobjUIMain;

    public GameObject _UIMainGobj
    {
        get { return g_GobjUIMain; }
        set { g_GobjUIMain = value; }
    }

    /// <summary>
    /// 鼠标文本提示
    /// </summary>
    UILabel txtTipCursor;

    GameView gameView;


    GameObject gobjHeroInfo;
   

    private static UIManager instance;

    public static UIManager Inst
    {
        get 
        {
            if (instance == null)
            {
                GameObject gobjUIManager = new GameObject();
                DontDestroyOnLoad(gobjUIManager);
                instance = gobjUIManager.AddComponent<UIManager>();
            }
            return instance;
        }
    }

    public void InitUI(GameView gameView)
    {
        this.gameView = gameView;

        g_UIRootMain = GameObject.Find("PanelMain");
        g_UIRootSecond = GameObject.Find("PanelSecond");
        g_UIRootDlg = GameObject.Find("PanelDlg");

        txtTipCursor = Tools.GetComponentInChildByPath<UILabel>(UICursor.instance.gameObject, "TxtTip");
    }

    UISkillInfo mUISkillInfo;
    internal void HideSkillInfo()
    {
        if (mUISkillInfo != null)
        {
            mUISkillInfo.vctl.SetVisible(false);
        }
    }

    internal void ShowSkillInfo(SkillBD data, Vector2 uiPos)
    {
        if (mUISkillInfo == null)
        {
            GameObject gobjEquipItemInfo = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "ui_skillinfo");
            mUISkillInfo = gobjEquipItemInfo.GetComponent<UISkillInfo>();
        }
        mUISkillInfo.vctl.SetVisible(true);
        mUISkillInfo.Refresh(data, uiPos);
    }

    public void ShowCursorTip(string tip, Color color)
    {
        txtTipCursor.text = tip;
        txtTipCursor.color = color;
    }

    // 显示主界面
    public bool GeneralShowUIMain(string path)
    {
        bool isSuccess = false;
        if (g_GobjUIMain != null)
        {
            DestroyObject(g_GobjUIMain);
        }
        g_GobjUIMain = Tools.AddNGUIChild(g_UIRootMain, path);
        if (g_GobjUIMain != null)
        {
            isSuccess = true;
            uiMain = g_GobjUIMain.GetComponent<UIMain>();
            uiMain.Init(gameView);
            uiMain.RefreshHeroHP();
            uiMain.RefreshHeroTL();
            uiMain.RefreshHeroVigor();
            uiMain.RefreshExp(gameView._ExpCurLevel, gameView.GetNeedExpInCurLevel());
        }
        return isSuccess;
    }

    public void RefreshMainUIHeroStateInfo()
    {
        if (uiMain != null)
        {
            uiMain.RefreshHeroHP();
            uiMain.RefreshHeroTL();
            uiMain.RefreshHeroVigor();
        }
    }

    public void ToggleUI_HeroInfo()
    {
        if (gobjHeroInfo != null)
        {
            CloseHeroInfo();
        }
        else
        {
            CloseUISkill();

            CloseNPCWords();
            CloseUINPCMutual();

            CloseUIMission();

            //UIHeroBag uhb = GetUIBag();
            //if (uhb != null)
            //{
            //    uhb.ToItemUsedSetting(false);
            //}

            //CloseUIBag();

            gobjHeroInfo = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_hero_info");
            UIHeroInfo uhi = gobjHeroInfo.GetComponent<UIHeroInfo>();
            uhi.Init(gameView);
        }
    }

    public void CloseHeroInfo() 
    {
        if (gobjHeroInfo != null)
        {
            DestroyObject(gobjHeroInfo);
        }
    }

    public void RefreshHeroInfo()
    {
        if (gobjHeroInfo != null)
        {
            UIHeroInfo uhi = gobjHeroInfo.GetComponent<UIHeroInfo>();
            uhi.Init(gameView);
        }
    }

    public void RefreshHeroExp(int curVal, int needVal)
    {
        if (uiMain != null)
        {
            uiMain.RefreshExp(curVal, needVal);
        }
    }
    #region 背包
    UIHeroBag bag;
    //背包
    public void ToggleUI_Bag()
    {
        bool showing = false;
        if (bag != null && bag.vctl.showing)
        {
            showing = true;
        }

        if (showing)
        {
            //关闭
            bag.vctl.SetVisible(false);
        }
        else
        {
            if (bag == null)
            {
                //显示
                CloseUISkill();

                CloseNPCWords();
                CloseUINPCMutual();

                CloseUIMission();

                GameObject gobjHeroBag = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_bag");
                bag = gobjHeroBag.GetComponent<UIHeroBag>();
                bag.Init(gameView);
            }
          
            bag.vctl.SetVisible(true);
        }
    }

    public void CloseUIBag()
    {
        if (bag != null)
        {
            bag.vctl.SetVisible(false);
        }
    }

    public UIHeroBag GetUIBag()
    {
        return bag;
    }

    public void RefreshHeroBagGold()
    {
        if (bag != null)
        {
            bag.RefreshGold();
        }
    }

    public void RecoverBagGridDisable()
    {
        if (bag != null)
        {
            bag.RecoverGridDisable();
        }
    }
    #endregion

    #region 装备详情
    UIEquipItemInfo _mEuqipItemInfo;
    public void ShowEquipItemInfo(EquipItem ei, Vector2 uiPos)
    {
        if (_mEuqipItemInfo == null)
        {
            GameObject gobjEquipItemInfo = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "ui_equipitem_info");
            _mEuqipItemInfo = gobjEquipItemInfo.GetComponent<UIEquipItemInfo>();
        }
        _mEuqipItemInfo.vctl.SetVisible(true);
        _mEuqipItemInfo.Refresh(ei, uiPos);
    }

    public void HideEquipItemInfo()
    {
        if (_mEuqipItemInfo != null)
        {
            _mEuqipItemInfo.vctl.SetVisible(false);
        }
    }
    #endregion

    public void GeneralTip(string txt, Color color)
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "ui_general_tip");
        UILabel uitxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uitxt.text = txt;
        uitxt.color = color;
        StartCoroutine(CoTipTime(1.5f, gobj));
    }

    IEnumerator CoTipTime(float dur, GameObject gobj)
    {
        yield return new WaitForSeconds(dur);
        if (gobj != null)
        {
            DestroyObject(gobj);
        }
    }

    // 显示物品掉落界面
    public void ShowUIItemDrops(List<EquipItem> eis)
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "DropItems/ui_dropitems");
        UIDropItems udi = gobj.GetComponent<UIDropItems>();
        udi.Init(eis);
    }

    public bool HasUI()
    {
        bool has = false;
        foreach (Transform item in g_UIRootSecond.transform)
        {
            UIVisibleCtl vctl = item.gameObject.GetComponent<UIVisibleCtl>();
            if (vctl == null || vctl.showing)
            {
                has = true;
            }
        }
        //if (g_UIRootSecond.transform.childCount > 0)
        //{
        //    has = true;
        //}
        //else if (g_UIRootDlg.transform.GetChildCount() > 1)
        //{
        //    has = true;
        //}
        return has;
    }

    public void ShowHurtedTxt(int damage, EDamageType damageType, int parryDamage = 0)
    {
        string txt = damage.ToString();
        switch (damageType)
        {
            case EDamageType.Phy:
                break;
            case EDamageType.Fire:
                txt += "!火焰伤害";
                break;
            case EDamageType.Lighting:
                txt += "!闪电伤害";
                break;
            case EDamageType.Poison:
                txt += "!毒素伤害";
                break;
            case EDamageType.Frozen:
                txt += "!冰冻伤害";
                break;
            default:
                break;
        }

        if (parryDamage > 0)
        {
            txt += string.Format("(格挡{0})", parryDamage);
        }

        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_hurt");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = txt;
        TweenPosition tp = uiTxt.GetComponent<TweenPosition>();
        int rXTo = UnityEngine.Random.Range(-100, -247);
        int rYTo = UnityEngine.Random.Range(-305, -200);
        tp.to = new Vector3(rXTo, rYTo, 0f);
        tp.PlayForward();
    }

    public void ShowDamageTxt(int damage, EDamageType damageType, int parryDamage = 0) 
    {
        //if (damageType != EDamageType.Phy)
        //{
        //    return;
        //}
        string txt = damage.ToString();
        switch (damageType)
        {
            case EDamageType.Phy:
                break;
            case EDamageType.Fire:
                txt += "!火焰伤害";
                break;
            case EDamageType.Lighting:
                txt += "!闪电伤害";
                break;
            case EDamageType.Poison:
                txt += "!毒素伤害";
                break;
            case EDamageType.Frozen:
                txt += "!冰冻伤害";
                break;
            default:
                break;
        }

        if (parryDamage > 0)
        {
            txt += string.Format("(格挡{0})", parryDamage);
        }

        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_damage");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = txt;
        TweenPosition tp = uiTxt.GetComponent<TweenPosition>();
        int rXTo = UnityEngine.Random.Range(199, 340);
        int rYTo = UnityEngine.Random.Range(198, 253);
        tp.to = new Vector3(rXTo, rYTo, 0f);
        tp.PlayForward();
    }

    public void ShowDodgeTip()
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_hurt");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = "闪避";
        TweenPosition tp = uiTxt.GetComponent<TweenPosition>();
        int rXTo = UnityEngine.Random.Range(-100, -247);
        int rYTo = UnityEngine.Random.Range(-305, -200);
        tp.to = new Vector3(rXTo, rYTo, 0f);
        tp.PlayForward();
    }

    public void ShowEnermyDodgeTip()
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_damage");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = "闪避";
        TweenPosition tp = uiTxt.GetComponent<TweenPosition>();
        int rXTo = UnityEngine.Random.Range(199, 340);
        int rYTo = UnityEngine.Random.Range(198, 253);
        tp.to = new Vector3(rXTo, rYTo, 0f);
        tp.PlayForward();
    }

    /// <summary>
    /// 显示目标战斗状态浮动信息
    /// </summary>
    public void ShowTargetBattleStateInfo(string info) 
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_target_battle_info");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = info;
    }

    /// <summary>
    /// 显示自身战斗状态浮动信息
    /// </summary>
    /// <param name="info"></param>
    public void ShowBattleStateInfo(string info)
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_battle_info");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = info;
    }

    public void ShowDSDamageTxt(string txt) 
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "txt_ds_damage");
        UILabel uiTxt = Tools.GetComponentInChildByPath<UILabel>(gobj, "txt");
        uiTxt.text = txt;
    }

    Queue<string> g_QueueMsgFloatTip = new Queue<string>();
    bool g_showingMsg = false;
    public void ShowFloatTip(string txt)
    {
        g_QueueMsgFloatTip.Enqueue(txt);
        if (!g_showingMsg)
        {
            MsgQueueHandler();
        }

    }

    /// <summary>
    /// 从队列中取出一条消息显示
    /// </summary>
    void MsgQueueHandler()
    {
        if (g_QueueMsgFloatTip.Count > 0)
        {
            string txt = g_QueueMsgFloatTip.Dequeue();
            ShowFloatTipDelayImmediately(txt);
        }
    }

    /// <summary>
    /// 立刻显示一条消息
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="delay"></param>
    void ShowFloatTipDelayImmediately(string txt)
    {
        g_showingMsg = true;
        GameObject gobjMsg = Tools.AddNGUIChild(g_UIRootMain, IPath.UI + "ui_general_tip");
        gobjMsg.name = "ui_msg";
        // 内容
        UILabel txtContext = Tools.GetComponentInChildByPath<UILabel>(gobjMsg, "txt");
        txtContext.text = txt;
        GObjLife gl = gobjMsg.GetComponent<GObjLife>();
        gl.OnDie = OnMsgDie;
    }

    // 当消息死亡时，显示下一个消息
    void OnMsgDie(GameObject gobjMsg)
    {
        g_showingMsg = false;
        MsgQueueHandler();
    }

#region NPC台词
    GameObject gobjUIWords = null;
    public void ShowNPCWords(string name, string words)
    {
        if (gobjUIWords != null)
        {
            CloseNPCWords();
        }
        gobjUIWords = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_words");
        UIWords uw = gobjUIWords.GetComponent<UIWords>();
        uw.Init(name, words);
    }

    public void CloseNPCWords()
    {
        if (gobjUIWords != null)
        {
            DestroyObject(gobjUIWords);
        }
    }
#endregion
#region  左上角提示
    UISmallTip g_UISmallTip = null;
    public void AddASmallTip(string content)
    {
        if (g_UISmallTip == null)
        {
            g_UISmallTip = Tools.AddNGUIChild(g_UIRootMain, IPath.UI + "ui_smalltip").GetComponent<UISmallTip>();
        }
        g_UISmallTip.AddATip(content);
    }
#endregion

    #region 多地图切换
    UIChangeMapTip g_UIChangeMapTip = null;
    public void ShowUIChangeMapTip(GameMapBaseData mapTarget, int targetMGId)
    {
        CloseUIChangeMapTip();
        g_UIChangeMapTip = Tools.AddNGUIChild(g_UIRootMain, IPath.UI + "ui_changemap_tip").GetComponent<UIChangeMapTip>();
        g_UIChangeMapTip.Init(mapTarget, targetMGId);
    }

    public void CloseUIChangeMapTip()
    {
        if (g_UIChangeMapTip != null)
        {
            DestroyObject(g_UIChangeMapTip.gameObject);
        }
    }
#endregion

#region 技能
    public UISkillsNew gUISkill = null;
    void ShowUISkill()
    {
        CloseUIBag();
        CloseNPCWords();
        CloseUINPCMutual();
        CloseUIMission();

        if (gUISkill == null)
        {
            gUISkill = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_skillsnew").GetComponent<UISkillsNew>();
            gUISkill.Init();
        }

        gUISkill.vCtl.SetVisible(true);
    }

    public void CloseUISkill()
    {
        if (gUISkill != null)
        {
            gUISkill.vCtl.SetVisible(false);
        }
    }

    public void ToggleUI_Skill()
    {
        bool showing = false;
        if (gUISkill != null && gUISkill.vCtl.showing)
        {
            showing = true;
        }

        if (showing)
        {
            CloseUISkill();
        }
        else
        {
            ShowUISkill();
        }
    }
#endregion
    #region 任务UI
    UIMission uiMission = null;
    public void ShowUIMission()
    {
        CloseAllUISecond();
        uiMission = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_mission").GetComponent<UIMission>();
        uiMission.Init();
    }

    public void CloseUIMission()
    {
        if (uiMission != null)
        {
            DestroyObject(uiMission.gameObject);
        }
    }

    public void ToggleUIMission()
    {
        if (uiMission == null)
        {
            ShowUIMission();
        }
        else
        {
            CloseUIMission();
        }
    }
#endregion
    public void CloseAllUISecond()
    {
        foreach (Transform tfChild in g_UIRootSecond.transform)
        {
            DestroyObject(tfChild.gameObject);
        }
    }


    #region 怪物信息
    GameObject gobjUIEnermyInfo = null;
    public void ShowUIEnermyInfo(Enermy enermy) 
    {
        gobjUIEnermyInfo = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_enermy_info");
        // name
        UILabel txtName = Tools.GetComponentInChildByPath<UILabel>(gobjUIEnermyInfo, "name");
        txtName.text = enermy._MonsterBD.name;
        // level
        UILabel txtLevel = Tools.GetComponentInChildByPath<UILabel>(gobjUIEnermyInfo, "level");
        txtLevel.text = "LV" + enermy._MonsterBD.level.ToString();
        // 技能
        IMonSkill[] skills = enermy.GetComponents<IMonSkill>();
        UIGrid gridSkills = Tools.GetComponentInChildByPath<UIGrid>(gobjUIEnermyInfo, "grid_skill");
        for (int i = 0; i < skills.Length; i++)
        {
            IMonSkill itemSkill = skills[i];
            if (itemSkill.skillBD.id == 27 || itemSkill.skillBD.id == 28)
            {
                continue;
            }
            GameObject gobjItemSkill = Tools.AddNGUIChild(gridSkills.gameObject, IPath.UI + "item_skill_enermy");
            // icon
            UISprite icon = Tools.GetComponentInChildByPath<UISprite>(gobjItemSkill, "icon");
            icon.spriteName = itemSkill.skillBD.icon;
            // name
            UILabel txtNameSkill = Tools.GetComponentInChildByPath<UILabel>(gobjItemSkill, "txt_name");
            txtNameSkill.text = itemSkill.skillBD.name;
            // level
            UILabel txtLevelSkill = Tools.GetComponentInChildByPath<UILabel>(gobjItemSkill, "txt_lv");
            txtLevelSkill.text = "LV" + itemSkill.level.ToString();
            // desc
            UILabel txtDesc = Tools.GetComponentInChildByPath<UILabel>(gobjItemSkill, "txt_desc");
            txtDesc.text = itemSkill.skillBD.ToDesc(itemSkill.level);
        }
        gridSkills.Reposition();

        UIButton btnClose = Tools.GetComponentInChildByPath<UIButton>(gobjUIEnermyInfo, "btn_close");
        btnClose.onClick.Add(new EventDelegate(Btn_CloseEnermyInfo));
    }

    void Btn_CloseEnermyInfo() 
    {
        CloseUIEnermyInfo();
    }

    public void CloseUIEnermyInfo() 
    {
        if (gobjUIEnermyInfo != null)
        {
            DestroyObject(gobjUIEnermyInfo);
        }
    }
#endregion

#region 攻击进度条

#endregion

    GameObject gobjDieUI = null;
    public void ShowHeroDieUI() 
    {
        if (gobjDieUI != null)
        {
            return;
        }
        gobjDieUI = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_die");

        int lostGold = gameView.GetLostGoldOnDie();

        UILabel txtTipLostGold = Tools.GetComponentInChildByPath<UILabel>(gobjDieUI, "tip_lost_gold");
        txtTipLostGold.text = txtTipLostGold.text.Replace("&", lostGold.ToString());

        GameView.Inst.eiManager._Gold -= lostGold;

        UIButton btnComfirm = Tools.GetComponentInChildByPath<UIButton>(gobjDieUI, "btn_comfirm");
        btnComfirm.onClick.Add(new EventDelegate(BtnComfirm_DieUI));
    }

    void BtnComfirm_DieUI() 
    {
        gameView.OnComfirmDie();
    }

    public void RemoveHeroDieUI() 
    {
        if (gobjDieUI != null)
        {
            DestroyObject(gobjDieUI);
        }
    }

    GameObject gobjUINPCMutual;
    /// <summary>
    /// 显示NPC交互选择界面
    /// </summary>
    public UINPCMutual ShowUINPCMutual() 
    {
        gobjUINPCMutual = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_npc_mutual");
        return gobjUINPCMutual.GetComponent<UINPCMutual>();
    }

    public void CloseUINPCMutual() 
    {
        if (gobjUINPCMutual != null)
        {
            DestroyObject(gobjUINPCMutual);
        }
    }


    GameObject gobjUITrade;
    /// <summary>
    /// 显示交易UI
    /// </summary>
    public UITrade ShowUITrade() 
    {
        gobjUITrade = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_trade");
        return gobjUITrade.GetComponent<UITrade>();
    }

    public void CloseUITrade() 
    {
        if (gobjUITrade != null)
        {
            DestroyObject(gobjUITrade);
        }
    }

    public UITrade GetUITrade() 
    {
        UITrade ut = null;
        if (gobjUITrade != null)
        {
            ut = gobjUITrade.GetComponent<UITrade>();
        }
        return ut;
    }

    UITrial uiTrial;
    public UITrial ShowUITrial() 
    {
        GameObject gobjUITrial = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_trial");
        uiTrial = gobjUITrial.GetComponent<UITrial>();
        return uiTrial;
    }

    public void CloseUITrial() 
    {
        if (uiTrial != null)
        {
            DestroyObject(uiTrial.gameObject);
        }
    }

    UISmallBag uiSmallBag;
    public UISmallBag ShowUISmallBag()
    {
        GameObject gobjUISmallBag = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_small_bag");
        uiSmallBag = gobjUISmallBag.GetComponent<UISmallBag>();
        return uiSmallBag;
    }

    public void CloseUISmallBag()
    {
        if (uiSmallBag != null)
        {
            DestroyObject(uiSmallBag.gameObject);
        }
    }

    #region 传送站
    UITransfer uiTransfer;
    public UITransfer ShowUITransfer()
    {
        GameObject gobjUITransfer = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_transfer");
        uiTransfer = gobjUITransfer.GetComponent<UITransfer>();
        return uiTransfer;
    }

    public void CloseUITransfer()
    {
        if (uiTransfer != null)
        {
            DestroyObject(uiTransfer.gameObject);
        }
    }

    #endregion

    #region 地图提示
    UIComfirmMapTip mComfirmMapTip;
    internal void ShowUIComfirmMapTips(string tips)
    {
        GameObject gobjComfirmMapTip = Tools.AddNGUIChild(g_UIRootMain, IPath.UI + "ui_map_comfirm_tip");
        mComfirmMapTip = gobjComfirmMapTip.GetComponent<UIComfirmMapTip>();
        mComfirmMapTip.Init(tips);
    }

    internal void ClseeUIMapTip()
    {
        if (mComfirmMapTip != null)
        {
            DestroyObject(mComfirmMapTip.gameObject);
        }
    }
    #endregion

    #region 通用弹出框
    UIDefaultDlg mDefDlg;
    internal void ShowDefaultDlg(string tip, Action onComfirm)
    {
        GameObject gobj = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "ui_def_dlg");
        mDefDlg = gobj.GetComponent<UIDefaultDlg>();
        mDefDlg.Init(tip, onComfirm);
    }
    #endregion

    #region 装备操作
    public void OnDropEquipItemTo(GameObject target, bool pressed)
    {
        if (UIEquipItemOperControll.curDropItem != null)
        {
            if (target.CompareTag("EIBagGrid"))
            {
                // 放置到背包空格中
                OnDropToBagGrid(target);
            }
            else if (target.CompareTag("EquipItem"))
            {
                OnDropToOtherEuqipItem(target);
            }
            else if (target.CompareTag("EIEquipGrid"))
            {
                OnDropToEquipGrid(target);
            }
            else if (target.CompareTag("DestroyGrid"))
            {
                OnDropToDestroy(target);
            }
            else if (target.CompareTag("SellGrid"))
            {
                OnDropToSell(target);
            }
            else if (target.CompareTag("UseGrid"))
            {
                OnDropToUse(target);
            }
            else
            {
                //复原
                UIEquipItemOperControll.curDropItem.Recover();
            }
        }
    }

    /// <summary>
    /// 配置道具
    /// </summary>
    /// <param name="target"></param>
    private void OnDropToUse(GameObject target)
    {
        // 配置道具
        EquipItem ei = UIEquipItemOperControll.curDropItem.mEquipItem;
        if (ei.baseData.useType != EEquipItemUseType.None)
        {
            ItemUesdGrid iug = target.GetComponent<ItemUesdGrid>();
            int index = iug.index;
            GameView.Inst.eiManager.SetItemUsed(index, ei.baseData.id);
            GameView.Inst.eiManager.SaveItemUesd();
            iug.SetEquipItem(ei);
            // UI
            iug.RefershUI();
        }

        UIEquipItemOperControll.curDropItem.Recover();
    }

    /// <summary>
    /// 出售
    /// </summary>
    /// <param name="target"></param>
    private void OnDropToSell(GameObject target)
    {
        EquipItem ei = UIEquipItemOperControll.curDropItem.mEquipItem;

        // UI背包移除
        UIManager.Inst.GetUIBag().RemoveAEquipItem(ei);
        // UI人物外观更新
        GameManager.gameView.UpdateOnChangeEquip(ei, false);
        // 添加金钱
        int price = ei.GetTradePrice();
        GameView.Inst.eiManager._Gold += price;

        // 移除装备数据
        GameView.Inst.eiManager.RemoveEquipItem(ei);
        // 保存装备信息
        GameView.Inst.eiManager.SaveEquipItems();

        // 回到原位
        UIEquipItemOperControll.curDropItem.Recover();
    }

    /// <summary>
    /// 摧毁
    /// </summary>
    /// <param name="target"></param>
    private void OnDropToDestroy(GameObject target)
    {
        EquipItem ei = UIEquipItemOperControll.curDropItem.mEquipItem;

        // 移除装备数据
        GameView.Inst.eiManager.RemoveEquipItem(ei);
        // 保存装备信息
        GameView.Inst.eiManager.SaveEquipItems();
        
        // UI背包移除
        Inst.GetUIBag().RemoveAEquipItem(ei);
        // UI人物外观更新
        GameManager.gameView.UpdateOnChangeEquip(ei, false);
        // 移除配置
        Inst.uiMain.ClearItemUsed(ei);

        // 回到原位
        UIEquipItemOperControll.curDropItem.Recover();
    }

    private void OnDropToEquipGrid(GameObject target)
    {
        UIEquipItemOperControll.curDropItem.transform.parent = target.transform;
        UIEquipItemOperControll.curDropItem.transform.localPosition = Vector3.zero;

        EquipItem ei = UIEquipItemOperControll.curDropItem.mEquipItem;
        EEquipPart partTo = target.GetComponent<HeroItemGrid>().part;

        GameView.Inst.DoMoveAEquipItemToEquip(ei, partTo);

        GameView.Inst.eiManager.SaveEquipItems();

        UIEquipItemOperControll.curDropItem.Recover();
    }

    private void OnDropToOtherEuqipItem(GameObject target)
    {
        if (target == UIEquipItemOperControll.curDropItem.gameObject)
        {
            UIEquipItemOperControll.curDropItem.Recover();
            return;
        }

        EquipItem ei = UIEquipItemOperControll.curDropItem.mEquipItem;
        UIEquipItemOperControll operCtlOther = target.GetComponent<UIEquipItemOperControll>();
        EquipItem eiTo = operCtlOther.mEquipItem;

        bool succed = false;

        //gameView.
        // 装备移到背包
        if (ei.Part != EEquipPart.None && eiTo.Part == EEquipPart.None)
        {
            EEquipPart partOri = ei.Part;
            int gridIdTo = eiTo.BagGridId;
            if (gridIdTo > 0)
            {
                GameView.Inst.DoMoveAEquipItemToBag(ei, gridIdTo);
                GameView.Inst.DoMoveAEquipItemToEquip(eiTo, partOri);
                succed = true;
            }

        }
        // 背包移到背包
        else if (ei.Part == EEquipPart.None && eiTo.Part == EEquipPart.None)
        {
            int gridOri = ei.BagGridId;
            int gridIdTo = eiTo.BagGridId;
            if (gridOri > 0 && gridIdTo > 0)
            {
                GameView.Inst.eiManager.MoveAEquipItemToBag(ei, gridIdTo);
                GameView.Inst.eiManager.MoveAEquipItemToBag(eiTo, gridOri);
                succed = true;
            }

        }
        // 背包移到装备栏
        else if (ei.Part == EEquipPart.None && eiTo.Part != EEquipPart.None)
        {
            int gridOri = ei.BagGridId;
            EEquipPart partTo = eiTo.Part;

            GameView.Inst.DoMoveAEquipItemToBag(eiTo, gridOri);
            GameView.Inst.DoMoveAEquipItemToEquip(ei, partTo);
            succed = true;
        }

        if (succed)
        {
            // 互调位置
            Transform tfMDragParent = UIEquipItemOperControll.curDropItem.transform.parent;
            UIEquipItemOperControll.curDropItem.transform.parent = target.transform.parent;
            target.transform.parent = tfMDragParent;
            target.transform.localPosition = Vector3.zero;
            UIEquipItemOperControll.curDropItem.transform.localPosition = Vector3.zero;
            GameView.Inst.eiManager.SaveEquipItems();
        }

        UIEquipItemOperControll.curDropItem.Recover();
    }

    private void OnDropToBagGrid(GameObject target)
    {
        UIEquipItemOperControll.curDropItem.transform.parent = target.transform;
        UIEquipItemOperControll.curDropItem.transform.localPosition = Vector3.zero;

        EquipItem ei = UIEquipItemOperControll.curDropItem.mEquipItem;
        int gridIdTo = int.Parse(target.name);
        GameView.Inst.DoMoveAEquipItemToBag(ei, gridIdTo);
        GameView.Inst.eiManager.SaveEquipItems();
        UIEquipItemOperControll.curDropItem.Recover();
    }
    #endregion

    #region 少女提示
    UIGirlTip girlTip;
    public void ShowUIGirlTip(ItemGrilTip item)
    {
        GameObject gobjGirlTip = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_girltip");
        girlTip = gobjGirlTip.GetComponent<UIGirlTip>();
        girlTip.Init(item);
    }

    public void CloseUIGirlTip()
    {
        if (girlTip != null)
        {
            MonoKit.DestroyObject(girlTip.gameObject);
        }
    }
    #endregion
}
