using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    GameView gameView;


    GameObject gobjHeroInfo;
    GameObject gobjHeroBag;
    GameObject gobjEquipItemInfo;

    private static UIManager instance;

    public static UIManager _Instance
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
            uiMain.RefreshHeroMP();
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
            uiMain.RefreshHeroMP();
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

    //背包
    public void ToggleUI_Bag()
    {
        if (gobjHeroBag != null)
        {
            DestroyObject(gobjHeroBag);
        }
        else
        {
            CloseUISkill();

            CloseNPCWords();
            CloseUINPCMutual();

            CloseUIMission();
            //CloseHeroInfo();

            gobjHeroBag = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_bag");
            RefreshHeroBag();
        }
    }

    public void CloseUIBag() 
    {
        if (gobjHeroBag != null)
        {
            DestroyObject(gobjHeroBag);
        }
    }

    private void RefreshHeroBag()
    {
        if (gobjHeroBag != null)
        {
            UIHeroBag uhb = gobjHeroBag.GetComponent<UIHeroBag>();
            uhb.Init(gameView);
        }
    }

    public UIHeroBag GetUIBag()
    {
        UIHeroBag uhb = null;
        if (gobjHeroBag != null)
        {
             uhb = gobjHeroBag.GetComponent<UIHeroBag>();
        }
        return uhb;
    }

    public void RefreshHeroBagGold()
    {
        if (gobjHeroBag != null)
        {
            UIHeroBag uhb = gobjHeroBag.GetComponent<UIHeroBag>();
            uhb.RefreshGold();
        }
    }

    public void OnBtnPress(UIButton btn, bool pressed)
    {
        if (btn.CompareTag("EquipItem"))
        {
            // 按住一件装备
            // 无论在哪个界面，都显示装备信息
            // 如果在背包界面，显示可放置格子
            if (pressed)
            {
                EquipItem ei = btn.data as EquipItem;
                ShowEquipItemInfo(ei);

                if (gobjHeroBag != null)
                {
                    UIHeroBag uhb = gobjHeroBag.GetComponent<UIHeroBag>();
                    if (uhb != null)
                    {
                        uhb.DisableGrid(ei);
                    }
                }
            }
            else
            {
                HideEquipItemInfo();
                if (gobjHeroBag != null)
                {
                    RecoverBagGridDisable();
                }
            }
        }
    }

    public void RecoverBagGridDisable()
    {
        UIHeroBag uhb = gobjHeroBag.GetComponent<UIHeroBag>();
        uhb.RecoverGridDisable();
    }

    void ShowEquipItemInfo(EquipItem ei) 
    {
        if (gobjEquipItemInfo != null)
        {
            DestroyObject(gobjEquipItemInfo);
        }
        gobjEquipItemInfo = Tools.AddNGUIChild(g_UIRootDlg, IPath.UI + "ui_equipitem_info");
        UIEquipItemInfo ueii = gobjEquipItemInfo.GetComponent<UIEquipItemInfo>();
        ueii.Init(gameView, ei);
    }

    public void HideEquipItemInfo()
    {
        if (gobjEquipItemInfo != null)
        {
            DestroyObject(gobjEquipItemInfo);
        }
    }



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
        if (g_UIRootSecond.transform.childCount > 0)
        {
            has = true;
        }
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
            case EDamageType.Forzen:
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
        int rXTo = Random.Range(-100, -247);
        int rYTo = Random.Range(-305, -200);
        tp.to = new Vector3(rXTo, rYTo, 0f);
        tp.Play();
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
            case EDamageType.Forzen:
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
        int rXTo = Random.Range(199, 340);
        int rYTo = Random.Range(198, 253);
        tp.to = new Vector3(rXTo, rYTo, 0f);
        tp.Play();
       
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
    UISkills gUISkill = null;
    void ShowUISkill()
    {
        CloseAllUISecond();
        
        gUISkill = Tools.AddNGUIChild(g_UIRootSecond, IPath.UI + "ui_skills").GetComponent<UISkills>();
        gUISkill.Init();
    }

    void CloseUISkill()
    {
        if (gUISkill != null)
        {
            DestroyObject(gUISkill.gameObject);
        }
    }

    public void ToggleUI_Skill()
    {
        if (gUISkill == null)
        {
            ShowUISkill();
        }
        else
        {
            CloseUISkill();
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

        gameView._MHero._Gold -= lostGold;

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
}
