using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Text;
using SimpleJSON;
using System;

public enum GameRoundLogicState
{
    PlayerRoundStart,//玩家回合开始 
    Normal,//玩家输入阶段
    HeroAction,//玩家角色行为阶段。移动等
    WorldEventAction,//世界事件阶段
    NPCAction,
    Battle
}

/// <summary>
/// 玩家输入状态
/// </summary>
public enum EPlayInputState
{
    Nomal,
    ChooseTarget //选择目标
}

public class GameView : MonoBehaviour {
    public static GameView Inst;

    public CameraControl cameraControl;

    /// <summary>
    /// 装备数据管理器
    /// </summary>
    public EquipItemManager eiManager;

   

    /// <summary>
    /// 当前地图
    /// </summary>
    public GameMap mCurGameMap;
    /// <summary>
    /// 世界中的地图缓存
    /// </summary>
    public Dictionary<int, GameMap> mDicMapCaches = new Dictionary<int, GameMap>();

    //public ActionPointSet acpSet;

    public Camera cameraMain;

    public Camera cameraUI;

    //Hero hero;

    public Hero _MHero
    {
        get { return GameManager.hero; }
    }

    GameRoundLogicState state;

    public GameRoundLogicState _RoundLogicState
    {
        get { return state; }
        set { state = value;}
    }

    EPlayInputState gInputState = EPlayInputState.Nomal;

    ISkill gSkillTractic; // 当前战术技能

    EquipItem gItemToUse = null;//当前要使用的道具 

    MapGrid gCurSelectMg = null;//当前选择的目标格子

    bool enableInput = true;

    public bool EnableInput
    {
        get { return enableInput; }
        set { enableInput = value;}
    }

    Enermy g_Target;
    MapGrid g_TargetMapGrid;

    DbAccess dba;

    int propHasAllotToStr = 0;
    public int _PropHasAlltoToStr
    {
        get
        {
            return propHasAllotToStr;
        }

        set
        {
            propHasAllotToStr = value;
            PlayerPrefs.SetInt(IConst.KEY_HASALLOT_STR, value);
        }
    }

    int propHasAllotToAgi = 0;
    public int _PropHasAlltoToAgi
    {
        get
        {
            return propHasAllotToAgi;
        }

        set
        {
            propHasAllotToAgi = value;
            PlayerPrefs.SetInt(IConst.KEY_HASALLOT_AGI, value);
        }
    }

    int propHasAllotToInt = 0;
    public int _PropHasAllotToTen
    {
        get
        {
            return propHasAllotToInt;
        }
        set
        {
            propHasAllotToInt = value;
            PlayerPrefs.SetInt(IConst.KEY_HASALLOT_INT, value);
        }
    }

    internal bool HasKilledRecord(Enermy enermy)
    {
        bool r = false;
        string record = PlayerPrefs.GetString(IConst.KEY_KILL_RECORD);
        if (!string.IsNullOrEmpty(record) && record.Contains(enermy.guid + "&"))
        {
            r = true;
        }
        return r;
    }

    int propHasAllotToSta = 0;
    public int _PropHasAllotToSta
    {
        get
        {
            return propHasAllotToSta;
        }
        set
        {
            propHasAllotToSta = value;
            PlayerPrefs.SetInt(IConst.KEY_HASALLOT_STA, value);
        }
    }

    int propHasAllotToEnd = 0;
    public int _PropHasAllotToEnd
    {
        get
        {
            return propHasAllotToEnd;
        }

        set
        {
            propHasAllotToEnd = value;
            PlayerPrefs.SetInt(IConst.KEY_HASALLOT_END, value);
        }
    }

    MapGrid mgInterActive = null; //当前点击交互格子 

    int enermysCount = 0; // 当前地图敌人总数
    int enermyKilled = 0; // 已击杀敌人数量

    int curRound = 0; // 当前回合。进入地图清0，每行走一格+1

    List<Enermy> mListEnermys
    {
        get
        {
            return mCurGameMap.mListEnermys;
        }
    }

    /// <summary>
    /// 当前地图的格子
    /// </summary>
    public List<MapGrid> mListMGs
    {
        get
        {
            return mCurGameMap.mListMGs;
        }
    }

    public int _CurRound
    {
        get { return curRound; }
        set 
        { 
            curRound = value;
            if (UIManager.Inst.uiMain != null)
            {
                UIManager.Inst.uiMain.ShowRound(curRound);
            }
            EventsMgr.GetInstance().TriigerEvent(eEventsKey.RoundChange, curRound);
        }
    }

    bool hasKillCurBoss = false;

    PlayerMoveCtl playerMoveCtl;
    PlayerInputCtl playerInputCtl;

    public void AddToListEnermy(Enermy enermy)
    {
        mListEnermys.Add(enermy);
    }

    public void ClearListEnermy()
    {
        mListEnermys.Clear();
    }

    public void RemoveFormListEnermy(Enermy enermy)
    {
        mListEnermys.Remove(enermy);
    }

    void Awake()
    {
#if OutLog
        OutLog.enableLog = true;
        OutLog.ToggleLog();
#endif
        GameManager.gameView = this;
        GameManager.ConTODB();
    }

    void InitCommonCPU()
    {
        GameObject gobjComCPU = new GameObject();
        gobjComCPU.name = "_CommonCPU";
        DontDestroyOnLoad(gobjComCPU);
        GameManager.commonCPU = gobjComCPU.AddComponent<CommonCPU>();
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(CoInit());
    }

    public void AddToMapCache(int id, GameMap map)
    {
        if (!mDicMapCaches.ContainsKey(id))
        {
            mDicMapCaches.Add(id, map);
        }
    }

    public void RemoveFromMapCache(int id)
    {
        if (mDicMapCaches.ContainsKey(id))
        {
            mDicMapCaches.Remove(id);
        }
    }

    internal void OnOpenDoor(ItemDoor door)
    {
        if (door.needItemID != 0)
        {
            EquipItem ei = eiManager.GetEquipItemInBagById(door.needItemID);
            if (ei != null)
            {
                //有道具
                door.Open();
                //移除道具
                eiManager.RemoveFromItemsInBag(ei);
                //保存记录
                SaveDoorOpend(door.guid);
            }
            else
            {
                EquipItemBaseData bd = GameDatas.GetEIBD(door.needItemID);
                if (bd != null)
                {
                    UIManager.Inst.GeneralTip("需要" + bd.name, Color.red);
                }
            }
        }
        else
        {
            //直接打开
            door.Open();
            //保存记录
            SaveDoorOpend(door.guid);
        }
    }

    internal void OnTouchGirlTip(ItemGrilTip girlTip)
    {
        //进入cg模式
        ToCGMode(true);
        //显示ui
        UIManager.Inst.ShowUIGirlTip(girlTip);
    }

    public void ToCGMode(bool toCG)
    {
        UIManager.Inst.uiMain.vctl.SetVisible(!toCG);
    }

    IEnumerator CoInit()
    {
        Inst = this;
        GameManager.gameView = this;

        eiManager = new EquipItemManager();

        InitCommonCPU();

        GameManager.commonCPU.InitSprites();

       
        UIManager.Inst.InitUI(this);

        _RoundLogicState = GameRoundLogicState.Normal;

        EventsMgr.GetInstance().Init();

        // 读取游戏记录
        propHasAllotToStr = ReadHeroPropertyToStr();
        propHasAllotToAgi = ReadHeroPropertyToAgi();
        propHasAllotToInt = ReadHeroPropertyToInt();
        propHasAllotToSta = ReadHeroPropertyToSta();

        CreatePlayer();


        ShowUI_Main();

        PlayerToMap(GameDatas.GetGameMapBD(GameManager.commonCPU.ReadCurHomeMap()), -1);

        playerMoveCtl = new PlayerMoveCtl(_MHero);

        //输入控制器
        playerInputCtl = new PlayerInputCtl(this);
        yield return 0;
    }

    /// <summary>
    /// 激活一个传送站
    /// </summary>
    /// <param name="transfer"></param>
    internal void ActiveTransfer(ItemTransfer transfer)
    {
        transfer.actived = true;
        transfer.RefreshActived();
        SaveTransferActived(transfer.mapID);
    }

    // 设置玩家初始位置
    void SetPlayerToStartGrid()
    {
        MapGrid mgStart = FinePlayerStartGrid();
        _MHero.SetMapGrid(mgStart);
        cameraControl.target = _MHero.gameObject;
    }

    /// <summary>
    /// 随机生成怪物
    /// </summary>
    void InitMonsters()
    {

        return;//######

        ClearListEnermy();

        int monCount = mCurGameMap.baseData.monsterCount;
        for (int i = 0; i < monCount; i++)
        {
            MapGrid mg = GetAEmptyGrid(true);
            // 创建一个怪物在格子上
            MonsterBaseData monBD = GameDatas.GetMonsterBDBetweenLevel(mCurGameMap.baseData.minLevel, mCurGameMap.baseData.maxLevel);
            CreateAEnermy(mg, monBD);
        }
    }

    /// <summary>
    /// 当休息时
    /// </summary>
    /// 
    List<int> mMapCacheToRemove = new List<int>();
    internal void OnRest()
    {
        //恢复生命值
        _MHero.RecoverHp(_MHero.Prop.HpMax - _MHero.Prop.Hp);
        mMapCacheToRemove.Clear();
        //删除地图缓存
        foreach (int id in mDicMapCaches.Keys)
        {
            if (id != mCurGameMap.baseData.id)
            {
                mMapCacheToRemove.Add(id);
                DestroyObject(mDicMapCaches[id].gameObject);
            }
        }

        for (int i = 0; i < mMapCacheToRemove.Count; i++)
        {
            mDicMapCaches.Remove(mMapCacheToRemove[i]);
        }
    }

    /// <summary>
    /// 随机生成神坛
    /// </summary>
    void InitAltars() 
    {

        return;

        int count = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            MapGrid mg = GetAEmptyGrid(true);
            // 创建一个神坛在格子上
            NPCBaseData bdNPC = GameDatas.GetARandomAltar();
            GameObject gobjAltar = Tools.LoadResourcesGameObject(IPath.Items + bdNPC.model, mg.gameObject, 0f, 0f, 0f);
            ItemNPC itemNPC = gobjAltar.AddComponent<ItemNPC>();
            itemNPC.npcData = bdNPC;
            itemNPC.Init(bdNPC);
        }
    }

    Enermy CreateAEnermy(MapGrid mg, MonsterBaseData monBD) 
    {
        Enermy enermyMon = null;
        if (monBD != null)
        {
            GameObject gobjMon = Tools.LoadResourcesGameObject(IPath.Items + monBD.model, mg.gameObject, 0f, 0f, 0f);
            enermyMon = gobjMon.AddComponent<Enermy>();
            enermyMon.Init(monBD);
        }
        return enermyMon; 
    }

    int GetEnermyCountInCurMap() 
    {
        int count = 0;
        foreach (Transform child in mCurGameMap.transform)
        {
            MapGrid mgTemp = child.GetComponent<MapGrid>();
            if (mgTemp != null)
            {
                if (mgTemp.GetItem<Enermy>() != null)
                {
                    count++;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// 随机取一个空地
    /// </summary>
    /// checkEnablCreateMon : 检测是否允许刷怪
    /// <returns></returns>
    MapGrid GetAEmptyGrid(bool checkEnablCreateMon)
    {
        MapGrid mg = null;
        int minId = 0;
        int maxId = mCurGameMap.baseData.width * mCurGameMap.baseData.height - 1;
        while (true)
        {
            int ranMGid = UnityEngine.Random.Range(minId, maxId + 1);
            MapGrid mgRan = GetMapGridById(ranMGid);
            if (mgRan.Type == EGridType.None && mgRan.GetItemGobj() == null)
            {
                if (checkEnablCreateMon)
                {
                    if (mgRan.enableCreateAMon)
                    {
                        mg = mgRan;
                        break;
                    }
                }
                else
                {
                    mg = mgRan;
                    break;
                }
                
            }
        }
        return mg;
    }

    public void EchoError(string txt) 
    {
        Debug.LogError(txt);
    }

    void ShowUI_Main()
    {
        UIManager.Inst.GeneralShowUIMain(IPath.UI + "ui_main");
    }

    /// <summary>
    /// 创建游戏地图
    /// </summary>
    public IEnumerator CoLoadMap(GameMapBaseData gameMap)
    {
        if (mCurGameMap != null)
        {
            SetMapGobjVisible(mCurGameMap.gameObject, false);
            yield return 0;
        }
        if (mDicMapCaches.ContainsKey(gameMap.id))
        {
            //有缓存，显示
            GameObject gobjMap = mDicMapCaches[gameMap.id].gameObject;
            SetMapGobjVisible(gobjMap, true);
            mCurGameMap = gobjMap.GetComponent<GameMap>();
        }
        else
        {
            //无缓存， 创建新地图
            GameObject gobjMap = Tools.LoadResourcesGameObject(IPath.Maps + gameMap.scene);
            gobjMap.transform.position = Vector3.zero;
            gobjMap.transform.localEulerAngles = Vector3.zero;
            mCurGameMap = gobjMap.GetComponent<GameMap>();
            mCurGameMap.Init(gameMap);
            //加入缓存
            AddToMapCache(gameMap.id, mCurGameMap);
            //初始化
            if (!mCurGameMap.baseData.isHome)
            {
                InitAltars();
                InitMonsters();
            }
        }
    }

    public void SetMapGobjVisible(GameObject gobjMap, bool visible)
    {
        if (visible)
        {
            gobjMap.transform.position = Vector3.zero;
        }
        else
        {
            gobjMap.transform.position = Vector3.right * 1000f;
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (_RoundLogicState == GameRoundLogicState.Normal && InputState == EPlayInputState.Nomal)
        {
            UpdateNormalInput();
        }
        else if (_RoundLogicState == GameRoundLogicState.Battle)
        {
            UpdateBattleInput();
        }
    }

  

    /// <summary>
    /// 战斗状态输入
    /// </summary>
    private void UpdateBattleInput()
    {
        if (Input.GetButtonDown("Stop"))
        {

            _MHero.BsManager.ActionStop();
        }
        else if (Input.GetButton("Attack"))
        {
            //攻击
            _MHero.BsManager.ActionAtk();
        }

        if (Input.GetButton("PowerAttack"))
        {
            //蓄力攻击
            _MHero.BsManager.ActionPoweringStart();
        }
        else if (Input.GetButtonUp("PowerAttack"))
        {
            //蓄力结束
            _MHero.BsManager.ActionPoweringOver();
        }

        if (Input.GetButton("Def"))
        {
            _MHero.BsManager.ActionDef();
        }
        if (Input.GetButtonUp("Def"))
        {
            _MHero.BsManager.ActionStopDef();
        }

        if (Input.GetButtonDown("Dodge"))
        {
            //闪避时间
            _MHero.BsManager.ActionDodge(Hero.Inst.GetDodgeDur());
        }

        if (Input.GetButtonDown("Skill1"))
        {
            ISkill skill = _MHero.GetSkillByIndex(1);
            if (skill != null)
            {
                _MHero.BsManager.ActionSkill(skill);
            }
        }
        else if (Input.GetButtonDown("Skill2"))
        {
            ISkill skill = _MHero.GetSkillByIndex(2);
            if (skill != null)
            {
                _MHero.BsManager.ActionSkill(skill);
            }
        }
        else if (Input.GetButtonDown("Skill3"))
        {
            ISkill skill = _MHero.GetSkillByIndex(3);
            if (skill != null)
            {
                _MHero.BsManager.ActionSkill(skill);
            }
        }
        else if (Input.GetButtonDown("Skill4"))
        {
            ISkill skill = _MHero.GetSkillByIndex(4);
            if (skill != null)
            {
                _MHero.BsManager.ActionSkill(skill);
            }
        }
    }

    /// <summary>
    /// 普通状态输入更新
    /// </summary>
    private void UpdateNormalInput()
    {
        if (Input.GetAxisRaw("Vertical") >= 1)
        {
            if (playerMoveCtl != null)
            {
                playerMoveCtl.OnKeyArrow(EDirection.Up);
            }
        }
        else if (Input.GetAxisRaw("Vertical") <= -1)
        {
            if (playerMoveCtl != null)
            {
                playerMoveCtl.OnKeyArrow(EDirection.Down);
            }
        }
        else if (Input.GetAxisRaw("Horizontal") >= 1)
        {
            if (playerMoveCtl != null)
            {
                playerMoveCtl.OnKeyArrow(EDirection.Right);
            }
        }
        else if (Input.GetAxisRaw("Horizontal") <= -1)
        {
            if (playerMoveCtl != null)
            {
                playerMoveCtl.OnKeyArrow(EDirection.Left);
            }
        }

        // 打开人物属性界面
        if (Input.GetButtonDown("HeroInfo"))
        {
            UIManager.Inst.ToggleUI_HeroInfo();
        }

        if (Input.GetButtonDown("Bag"))
        {
            UIManager.Inst.ToggleUI_Bag();
        }

        if (Input.GetButtonDown("Skill"))
        {
            UIManager.Inst.ToggleUI_Skill();
        }

        if (Input.GetButtonDown("Log"))
        {
            UIManager.Inst.ToggleUIMission();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("删除存档");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            // 保存
            eiManager.SaveEquipItems();
        }
    }

    public int _ExpCurLevel
    {
        get
        {
            return _MHero.expCurLevel;
        }

        set
        {
            _MHero.expCurLevel = value;
            // 当前等级需要经验
            UIManager.Inst.RefreshHeroExp(value, GetNeedExpInCurLevel());
            _MHero.SaveExuCurLevel();
        }
    }

    public float GetExpPercent()
    {
        float percent = 0f;
        percent = _MHero.expCurLevel * 1.0f / GetNeedExpInCurLevel();
        return percent;
    }

    public int GetNeedExpInCurLevel()
    {
        int expToCur = GetNeedEXP(_MHero.level);
        int expNext = GetNeedEXP(_MHero.level + 1);
        int needInCurLevel = expNext - expToCur;
        return needInCurLevel;
    }

    public int _Level
    {
        get
        {
            return _MHero.level;
        }
        set
        {
            _MHero.level = value;
            _MHero.SaveLevel();
        }
    }

    void CreatePlayer() 
    {
       GameObject gobjPlayer = Tools.LoadResourcesGameObject("Prefabs/player");
       gobjPlayer.name = "player";
       
       // 初始化精灵
       Avtoar2D av2d = Tools.GetComponentInChildByPath<Avtoar2D>(gobjPlayer, "model");
       NodeSprite nsBody = new NodeSprite(EEquipPart.BaseBody, "man", Color.white);
       NodeSprite nsHair = new NodeSprite(EEquipPart.Helm, "hair_1", Color.red);
       av2d.SetDicSpriteNode(nsBody);
       av2d.SetDicSpriteNode(nsHair);

       GameManager.hero = gobjPlayer.GetComponent<Hero>();
       //_MHero._CurGridid = gridid;
       //gobjPlayer.transform.position = GetGridPos(gridid);
       //_MHero.SetPlayerDirection(EDirection.Down);
       //cameraControl.target = gobjPlayer;      
       _MHero.Init();
       _MHero.skillIdsTractics = new int[] {3};
       _MHero.InitTacticsSkill();
       _MHero.InitWeapon();
       _Level = _MHero.GetSavedLvel();
       _ExpCurLevel = _MHero.GetSavedExpCurLevel();

        // 初始化任务
       GameManager.commonCPU.ReadAndInitMissionStep();

       _MHero.nickname = "hiJ";
      
       _MHero.InitBaseProp();
       CalHeroPropertyByPropertyAllot();

       //AddAEquipItemToBag(_MHero, GenerateAEquipItem(1, EEquipItemQLevel.Normal));
       //AddAEquipItemToBag(_MHero, GenerateAEquipItem(7, EEquipItemQLevel.Normal));
       //AddAEquipItemToBag(_MHero, GenerateAEquipItem(9, EEquipItemQLevel.Uncommon));
       //AddAEquipItemToBag(_MHero, GenerateAEquipItem(9, EEquipItemQLevel.Magic));
       //AddAEquipItemToBag(_MHero, GenerateAEquipItem(10, EEquipItemQLevel.Normal));
       //AddAEquipItemToBag(_MHero, GenerateAEquipItem(1, EEquipItemQLevel.Legend));
       //EquipAEquipItem(_MHero, GenerateAEquipItem(3, EEquipItemQLevel.Normal));

        // 读取保存的装备
        eiManager.ReadEquipItem();

        // 创建几个物品给英雄
        //AddAEquipItemToBag(Hero.Inst, GenerateAEquipItem(95, EEquipItemQLevel.Normal));

       if (eiManager.itemsHasEquip.Count == 0 && eiManager.itemsInBag.Count == 0)
       {
            // 如果没有装备，则初始装备一个武器
           //EquipItem ei = eiManager.GenerateAEquipItem(9, EEquipItemQLevel.Normal);
           //eiManager.MoveAEquipItemToEquip(ei, EEquipPart.Hand1);
           //UpdateOnChangeEquip(ei, true);

            EquipItem ei = eiManager.GenerateAEquipItem(7, EEquipItemQLevel.Normal);
            eiManager.MoveAEquipItemToEquip(ei, EEquipPart.Pants);
            UpdateOnChangeEquip(ei, true);
        }
       else
       {
            // 根据装备刷新模型
           for (int i = 0; i < eiManager.itemsHasEquip.Count; i++)
           {
               EquipItem ei = eiManager.itemsHasEquip[i];
               UpdateOnChangeEquip(ei, true);
           }
       }
      
       // 装备带来的属性
       CalHeroPropByEquipItem();

       // 基础属性转化为直接属性
       BasePropToDirectProp();

        // 设置状态为100%
        _MHero.Prop.Hp = _MHero.Prop.HpMax;
        _MHero.Prop.Vigor = _MHero.Prop.VigorMax;

       _ProNeedAllot = ReadHeroProNeedAllot();
       eiManager.ReadSavedGold();
       ReadSavedBestTrial();

        //初始化技能
       GameManager.commonCPU.ReadAndInitSkills();
       GameManager.commonCPU.ReadSP();

        // 初始化道具使用配置
       eiManager.ReadAndSetItemUsed();
    }

    internal void SaveKillRecord(string guid)
    {
        string record = PlayerPrefs.GetString(IConst.KEY_KILL_RECORD);
        record = record + guid + "&";
        PlayerPrefs.SetString(IConst.KEY_KILL_RECORD, record);
    }

    /// <summary>
    /// 在属性前，需要设置基础攻速
    /// </summary>
    /// <returns></returns>
    float GetWeaponIAS()
    {
        float ias = 0f;
       EquipItem eiWeapon1 = eiManager.GetEquipItemHasEquip(EEquipPart.Hand1);//主手装备
       EquipItem eiWeapon2 = eiManager.GetEquipItemHasEquip(EEquipPart.Hand2);//副手装备
       if (eiWeapon1 != null)
       {
           ias = eiWeapon1.baseData.ias;
           if (eiWeapon2 != null)
           {
               // 双持，提升15%攻速
               ias *= 1.15f;
           }
       }
       else
       {
           ias = IConst.BaseIAS;
       }
      
       return ias;
    }

    internal void AddAEnermyToBattle(Enermy enermy)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// 获取战斗中所有人
    /// </summary>
    /// <returns></returns>
    public List<IActor> GetAllInBattle() 
    {
        List<IActor> acs = new List<IActor>();
        acs.Add(_MHero);
        for (int i = 0; i < _MHero.GetTargetsInBattle().Count; i++)
        {
            acs.Add(_MHero.GetTargetsInBattle()[i]);
        }
        
        return acs;
    }

    Vector3 GetGridPos(int gridid) 
    {
        Vector3 pos = Vector3.zero;
        GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(mCurGameMap.gameObject, gridid.ToString());
        pos = gobjGrid.transform.position;
        return pos;
    }

    //void OnMove(EDirection dir)
    //{
    //    StartCoroutine(_MHero.CoOnMove(dir));
    //}

    public IEnumerator CoOnEnterBattle(List<Enermy> enermys) 
    {
        _RoundLogicState = GameRoundLogicState.Battle;
        _MHero._State = EActorState.Battle;
        cameraControl.State = ECameraState.Battle;
        _MHero.BsManager.Start();
        yield return 0;

        UIManager.Inst.uiMain.ShowUIBattle(true);

        GameObject gobjBattleOri = GameObject.FindGameObjectWithTag("Battle");

        int enermyCount = enermys.Count;

        Enermy enermyToAttack = null;


        for (int i = 0; i < enermyCount; i++)
        {
            Enermy enermy = enermys[i];
            enermy.uiIndex = i;
        }
        UIManager.Inst.uiMain.InitTargetUI(enermys);

        for (int i = 0; i < enermyCount; i++)
        {
            GameObject gobjPosEnermy = Tools.GetGameObjectInChildByPathSimple(gobjBattleOri, enermyCount + "/enermy" + i);
            Enermy enermy = enermys[i];
            if (enermy != null)
            {
                enermy._State = EActorState.Battle;
                enermy.transform.position = gobjPosEnermy.transform.position;
                enermy.transform.localEulerAngles = gobjPosEnermy.transform.localEulerAngles;
                if (enermy._flagSpeedCtl != null)
                {
                    enermy._flagSpeedCtl.SetVisible(false);
                }
                enermy.OnEnterBattle();


                // 设置UI位置
                GameObject gobjUIEnermyItem = UIManager.Inst.uiMain.dicModelViews[enermy.GetInstanceID()] as GameObject;
                Tools.SetUIPosBy3DGameObj(gobjUIEnermyItem, gobjPosEnermy.transform.position + new Vector3(0f, 0.6f, 0f), cameraMain, cameraUI);

                // 开始攻击第一个
                if (i == 0)
                {
                    //g_Target = enermy;
                    enermyToAttack = enermy;
                }
                enermy.curBattleTarget = _MHero;
                enermy.StartBattleAI();
            }
        }

        if (enermyToAttack != null)
        {
            _MHero.SetAttackTarget(enermyToAttack);
        }
        //_MHero.StartAttack();
    }

    public int GetLostGoldOnDie() 
    {
        int val = _MHero.level * IConst.LOST_GOLD_LEVEL;
        if (val > eiManager._Gold)
        {
            val = eiManager._Gold;
        }
        return val;
    }

    void InitSkillsUI()
    {
        //for (int i = 0; i < _MHero.skills.Length; i++)
        //{
        //    ISkill skill = _MHero.skills[i];
        //    UISprite spSkill = Tools.GetComponentInChildByPath<UISprite>(comCPU.g_GobjUIMain, "battle/grid_spells/spell" + (i + 1));
        //    spSkill.spriteName = skill.GetBaseData().iconName;
        //}
    }

   public void OnMoveEnd()
    {
        EnableInput = true;
    }

    public int CalculateNextGridid(int curid, EDirection dir) 
    {
        int next = -1;
        //switch (dir)
        //{
        //    case EDirection.Up:
        //        next = curid - gGameMapOri.width;
        //        break;
        //    case EDirection.Down:
        //        next = curid + gGameMapOri.width;
        //        if (next > gGameMapOri.width * gGameMapOri.height - 1)
        //        {
        //            next = -1;
        //        }
        //        break;
        //    case EDirection.Left:
        //        if (curid % gGameMapOri.width != 0)
        //        {
        //            next = curid - 1;
        //        }             
        //        break;
        //    case EDirection.Right:
        //        if (!(curid != 0 && curid % (gGameMapOri.width - 1) == 0))
        //        {
        //            next = curid + 1;
        //        }
        //        break;
        //    default:
        //        break;
        //}
        return next;
    }

    /// <summary>
    /// 交互一个npc
    /// </summary>
    /// <param name="npc"></param>
    public void OnTriggerItem_NPC(ItemNPC npc)
    {
        // 显示交互选择界面
        UINPCMutual unm = UIManager.Inst.ShowUINPCMutual();
        unm.Init(npc);
    }

    public void OnTriggerItem_Transfer(ItemTransfer transfer)
    {
        UINPCMutual unm = UIManager.Inst.ShowUINPCMutual();
        unm.Init(transfer);
    }

    private void OnTriggerItem_Door(ItemDoor door)
    {
        UINPCMutual unm = UIManager.Inst.ShowUINPCMutual();
        unm.Init(door);
    }

    /// <summary>
    /// 与一个NPC交谈
    /// </summary>
    /// <param name="npc"></param>
    public void OnTaldToANPC(ItemNPC npc) 
    {
        // 取台词
        string name = npc.npcData.name;
        string words = npc.GetWords(_MHero._CurMainMission.id);
        UIManager.Inst.ShowNPCWords(name, words);
        // 任务检查
        if (_MHero._CurMainMission != null && _MHero._CurMainMission.targetType == EMissionType.InterActive
            && _MHero._CurMainMission.targetId == npc.npcData.id)
        {
            CompleteAMission(_MHero._CurMainMission);
        }
    }
    //==================================================================================================
    #region 回合逻辑
    /// <summary>
    /// 玩家等待一回合
    /// </summary>
    internal void RoundLogicWaitARound()
    {
        if (_RoundLogicState == GameRoundLogicState.Normal)
        {
            PlayerActionEnd();
        }
    }

    public void PlayerActionEnd()
    {
        StartCoroutine(GameView.Inst.RoundLogicPlayerActionEventCo());
    }

    /// <summary>
    /// 玩家行动事件逻辑
    /// </summary>
    /// <returns></returns>
    internal IEnumerator RoundLogicPlayerActionEventCo()
    {
        yield return StartCoroutine(RoundLogicWorldEventCo());
        //玩家回合结束，开始NPC回合
        RoundLogicNPCAction();
    }

    /// <summary>
    /// 单独一个AI动作事件逻辑。AI行动后触发事件
    /// </summary>
    /// <returns></returns>
    internal IEnumerator CoAIActEvent(Enermy e)
    {
        WorldEventForANPC(e);
        yield return 0;
        //yield return StartCoroutine(RoundLogicWorldEventCo());
    }

    /// <summary>
    /// 当玩家回合结束。回合结束事件
    /// </summary>
    internal void OnHeroRoundEnd()
    {
        GameManager.gameView._RoundLogicState = GameRoundLogicState.WorldEventAction;
        //TODO 玩家回合结束事件
        RoundLogicNPCAction();
    }

    /// <summary>
    /// 玩家回合开始逻辑：Buff，技能触发
    /// </summary>
    private void RoundLogicPlayerRoundStart()
    {
        gRoundCount++;
        _RoundLogicState = GameRoundLogicState.PlayerRoundStart;
        
        //TODO buff，技能触发
        RoundLogicPlayerInput();
    }
    
    public void RoundLogicOnInitMap()
    {
        gRoundCount = 0;
        //草丛逻辑
        for (int i = 0; i < mListEnermys.Count; i++)
        {
            Enermy e = mListEnermys[i];
            MapGrid mgE = e.GetCurMapGrid();
            if (mgE.Surface == EMGSurface.Grass)
            {
                Buff_Hiding buffHiding = Tools.GetOrAddCom<Buff_Hiding>(e.gameObject);
                buffHiding.Init(e);
                buffHiding.StartEffect();
            }
        }

        RoundLogicPlayerRoundStart();
    }

    /// <summary>
    /// 玩家输入逻辑
    /// </summary>
    private void RoundLogicPlayerInput()
    {
        _RoundLogicState = GameRoundLogicState.Normal;
        //切换地图判断
        Hero.Inst.StandOnAGridHandler();
    }

    /// <summary>
    /// 事件流程逻辑
    /// </summary>
    internal IEnumerator RoundLogicWorldEventCo()
    {
        _RoundLogicState = GameRoundLogicState.WorldEventAction;

        for (int mgIndex = 0; mgIndex < mListMGs.Count; mgIndex++)
        {
            MapGrid mg = mListMGs[mgIndex];
            //火势蔓延
            //非该回合燃烧
            if (mg.Surface == EMGSurface.Fireing && mg.burnRoundIndex != gRoundCount)
            {
                //周围格子着火
                List<MapGrid> mgsNear = mg.GetNearGrids();
                for (int indexMgNear = 0; indexMgNear < mgsNear.Count; indexMgNear++)
                {
                    MapGrid mgNear = mgsNear[indexMgNear];
                    if (mgNear != null && mgNear.CanBurn())
                    {
                        mgNear.burnRoundIndex = gRoundCount;
                        mgNear.Burn();
                    }
                }
            }
        }

        //TODO 检测每块地形，地图道具，执行逻辑
        for (int i = 0; i < mListEnermys.Count; i++)
        {
            Enermy e = mListEnermys[i];
            WorldEventForANPC(e);
        }

        //玩家在火中受伤
        if (Hero.Inst.GetCurMapGrid().Surface == EMGSurface.Fireing)
        {
            DamageTarget(Hero.Inst, 100, EDamageType.Fire);
        }

        //逻辑结束
        yield return 0;
    }

    void WorldEventForANPC(Enermy e)
    {
        MapGrid mgE = e.GetCurMapGrid();

        //在草丛内隐匿
        if (mgE.Surface == EMGSurface.Grass)
        {
            Buff_Hiding buffHiding = Tools.GetOrAddCom<Buff_Hiding>(e.gameObject);
            buffHiding.Init(e);
            buffHiding.StartEffect();
        }
        else
        {
            Buff_Hiding buffHidin = e.GetBuff<Buff_Hiding>();
            if (buffHidin != null)
            {
                buffHidin.Remove();
            }
        }

        //火中受伤害
        if (mgE.Surface == EMGSurface.Fireing)
        {
            DamageTarget(e, 100, EDamageType.Fire);
        }
    }

    /// <summary>
    /// 对目标造成伤害。伤害源是世界事件
    /// </summary>
    /// <param name="e"></param>
    private void DamageTarget(IActor target, int damage, EDamageType damageType)
    {
        //坚韧
        damage = Mathf.CeilToInt(damage * (1 - target.Prop.DamReduce));

        target.OnHurted(damage, damageType, null, false);

        int oriTargetHP = target.Prop.Hp;

        target.Prop.Hp -= damage;
        if (target.Prop.Hp <= 0)
        {
            target.Prop.Hp = 0;
            target._State = EActorState.Dead;
            target.OnDead();
            if (target.isHero)
            {
                GameManager.gameView.OnHeroDie();
            }
            else
            {
                GameManager.gameView.OnEnermyDie(target as Enermy);
                
            }
        }
        else
        {
            target.OnHPChange(oriTargetHP, target.Prop.Hp);
        }

        if (!target.isHero)
        {
            //UIManager.Inst.uiMain.RefreshTargetHP(target as Enermy);
            //UIManager.Inst.ShowDamageTxt(damage, damageType);
        }
        else
        {
            UIManager.Inst.uiMain.RefreshHeroHP();
            UIManager.Inst.ShowHurtedTxt(damage, damageType);
        }
    }

    /// <summary>
    /// 开始NPC行为逻辑
    /// </summary>
    private void RoundLogicNPCAction()
    {
        _RoundLogicState = GameRoundLogicState.NPCAction;
        for (int i = 0; i < mListEnermys.Count; i++)
        {
            Enermy e = mListEnermys[i];
            if (e != null && e._State != EActorState.Dead)
            {
                e.enableAction = true;
            }
        }
        StartCoroutine(CoRoundLoginNPCActions());
    }

    private IEnumerator CoRoundLoginNPCActions()
    {
        // 视野检测

        for (int i = 0; i < mListEnermys.Count; i++)
        {
            Enermy e = mListEnermys[i];
            if (e != null && e._State != EActorState.Dead)
            {
                yield return StartCoroutine(e.CoAIAction());
                yield return StartCoroutine(CoAIActEvent(e));
            }
        }

        //怪物回合结束
        StartRoundLogicBattle();
    }

    /// <summary>
    /// 开始战斗逻辑
    /// </summary>
    internal void StartRoundLogicBattle()
    {
        Hero.Inst.ClearBattleTargets();

        List<MapGrid> mgsNearHero = Hero.Inst.GetCurMapGrid().GetNearGrids(true);
        for (int i = 0; i < mgsNearHero.Count; i++)
        {
            MapGrid mgNearHero = mgsNearHero[i];
            Enermy e = mgNearHero.GetItem<Enermy>();
            if (e != null && (e._AIState == EAIState.Battle || e._AIState == EAIState.FindTarget))
            {
                MapGrid mgEnermy = e.GetCurMapGrid();
                int dis = MapGrid.GetDis(Hero.Inst.GetCurMapGrid(), mgEnermy);
                if (dis == 2)
                {
                    //隐匿状态不被动加入战斗
                    if (!e.HasBuff<Buff_Hiding>())
                    {
                        List<MapGrid> mgNearTemp = mgEnermy.GetNearGrids();
                        for (int j = 0; j < mgNearTemp.Count; j++)
                        {
                            MapGrid mgTemp = mgNearTemp[j];
                            Enermy eTemp = mgTemp.GetItem<Enermy>();
                            if (eTemp != null && eTemp._AIState == EAIState.Battle)
                            {
                                Hero.Inst.AddAEnermyToBattle(e);
                            }
                        }
                    }
                }
                else
                {
                    Hero.Inst.AddAEnermyToBattle(e);
                }
                
            }
        }

        if (Hero.Inst.HasEnermyToBattle())
        {
            StartCoroutine(CoOnEnterBattle(Hero.Inst.GetBattleTargets()));
        }
        else
        {
            RoundLogicPlayerRoundStart();
        }
    }
    #endregion

    #region 当激活一个npc
    #endregion
    /// <summary>
    /// 当激活一个npc（神坛）
    /// </summary>
    /// <param name="npc"></param>
    public void OnActiveANPC(ItemNPC npc) 
    {
		if (npc.npcData.type ==  ENPCType.Altar) 
		{
			AltarBase altar = npc.gameObject.GetComponent<AltarBase>();

			if (altar == null)
			{
					if (npc.npcData.subType == (int)EAltarType.Recover)
					{
							// 恢复神坛
							altar = npc.gameObject.AddComponent<AltarRecover>();
					}
					else if (npc.npcData.subType == (int)EAltarType.Fury)
					{
							// 狂怒神坛
							altar = npc.gameObject.AddComponent<AltarFury>();
					}
					else if (npc.npcData.subType == (int)EAltarType.Def) 
					{
							// 防御圣殿
							altar = npc.gameObject.AddComponent<AltarDef>();
					}
					else if (npc.npcData.subType == (int)EAltarType.Fight)
					{
							// 战斗神殿
							altar = npc.gameObject.AddComponent<AltarFight>();
					}
					altar.Init(npc);
			}

			altar.OnActive();
		}
       
    }

	
    /// <summary>
    /// 触发宝箱
    /// </summary>
    /// <param name="itc"></param>
    public void OnTriggerItem_TreasureChest(ItemTreasureChest itc) 
    {
        if (!itc.hasInitItems)
        {
            itc.InitItems();
        }
        // 显示界面
        UIManager.Inst.ShowUIItemDrops(itc.listEquipItems);

        //保存记录
        //地图上的宝箱
        if (!string.IsNullOrEmpty(itc.guid))
        {
            SaveRecordChest(itc);
        }
    }

    private void SaveRecordChest(ItemTreasureChest itc)
    {
        string record = PlayerPrefs.GetString(IConst.KEY_CHEST_OPENED);
        record = record + itc.guid + "&";
        PlayerPrefs.SetString(IConst.KEY_CHEST_OPENED, record);
    }

    public MapGrid GetMapGridById(int id)
    {
        MapGrid mg = null;
        mg = Tools.GetComponentInChildByPath<MapGrid>(mCurGameMap.gameObject, id.ToString());
        return mg;
    }

    public MapGrid GetMapGridByXY(int x, int y)
    {
        int id = y * mCurGameMap.baseData.width + x;
        return GetMapGridById(id);
    }

    /// <summary>
    /// 寻找玩家初始位置格子
    /// </summary>
    /// <returns></returns>
    public MapGrid FinePlayerStartGrid()
    {
        MapGrid mg = null;
        foreach (Transform child in mCurGameMap.transform)
        {
            MapGrid mgTemp = child.GetComponent<MapGrid>();
            if (mgTemp != null)
            {
                if (mgTemp.Type == EGridType.Start)
                {
                    mg = mgTemp;
                    break;
                }
            }
        }
        return mg;
    }

    public MapGrid FindStartAndToHomeGrid() 
    {
        MapGrid mg = null;
        foreach (Transform child in mCurGameMap.transform)
        {
            MapGrid mgTemp = child.GetComponent<MapGrid>();
            if (mgTemp != null)
            {
                if (mgTemp.Type == EGridType.StartAndToHome)
                {
                    mg = mgTemp;
                    break;
                }
            }
        }
        return mg;
    }

    /// <summary>
    /// 开启视野
    /// </summary>
    public void OpenShadow()
    {
        int area = 3;
        List<int> listY = new List<int>();
        int idTemp = _MHero._CurGridid;
        listY.Add(idTemp);
        for (int i = 1; i <= area / 2; i++)
        {
            idTemp = CalculateNextGridid(idTemp, EDirection.Up);
            if (idTemp > 0)
            {
                listY.Add(idTemp);
            }
        }

        idTemp = _MHero._CurGridid;
        for (int i = 1; i <= area / 2; i++)
        {
            idTemp = CalculateNextGridid(idTemp, EDirection.Down);
            if (idTemp > 0)
            {
                listY.Add(idTemp);
            }
        }

        foreach (int idY in listY)
        {
            int idTempX = idY;
            GameObject gobjGrid = Tools.GetGameObjectInChildByPathSimple(mCurGameMap.gameObject, idTempX.ToString());
            if (gobjGrid != null)
            {
                gobjGrid.SetActive(true);

            }    
            for (int i = 1; i <= area / 2; i++)
            {
                idTempX = CalculateNextGridid(idTempX, EDirection.Right);
                if (idTempX > 0)
                {
                    gobjGrid = Tools.GetGameObjectInChildByPathSimple(mCurGameMap.gameObject, idTempX.ToString());
                    if (gobjGrid != null)
                    {
                        gobjGrid.SetActive(true);

                    }    
                }
            }
            idTempX = idY;
            for (int i = 1; i <= area / 2; i++)
            {
                idTempX = CalculateNextGridid(idTempX, EDirection.Left);
                if (idTempX > 0)
                {
                    gobjGrid = Tools.GetGameObjectInChildByPathSimple(mCurGameMap.gameObject, idTempX.ToString());
                    if (gobjGrid != null)
                    {
                        gobjGrid.SetActive(true);

                    }
                }
            }
        }
    }

    public void OnKillCurTarget()
    {
        
        //OnExitBattle();
        // 自动切换到下一个目标
        Enermy nextTarget = _MHero.GetNextTarget();
        if (nextTarget != null)
        {
            _MHero.SetAttackTarget(nextTarget);
            UIManager.Inst.uiMain.SetTarget(nextTarget);
        }
        else
        {
            // 无目标，退出战斗
            StartCoroutine(ExitBattleDelay(1.2f));
        }
    }

    /// <summary>
    /// 延迟退出战斗
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ExitBattleDelay(float delay) 
    {
        yield return new WaitForSeconds(delay);
        OnExitBattle();
    }

    /// <summary>
    /// 离开战斗
    /// </summary>
    public void OnExitBattle()
    {
        _RoundLogicState = GameRoundLogicState.Normal;
        if (_MHero._State != EActorState.Dead)
        {
            _MHero._State = EActorState.Normal;
            _MHero.Prop.Vigor = _MHero.Prop.VigorMax;
            UIManager.Inst.uiMain.RefreshHeroVigor();
            _MHero.transform.position = _MHero.GetCurMapGrid().transform.position;
            _MHero.StandOnAGridHandler();
            OnRoundEnd();
        }

        _MHero.BsManager.Clear();

        UIManager.Inst.uiMain.ShowUIBattle(false);
        UIManager.Inst.uiMain.ShowTargetUI(false);
        cameraControl.State = ECameraState.Normal;

        if (hasKillCurBoss)
        {
            // 击杀试炼塔boss
            OnFinishATrial();
            UIManager.Inst.GeneralTip("挑战完成", Color.yellow);
            hasKillCurBoss = false;
        }
        else if (mCurGameMap.baseData.tier > 0 && GetEnermyCountInCurMap() == 0)
        {
            OnKillAllEnemy();
        }


    }

    /// <summary>
    /// 当击杀所有小怪
    /// </summary>
    void OnKillAllEnemy() 
    {
        UIManager.Inst.GeneralTip("邪恶凝聚，一个强大的怪物出现了", Color.red);
        MapGrid mg = FindStartAndToHomeGrid();
        MonsterBaseData mon = GameDatas.GetMonsterBaseData(mCurGameMap.baseData.bossId);
        Enermy enermyBoss = CreateAEnermy(mg, mon);
        enermyBoss.isTierBoss = true;
    }

    public void OnRoundEnd() 
    {
        _CurRound++;
    }

    /// <summary>
    /// 试炼塔完成挑战
    /// </summary>
    void OnFinishATrial() 
    {
        int curTier = mCurGameMap.baseData.tier;
        _MHero._BestTrial = curTier;
    }

    public void OnComfirmDie() 
    {
        UIManager.Inst.RemoveHeroDieUI();

        // 退出战斗
        if (_RoundLogicState == GameRoundLogicState.Battle)
        {
            OnExitBattle();
        }
        // 清除警觉
        _MHero.ClearAlterness();
        // 复活
        _MHero._State = EActorState.Normal;
        // 回到城镇
        PlayerToMap(GameDatas.GetGameMapBD(GameManager.commonCPU.ReadCurHomeMap()), -1);
    }

    /// <summary>
    /// 当玩家死亡
    /// </summary>
    public void OnHeroDie()
    {
        UIManager.Inst.ShowHeroDieUI();
    }

    /// <summary>
    /// 当敌人死亡
    /// </summary>
    public void OnEnermyDie(Enermy enermy)
    {
        // 掉落装备
        List<EquipItem> items = GetMonsterDropList(enermy.GetDrops(), enermy);
        if (items.Count > 0)
        {
            CreateATreasureChest(GetMapGridOfItem(enermy.gameObject), items);
        }
        // 获取经验
        //int exp = GetMonEXP(_MHero.level, enermy.level);
       // PlayerGetExp(exp);

        // 任务检测
        if (_MHero._CurMainMission != null && _MHero._CurMainMission.targetType == EMissionType.Kill 
            && _MHero._CurMainMission.targetId == enermy._MonsterBD.id)
        {
            CompleteAMission(_MHero._CurMainMission);
        }

        if (enermy.isTierBoss)
        {
            hasKillCurBoss = true;
        }

        GameManager.commonCPU.ShankASprite(enermy.GetSpriteRender());
        GObjLife gl = enermy.gameObject.AddComponent<GObjLife>();
        gl.lifeTime = 1f;
    }

    /// <summary>
    /// 玩家获得经验
    /// </summary>
    /// <param name="exp"></param>
    public void PlayerGetExp(int exp)
    {
        // 到下一等级需要经验
        int expNeed = GetNeedExpInCurLevel();
        int difVal = expNeed - _ExpCurLevel; // 差值
        if (difVal <= exp)
        {
            // 升级
            _Level++;
            _ExpCurLevel = exp - difVal;
            OnLevelUP();
        }else
        {
            _ExpCurLevel += exp;
        }
        
    }

    /// <summary>
    /// 升级
    /// </summary>
    public void OnLevelUP()
    {
        UIManager.Inst.GeneralTip("等级提升！", new Color32(255, 140, 0, 255));
        // 恢复至百分百状态
        _MHero.Prop.Hp = _MHero.Prop.HpMax;
        //_MHero._Mp = _MHero.mpMax;
        UIManager.Inst.RefreshMainUIHeroStateInfo();
        // 获得5属性点
        _ProNeedAllot += 5;
        // 获得一点技能点
        _MHero._SkillNeedAllot++;
    }

    public MapGrid GetMapGridOfItem(GameObject gobjItem) 
    {
        return gobjItem.transform.parent.GetComponent<MapGrid>();
    }

    public void UIShowDamage(int damage) { }

    public void UIShowHurt(int hurt)
    {

    }

    public void UIShowHeal(int heal) 
    {
    }

    public void UIAddBuff(IBaseBuff buff) { }

    public void UIRemoveBuff(IBaseBuff buff) { }



    void ReadSavedBestTrial() 
    {
        if (PlayerPrefs.HasKey(IConst.KEY_BEST_TRIAL))
        {
            _MHero._BestTrial = PlayerPrefs.GetInt(IConst.KEY_BEST_TRIAL);
        }
        else
        {
            _MHero._BestTrial = 0;
        }
    }

    #region 属性管理
    /// <summary>
    /// 读取本地保存的已分配到力量的属性点
    /// </summary>
    int ReadHeroPropertyToStr()
    {
        int r = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_HASALLOT_STR))
        {
            r = PlayerPrefs.GetInt(IConst.KEY_HASALLOT_STR);
        }
        return r;
    }

    /// <summary>
    /// 本地保存的已分配到力量的属性点添加val点
    /// </summary>
    /// <param name="val"></param>
    void AddSavedPropToStr(int val)
    {

    }

    int ReadHeroPropertyToAgi()
    {
        int r = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_HASALLOT_AGI))
        {
            r = PlayerPrefs.GetInt(IConst.KEY_HASALLOT_AGI);
        }
        return r;
    }

    int ReadHeroPropertyToInt()
    {
        int r = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_HASALLOT_INT))
        {
            r = PlayerPrefs.GetInt(IConst.KEY_HASALLOT_INT);
        }
        return r;
    }

    int ReadHeroPropertyToSta()
    {
        int r = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_HASALLOT_STA))
        {
            r = PlayerPrefs.GetInt(IConst.KEY_HASALLOT_STA);
        }
        return r;
    }

    /// <summary>
    /// 读取未分配属性点
    /// </summary>
    /// <returns></returns>
    int ReadHeroProNeedAllot()
    {
        int r = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_NEED_ALLOT))
        {
            r = PlayerPrefs.GetInt(IConst.KEY_NEED_ALLOT);
        }
        return r;
    }

    int g_propNeedAllot = 0;

    private int gRoundCount;//当前地图回合计数

    /// <summary>
    ///  未分配属性点
    /// </summary>
    public int _ProNeedAllot
    {
        get
        {
            return g_propNeedAllot;
        }
        set
        {
            g_propNeedAllot = value;
            PlayerPrefs.SetInt(IConst.KEY_NEED_ALLOT, value);
        }
    }

    public EPlayInputState InputState
    {
        get
        {
            return gInputState;
        }

        set
        {
            gInputState = value;
            if (gInputState == EPlayInputState.ChooseTarget)
            {
                if (gItemToUse != null)
                {
                    //显示UI
                    UIManager.Inst.GeneralTip("选择目标", Color.yellow);
                    UIManager.Inst.uiMain.gUIChooseTarget.SetVisible(true);
                    //显示可选择的格子
                    string data = gItemToUse.baseData.data;
                    JSONNode jdData = JSONNode.Parse(data);
                    int range = jdData["range"].AsInt;
                    List<MapGrid> mgs = Hero.Inst.GetCurMapGrid().GetMGsInRange(range);
                    foreach (MapGrid itemMG in mgs)
                    {
                        itemMG.ChooseState = EChoosedState.Choosable;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 计算英雄属性: 属性点分配
    /// </summary>
    /// 
    void CalHeroPropertyByPropertyAllot()
    {
        // 属性点带来的基础属性
        int strAllot = _PropHasAlltoToStr;
        int agiAllot =_PropHasAlltoToAgi;
        int intAllot = _PropHasAllotToTen;
        int staAllot = _PropHasAllotToSta;
        int endAllot = _PropHasAllotToEnd;


        _MHero.Prop.Strength += strAllot;
        _MHero.Prop.Agility += agiAllot;
        _MHero.Prop.Tenacity += intAllot;
        _MHero.Prop.Stamina += staAllot;
        _MHero.Prop.Endurance += endAllot;
    }

    /// <summary>
    /// 计算装备带来的属性
    /// </summary>
    void CalHeroPropByEquipItem()
    {
        for (int i = 0; i < eiManager.itemsHasEquip.Count; i++)
        {
            EquipItem eiHasEque = eiManager.itemsHasEquip[i];
            AddEuqipItemPrpToHero(eiHasEque);
        }
    }

    /// <summary>
    /// 将装备属性添加到玩家身上
    /// </summary>
    /// <param name="eiHasEque"></param>
   public void AddEuqipItemPrpToHero(EquipItem eiHasEque)
    {
        // 基础装备属性，如护甲，攻击力
        _MHero.Prop.arm += eiHasEque.baseData.arm;

        if (eiHasEque.baseData.type == EEquipItemType.WeaponOneHand)
        {
            if (eiHasEque._Part == EEquipPart.Hand1)
            {
                Hero.Inst.Prop.AtkBaseA = eiHasEque.baseData.atk;
            }
            else if (eiHasEque._Part == EEquipPart.Hand2)
            {
                Hero.Inst.Prop.AtkBaseB = eiHasEque.baseData.atk;
            }
        }
        else if (eiHasEque.baseData.type == EEquipItemType.WeaponTwoHand)
        {
            Hero.Inst.Prop.AtkBaseA = eiHasEque.baseData.atk;
            Hero.Inst.Prop.AtkBaseB = 0;
        }

        if (eiHasEque.baseData.type == EEquipItemType.WeaponOneHand || eiHasEque.baseData.type == EEquipItemType.WeaponTwoHand)
        {
            // 武器速度计算
            _MHero.Prop.BaseWeaponIAS = GetWeaponIAS();
        }

        //重量
        Hero.Inst.Prop.LoadBase += eiHasEque.baseData.weight;
        // 移动速度
        Hero.Inst.Prop.MoveSpeedBase += eiHasEque.baseData.movespeed;
        //// 基础格挡率
        //Hero.Inst.Prop.parryDamPerBase += (eiHasEque.baseData.parry * 0.01f);
        ////基础格挡值
        //Hero.Inst.Prop.parryDmgVigorBase += eiHasEque.baseData.parryVigor;
        // 魔法属性
        for (int wordIndex = 0; wordIndex < eiHasEque.words.Count; wordIndex++)
        {
            EquipItemWord eiw = eiHasEque.words[wordIndex];
            EEquipItemProperty propType = eiw.wordBaseData.propertyType;
            switch (propType)
            {
                case EEquipItemProperty.Str:
                    {
                        _MHero.Prop.Strength += eiw.val;
                    }
                    
                    break;
                case EEquipItemProperty.Agi:
                    {
                        _MHero.Prop.Agility += eiw.val;
                    }
                    break;
                case EEquipItemProperty.Ten:
                    {
                        _MHero.Prop.Tenacity += eiw.val;
                    }
                    break;
                case EEquipItemProperty.Sta:
                    {
                        _MHero.Prop.Stamina += eiw.val;
                    }
                    break;
                case EEquipItemProperty.Arm:
                    _MHero.Prop.arm += eiw.val;
                    break;
                case EEquipItemProperty.IAS:
                    _MHero.Prop.BaseWeaponIAS *= (1 + eiw.val / 100f);
                    break;
                case EEquipItemProperty.ResFire:
                    _MHero.Prop.resFire += eiw.val;
                    break;
                case EEquipItemProperty.ResThunder:
                    _MHero.Prop.resThunder += eiw.val;
                    break;
                case EEquipItemProperty.ResPoison:
                    _MHero.Prop.resPoision += eiw.val;
                    break;
                case EEquipItemProperty.ResFrozen:
                    _MHero.Prop.resForzen += eiw.val;
                    break;
                case EEquipItemProperty.CriticalStrike:
                    _MHero.Prop.DeadlyStrike *= (1 + eiw.val / 100f);
                    break;
                case EEquipItemProperty.ParryDamage:
                    _MHero.Prop.parryDmbPerParamB *= (1 + eiw.val * 0.01f);
                    break;
                case EEquipItemProperty.FireDamage:
                    _MHero.Prop.atkFireParamAdd += eiw.val;
                    break;
                case EEquipItemProperty.ThunderDamage:
                    _MHero.Prop.atkThunderParamAdd += eiw.val;
                    break;
                case EEquipItemProperty.PoisonDamage:
                    _MHero.Prop.atkPoisonParamAdd += eiw.val;
                    break;
                case EEquipItemProperty.ForzenDamage:
                    _MHero.Prop.atkIceParamAdd += eiw.val;
                    break;
                case EEquipItemProperty.AddDamagePercent:
                    Hero.Inst.Prop.AtkParmaA += eiw.val;
                    break;
                case EEquipItemProperty.Weight:
                    Hero.Inst.Prop.LoadBase += eiw.val;
                    break;
                case EEquipItemProperty.MoveSpeed:
                    Hero.Inst.Prop.MoveSpeedBase += eiw.val;
                    break;
                case EEquipItemProperty.AddDamage:
                    Hero.Inst.Prop.AtkParmaB += eiw.val;
                    break;
                case EEquipItemProperty.ArmPercent:
                    Hero.Inst.Prop.DefIncrease(1 + eiw.val * 0.01f);
                    break;
                case EEquipItemProperty.PowerSpeed:
                    Hero.Inst.Prop.PowerSpeedIncrease(1 + eiw.val * 0.01f);
                    break;
                default:
                    break;
            }
        }
        UIManager.Inst.RefreshHeroInfo();
        UIManager.Inst.RefreshMainUIHeroStateInfo();
    }

    /// <summary>
    /// 移除一件装备在玩家上生效的属性
    /// </summary>
    /// <param name="eiHasEque"></param>
    public void RemoveEquipItemPropFromHero(EquipItem eiHasEque)
    {
        // 基础装备属性，如护甲，攻击力
        _MHero.Prop.arm -= eiHasEque.baseData.arm;

        if (eiHasEque.baseData.type == EEquipItemType.WeaponOneHand)
        {
            if (eiHasEque._Part == EEquipPart.Hand1)
            {
                Hero.Inst.Prop.AtkBaseA = 0;
            }
            else if (eiHasEque._Part == EEquipPart.Hand2)
            {
                Hero.Inst.Prop.AtkBaseB = 0;
            }
        }
        else if (eiHasEque.baseData.type == EEquipItemType.WeaponTwoHand)
        {
            Hero.Inst.Prop.AtkBaseA = 0;
            Hero.Inst.Prop.AtkBaseB = 0;
        }

        if (eiHasEque.baseData.type == EEquipItemType.WeaponOneHand || eiHasEque.baseData.type == EEquipItemType.WeaponTwoHand)
        {
            // 武器速度计算
            _MHero.Prop.BaseWeaponIAS = GetWeaponIAS();
        }

        //重量
        Hero.Inst.Prop.LoadBase -= eiHasEque.baseData.weight;
        // 移动速度
        Hero.Inst.Prop.MoveSpeedBase -= eiHasEque.baseData.movespeed;
        //// 基础格挡率
        //Hero.Inst.Prop.parryDamPerBase -= (eiHasEque.baseData.parry * 0.01f);
        //// 基础格挡值
        //Hero.Inst.Prop.parryDmgVigorBase -= eiHasEque.baseData.parryVigor;
        // 魔法属性
        for (int wordIndex = 0; wordIndex < eiHasEque.words.Count; wordIndex++)
        {
            EquipItemWord eiw = eiHasEque.words[wordIndex];
            EEquipItemProperty propType = eiw.wordBaseData.propertyType;
            switch (propType)
            {
                case EEquipItemProperty.Str:
                    {
                        _MHero.Prop.Strength -= eiw.val;
                    }

                    break;
                case EEquipItemProperty.Agi:
                    {
                        _MHero.Prop.Agility -= eiw.val;
                    }
                    break;
                case EEquipItemProperty.Ten:
                    {
                        _MHero.Prop.Tenacity -= eiw.val;
                    }

                    break;
                case EEquipItemProperty.Sta:
                    {
                        _MHero.Prop.Stamina -= eiw.val;
                    }
                    break;
                case EEquipItemProperty.Arm:
                    _MHero.Prop.arm -= eiw.val;
                    break;
                case EEquipItemProperty.IAS:
                    _MHero.Prop.BaseWeaponIAS /= (1 + eiw.val / 100f);
                    break;
                case EEquipItemProperty.ResFire:
                    _MHero.Prop.resFire -= eiw.val;
                    break;
                case EEquipItemProperty.ResThunder:
                    _MHero.Prop.resThunder -= eiw.val;
                    break;
                case EEquipItemProperty.ResPoison:
                    _MHero.Prop.resPoision -= eiw.val;
                    break;
                case EEquipItemProperty.ResFrozen:
                    _MHero.Prop.resForzen -= eiw.val;
                    break;
                case EEquipItemProperty.CriticalStrike:
                    _MHero.Prop.DeadlyStrike /= (1 + eiw.val / 100f);
                    break;
                case EEquipItemProperty.ParryDamage:
                    _MHero.Prop.parryDmbPerParamB /= (1 + eiw.val * 0.01f);
                    break;
                case EEquipItemProperty.FireDamage:
                    _MHero.Prop.atkFireParamAdd -= eiw.val;
                    break;
                case EEquipItemProperty.ThunderDamage:
                    _MHero.Prop.atkThunderParamAdd -= eiw.val;
                    break;
                case EEquipItemProperty.PoisonDamage:
                    _MHero.Prop.atkPoisonParamAdd -= eiw.val;
                    break;
                case EEquipItemProperty.ForzenDamage:
                    _MHero.Prop.atkIceParamAdd -= eiw.val;
                    break;
                case EEquipItemProperty.AddDamagePercent:
                    Hero.Inst.Prop.AtkParmaA -= eiw.val;
                    break;
                case EEquipItemProperty.Weight:
                    Hero.Inst.Prop.LoadBase -= eiw.val;
                    break;
                case EEquipItemProperty.MoveSpeed:
                    Hero.Inst.Prop.MoveSpeedBase -= eiw.val;
                    break;
                case EEquipItemProperty.AddDamage:
                    Hero.Inst.Prop.AtkParmaB -= eiw.val;
                    break;
                case EEquipItemProperty.ArmPercent:
                    Hero.Inst.Prop.DefIncrease(1 / (1 + eiw.val * 0.01f));
                    break;
                case EEquipItemProperty.PowerSpeed:
                    Hero.Inst.Prop.PowerSpeedIncrease(1 / (1 + eiw.val * 0.01f));
                    break;
                default:
                    break;
            }
        }
        UIManager.Inst.RefreshHeroInfo();
        UIManager.Inst.RefreshMainUIHeroStateInfo();
    }

    /// <summary>
    /// 将玩家的基础属性转化成直接属性
    /// </summary>
    void BasePropToDirectProp()
    {
        //StaToDirectProp(_MHero._PropertyHandler._Stamina, true);
        //AgiToDirectProp(_MHero._Agility, true);
        //IntToDirectProp(_MHero._Prop._Tenacity, true);
    }

    /// <summary>
    /// 敏捷转化为玩家的直接属性
    /// </summary>
    /// <param name="agi"></param>
    [Obsolete()]
    public void AgiToDirectProp(int agi, bool add)
    {
        return;
        int hitVal = agi * IConst.HIT_PER_AGI;
        int dodgeVal = agi * IConst.DODGE_PER_AGI;
        float iaspersent = agi * IConst.IAS_PERCENT_PER_AGI;
        if (add)
        {
            //_MHero.moveSpeed += dodgeVal;
            //_MHero._IAS *= (1 + iaspersent / 100f);
        }
        else
        {
            //_MHero.moveSpeed -= dodgeVal;
            //_MHero._IAS /= (1 + iaspersent / 100f);
        }
    }

    [Obsolete()]
    public void IntToDirectProp(int intVal, bool add)
    {
        return;
        int atkMagVal = intVal * IConst.ATK_MAG_PER_INT;
        int mp = intVal * IConst.MP_PER_INT;
        if (add)
        {
            //_MHero.mpMax += mp;
            _MHero.atkMag += atkMagVal;
        }
        else
        {
            //_MHero.mpMax -= mp;
            _MHero.atkMag -= atkMagVal;
        }
       
    }

    //public void StaToDirectProp(int sta, bool add) 
    //{
    //    int hp = sta * IConst.HP_PER_STA;
    //    int tl = sta * IConst.TL_PER_STA;

    //    if (add)
    //    {
    //        _MHero._PropertyHandler._HpMax += hp;
    //        _MHero.tlMax += tl;
    //    }
    //    else
    //    {
    //        _MHero._PropertyHandler._HpMax -= hp;
    //        _MHero.tlMax -= tl;
    //    }
        
    //}
    #endregion
    #region 宝箱
    public void OnCloseUIDrops(UIDropItems udi)
    {
        MonoKit.DestroyObject(udi.gameObject);
        // 移除交互格子宝箱
        mgInterActive.SetToNone();
    }

    //给定指定掉落列表创建宝箱
    public ItemTreasureChest CreateATreasureChest(MapGrid mg, List<EquipItem> eis) 
    {
        GameObject gobjChest = Tools.LoadResourcesGameObject(IPath.Items + "bagkodohorns");
        gobjChest.transform.parent = mg.gameObject.transform;
        gobjChest.transform.localPosition = Vector3.zero;
        ItemTreasureChest itc = gobjChest.AddComponent<ItemTreasureChest>();
        itc.listEquipItems.AddRange(eis);
        itc.hasInitItems = true;
        return itc;
    }

    /// <summary>
    /// 根据掉落信息获取掉落列表:
    /// 百分比_基础id_最小个数_最大个数:baseid&百分比_id:legendid&百分比_tlevel_最小个数_最大个数_稀有几率百分比_魔法几率百分比:tlevel;
    /// </summary>
    /// <param name="enermy"></param>
    /// <returns></returns>
    public List<EquipItem> GetMonsterDropList(string drops, Enermy enermy = null) 
    {
        if (string.IsNullOrEmpty(drops))
        {
            Debug.LogError("错误的掉落信息");
            return null;
        }
        List<EquipItem> eis = new List<EquipItem>();
        string[] dropNodes = drops.Split('&');
        // 对每个节点进行掉落分析
        for (int i = 0; i < dropNodes.Length; i++)
        {
            string dropNode = dropNodes[i];
            dropNode = dropNode.Replace('\n',' ');
            dropNode = dropNode.Trim();
            string[] dropsTemp = dropNode.Split(':');
            string strData = dropsTemp[0];
            string strDropType = dropsTemp[1];
            if (strDropType.Equals("baseid"))
            {
                // 指定物品id
                string[] strPramsDropData = strData.Split('_');
                int oddsPersent = int.Parse(strPramsDropData[0]);
                int id = int.Parse(strPramsDropData[1]);
                int minCount = int.Parse(strPramsDropData[2]);
                int maxCount = int.Parse(strPramsDropData[3]);
                if (Tools.IsHitOdds(oddsPersent))
                {
                    int count = UnityEngine.Random.RandomRange(minCount, maxCount);
                    EquipItem ei = eiManager.GenerateAEquipItem(id, EEquipItemQLevel.Normal);

                    // 金钱掉落倍数修正
                    if (enermy != null && ei.baseData.type == EEquipItemType.Gold)
                    {
                        count = Mathf.FloorToInt(count * enermy.dropCashOffet);
                    }

                    ei.count = count;
                    eis.Add(ei);
                }
            }
            else if (strDropType.Equals("legendid"))
            {
                // 指定传奇id
                string[] strPramsDropData = strData.Split('_');
                int oddsPresent = int.Parse(strPramsDropData[0]);
                int id = int.Parse(strPramsDropData[1]);
                if (Tools.IsHitOdds(oddsPresent))
                {
                    EquipItem ei = eiManager.GenerateAEquipItem(id, EEquipItemQLevel.Legend);
                    eis.Add(ei);
                }
            }
            else if (strDropType.Equals("tlevel"))
            {
                // 指定财宝等级掉落
                string[] strPramsDropData = strData.Split('_');
                int oddsPresent = int.Parse(strPramsDropData[0]);
                int tlevel = int.Parse(strPramsDropData[1]);
                int minCount = int.Parse(strPramsDropData[2]);
                int maxCount = int.Parse(strPramsDropData[3]);
                int oddsUncommon = int.Parse(strPramsDropData[4]);
                int oddsMagic = int.Parse(strPramsDropData[5]);

                if (Tools.IsHitOdds(oddsPresent))
                {
                    int count = UnityEngine.Random.RandomRange(minCount, maxCount);

                    // 额外掉落修正
                    if (enermy != null)
                    {
                        count += enermy.dropOffset;
                    }

                    int minTLevel = tlevel - 1;
                    if (minTLevel < 1)
                    {
                        minTLevel = 1;
                    }
                    int maxTLevel = tlevel;
                    int[] baseIds = GameDatas.GetEquipItemBaseIdsBetweenTLevel(minTLevel, maxTLevel, count);
                    for (int indexId = 0; indexId < count; indexId++)
                    {
                        int baseId = baseIds[indexId];
                        EquipItem ei = null;
                        // 稀有
                        if (Tools.IsHitOdds(oddsUncommon))
                        {
                            ei = eiManager.GenerateAEquipItem(baseId, EEquipItemQLevel.Uncommon);
                        }
                        // 魔法
                        else if (Tools.IsHitOdds(oddsMagic))
                        {
                            ei = eiManager.GenerateAEquipItem(baseId, EEquipItemQLevel.Magic);
                        }
                        // 普通
                        else
                        {
                            ei = eiManager.GenerateAEquipItem(baseId, EEquipItemQLevel.Normal);
                        }
                        eis.Add(ei);
                    }
                }
            }
            else
            {
                Debug.LogError("无法识别的掉落类型:" + strDropType);
            }
        }
        return eis;
    }
    #endregion
    #region 经验
    /// <summary>
    /// 到达某一等级需要的总经验
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetNeedEXP(int level)
    {
        return IConst.EXP_A * level * level + IConst.EXP_B * level;
    }

    /// <summary>
    /// 根据怪物等级获取其提供的经验值
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetMonEXP(int playerlevel, int level)
    {
        int exp = IConst.EXP_MON_K * level + IConst.EXP_MON_BASE;
        if (playerlevel - level >= 3)
        {
            exp = Mathf.CeilToInt(exp * 0.5f);
        }
        return exp;
    }
    #endregion

    void OnTap(TapGesture gesture)
    {
        if (EnableInput && (_RoundLogicState == GameRoundLogicState.Normal) && !UIManager.Inst.HasUI())
        {
            Ray ray = cameraControl.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            int layer = LayerMask.NameToLayer("MapGrid");
            RaycastHit2D rh = Physics2D.Raycast(cameraControl.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1000, 1 << layer);
            if (rh.collider != null)
            {
                OnTapAMapGrid(rh.collider.GetComponent<MapGrid>());
            }
        }
    }

    void OnTapAMapGrid(MapGrid mg)
    {
        // 战术技能选择目标
        //if (State == GameRoundLogicState.TarcticsSkilling)
        //{
        //    if (mg.GetItem<Enermy>() != null)
        //    {
        //        // 点击一个敌人
        //        // 距离检测
        //        bool success = true;
        //        if (gSkillTractic.range > 0)
        //        {
        //            int dis = MapGrid.GetDis(_MHero.GetCurMapGrid(), mg);
        //            if (dis > gSkillTractic.range)
        //            {
        //                success = false;
        //            }
        //        }
        //        if (success)
        //        {

        //            IActor target = mg.GetItem<Enermy>();
        //            gSkillTractic.SetCaster(_MHero);
        //            gSkillTractic.SetTarget(target);
        //            StartCoroutine(gSkillTractic.Act());
        //            // 恢复至默认图标
        //            GameObject gobjBtnSpell = UIManager._Instance.uiMain.GetGobjOfASpell(gSkillTractic);
        //            UISprite skillIcon = gobjBtnSpell.GetComponent<UISprite>();
        //            skillIcon.spriteName = gSkillTractic.GetBaseData().iconName;
        //        }
        //        else
        //        {
        //            UIManager._Instance.GeneralTip("距离太远", Color.red);
        //        }
        //    }
        //}
        //else 
        if (_RoundLogicState == GameRoundLogicState.Normal)
        {
            if (InputState == EPlayInputState.Nomal)
            {
                if (mg.GetItem<Enermy>() != null)
                {
                    // 点击一个敌人
                    OnTouchAEnermyInNormal(mg.GetItem<Enermy>());
                }
                else if (mg.GetItem<ItemTreasureChest>() != null)
                {
                    if (mg.IsNear(_MHero.GetCurMapGrid()))
                    {
                        mgInterActive = mg;
                        ItemTreasureChest itc = mg.GetItem<ItemTreasureChest>();
                        OnTriggerItem_TreasureChest(itc);
                    }
                    else
                    {
                        UIManager.Inst.GeneralTip("距离太远", Color.white);
                    }
                }
                else if (mg.GetItem<ItemNPC>() != null)
                {
                    if (mg.IsNear(_MHero.GetCurMapGrid()))
                    {
                        mgInterActive = mg;
                        ItemNPC itemNPC = mg.GetItem<ItemNPC>();
                        OnTriggerItem_NPC(itemNPC);
                    }
                    else
                    {
                        UIManager.Inst.GeneralTip("距离太远", Color.white);
                    }
                }
                else if (mg.GetItem<ItemTransfer>() != null)
                {
                    if (mg.IsNear(_MHero.GetCurMapGrid()))
                    {
                        mgInterActive = mg;
                        ItemTransfer itemTransfer = mg.GetItem<ItemTransfer>();
                        OnTriggerItem_Transfer(itemTransfer);
                    }
                    else
                    {
                        UIManager.Inst.GeneralTip("距离太远", Color.white);
                    }
                }
                else if (mg.GetItem<ItemDoor>() != null)
                {
                    if (mg.IsNear(_MHero.GetCurMapGrid()))
                    {
                        mgInterActive = mg;
                        ItemDoor door = mg.GetItem<ItemDoor>();
                        OnTriggerItem_Door(door);
                    }
                    else
                    {
                        UIManager.Inst.GeneralTip("距离太远", Color.white);
                    }
                }
                else if (mg.GetItem<ItemBase>() != null)
                {
                    if (mg.IsNear(_MHero.GetCurMapGrid()))
                    {
                        mgInterActive = mg;
                        ItemBase item = mg.GetItem<ItemBase>();
                        item.OnTiggered();
                    }
                    else
                    {
                        UIManager.Inst.GeneralTip("距离太远", Color.white);
                    }
                }
            }
            else if (InputState == EPlayInputState.ChooseTarget)
            {
                if (gCurSelectMg != null)
                {
                    gCurSelectMg.ChooseState = EChoosedState.Choosable;
                }
                gCurSelectMg = mg;
                gCurSelectMg.ChooseState = EChoosedState.Choosed;
            }

        }
    }



    /// <summary>
    /// 取消选择目标
    /// </summary>
    internal void OnCancelUseItem()
    {
        GameView.Inst.InputState = EPlayInputState.Nomal;
        foreach (MapGrid itemMG in mListMGs)
        {
            itemMG.ChooseState = EChoosedState.UnChooseable;
        }
    }

    /// <summary>
    /// 确认对目标使用道具
    /// </summary>
    internal void OnComfirmUseItemToTarget()
    {
        //恢复状态
        GameView.Inst.InputState = EPlayInputState.Nomal;
        foreach (MapGrid itemMG in mListMGs)
        {
            itemMG.ChooseState = EChoosedState.UnChooseable;
        }

        if (gCurSelectMg != null && gItemToUse != null)
        {
            if (gItemToUse.baseData.type == EEquipItemType.Torch)
            {
                //使用火把
                UseItemTorch(gCurSelectMg, gItemToUse);
            }

            // 移除数量
            RedeceEiCount(gItemToUse);
            //使用结束
            gItemToUse = null;
        }

        //回合结束
        PlayerActionEnd();
    }

    /// <summary>
    /// 使用道具 - 火把
    /// </summary>
    /// <param name="gCurSelectMg"></param>
    /// <param name="gItemToUse"></param>
    private void UseItemTorch(MapGrid curSelectMg, EquipItem itemToUse)
    {
        if (curSelectMg.CanBurn())
        {
            curSelectMg.burnRoundIndex = gRoundCount;
            curSelectMg.Burn();
        }
    }

    /// <summary>
    /// 点击一个敌人
    /// </summary>
    void OnTouchAEnermyInNormal(Enermy enermy) 
    {
        //隐身的敌人不可点击
        if (!enermy.HasBuff<Buff_Hiding>())
        {
            UIManager.Inst.ShowUIEnermyInfo(enermy);
        }
    }

    /// <summary>
    /// 开始使用战术技能
    /// </summary>
    public void StartSkillTractics(ISkill skill)
    {
        //State = GameRoundLogicState.TarcticsSkilling;
        gSkillTractic = skill;
        if (skill.GetBaseData().targetType != ESkillTargetType.None)
        {
            UIManager.Inst.GeneralTip("选择一个目标", Color.yellow);
            GameObject gobjBtnSpell = UIManager.Inst.uiMain.GetGobjOfASpell(skill);
            UISprite skillIcon = gobjBtnSpell.GetComponent<UISprite>();
            skillIcon.spriteName = "btn_cancel";
        }
        else
        {
            Debug.Log("TODO:立即释放一个战术技能");
        }
    }

    /// <summary>
    /// 取消战术技能
    /// </summary>
    public void CancelSkillTractics()
    {
        _RoundLogicState = GameRoundLogicState.Normal;
        UIManager.Inst.GeneralTip("取消战术技能", Color.yellow);
        GameObject gobjBtnSpell = UIManager.Inst.uiMain.GetGobjOfASpell(gSkillTractic);
        UISprite skillIcon = gobjBtnSpell.GetComponent<UISprite>();
        skillIcon.spriteName = gSkillTractic.GetBaseData().iconName;
    }

#region 任务
    /// <summary>
    /// 开始一个任务
    /// </summary>
    /// <param name="missionId"></param>
    public void StartAMission(int missionId) 
    {
        MissionBD mission = GameDatas.GetMissionBD(missionId);
        if (mission.parent == 0)
        {
            // 是父任务
            UIManager.Inst.ShowFloatTip("开始:" + mission.targetDesc);
            // 找到第一个子任务
            MissionBD nextMission = GameDatas.GetNextMission(mission);
            if (nextMission != null)
            {
                _MHero._CurMainMission = nextMission;
                UIManager.Inst.ShowFloatTip(nextMission.targetDesc);
            }
            else
            {
                // 无子任务
                _MHero._CurMainMission = mission;
            }
        }
        else
        {
            // 开始一个子任务
            _MHero._CurMainMission = mission;
            UIManager.Inst.ShowFloatTip(mission.targetDesc);
        }
    }

    /// <summary>
    /// 完成一个任务
    /// </summary>
    public void CompleteAMission(MissionBD mission)
    {
        if (mission == null)
        {
            Debug.LogError("Error!");
            return;
        }
        //MissionBD mission = GameResources.GetMissionBD(missionId);
        UIManager.Inst.ShowFloatTip("完成:" + mission.targetDesc);
        // 获得奖励
        string strReward = mission.reward;
        if (!string.IsNullOrEmpty(strReward))
        {
            List<EquipItem> equipItems = GetMonsterDropList(strReward);
            for (int i = 0; i < equipItems.Count; i++)
            {
                EquipItem ei = equipItems[i];
                if (ei.baseData.id == 11) //金币
                {
                    eiManager.GetGold(ei.count);
                    UIManager.Inst.AddASmallTip("获得金币" + ei.count);
                }
                else
                {
                    DoAddAEquipToBag(ei);
                }
            }
        }
     
        // 取下一个任务
        MissionBD missionNext = GameDatas.GetNextMission(mission);
        if (missionNext != null)
        {
            StartAMission(missionNext.id);
        }
        else
        {
            _MHero._CurMainMission = null;
        }
    }
#endregion
    #region 多地图
    
    /// <summary>
    /// 玩家前往一张地图。目标格子id为-1表示在Start格子位置， -2表示在StartAndToHome格子
    /// </summary>
    /// <param name="mapTarget"></param>
    /// <param name="targetGridId"></param>
    public void PlayerToMap(GameMapBaseData mapTarget, int targetGridId) 
    {
        if (mCurGameMap != null && mCurGameMap.baseData.id == mapTarget.id)
        {
            //前往当前地图，不处理
            return;
        }

        ////当前地图怪物失去目标
        if (mCurGameMap != null)
        {
            for (int i = 0; i < mListEnermys.Count; i++)
            {
                Enermy enermy = mListEnermys[i];
                enermy._AIState = EAIState.Normal;
                if (enermy._State == EActorState.Battle)
                {
                    //战斗中的退出战斗
                    enermy.ExitFromBattle();
                }
            }
        }

        UIManager.Inst.uiMain.ShowMapName(mapTarget.name);

        _CurRound = 0;

        StartCoroutine(CoChangeMap(mapTarget, targetGridId));
    }

    // 通过地图出入口前往目标地图
    public void ComfirmChangeMap(GameMapBaseData gameMapTarget, int targetMapGridId)
    {
        PlayerToMap(gameMapTarget, targetMapGridId);
    }

    IEnumerator CoChangeMap(GameMapBaseData gameMapTarget, int targetMapGridId) 
    {
        GameObject gobjTemp = new GameObject();
        _MHero.transform.parent = gobjTemp.transform;
        //载入地图，并设置为当前地图
        yield return StartCoroutine(CoLoadMap(gameMapTarget));

        MapGrid mgTarget = null;
        if (targetMapGridId >= 0)
        {
            mgTarget = GetMapGridById(targetMapGridId);
        }
        else if(targetMapGridId == -1)
        {
            mgTarget = FinePlayerStartGrid();
        }
        else if (targetMapGridId == -2)
        {
            mgTarget = FindStartAndToHomeGrid();
        }

        _MHero.SetMapGrid(mgTarget);
        DestroyObject(gobjTemp);
        cameraControl.target = _MHero.gameObject;

        if (mCurGameMap.baseData.isHome)
        {
            // 如果进入了一个城镇，则保存
            GameManager.commonCPU.SaveCurHomeMap(mCurGameMap.baseData.id);

            //恢复精力
            Hero.Inst.Prop.Vigor = Hero.Inst.Prop.VigorMax;
            UIManager.Inst.uiMain.RefreshHeroVigor();
        }

        // 任务检查
        if (_MHero._CurMainMission != null && _MHero._CurMainMission.targetType == EMissionType.IntoMap
           && mCurGameMap.baseData.id == _MHero._CurMainMission.targetId)
        {
            CompleteAMission(_MHero._CurMainMission);
        }

        yield return 0;

        GameView.Inst.RoundLogicOnInitMap();
    }
    #endregion

    #region 道具使用
    public bool OnStartUseItem(EquipItem ei) 
    {
        gItemToUse = ei;
        if (ei.baseData.type == EEquipItemType.HPPotion)
        {
            // 弱效治疗药水
            string data = ei.baseData.data;
            JSONNode jdData = JSONNode.Parse(data);
            int val = jdData["val"].AsInt;

            _MHero.RecoverHp(val);
            
            // 移除数量
            RedeceEiCount(ei);
            //使用结束
            gItemToUse = null;
        }

        else if (ei.baseData.type == EEquipItemType.ResPoiPotion)
        {
            // 毒抗药水
            string data = ei.baseData.data;
            JSONNode jdData = JSONNode.Parse(data);
            int val = jdData["val"].AsInt;
            int roundDur = jdData["dur"].AsInt;
            bool overlay = jdData["overlay"].AsInt > 0 ? true : false;

            if (!overlay)
            {
                // 不允许叠加buff
                Buff_AddResPosi bap = _MHero.gameObject.GetComponent<Buff_AddResPosi>();
                if (bap != null)
                {
                    return false;
                }
            }

            Buff_AddResPosi bapNew = _MHero.gameObject.AddComponent<Buff_AddResPosi>();
            bapNew.Init(_MHero, val, roundDur);
            bapNew.StartEffect();
            // 移除数量
            RedeceEiCount(ei);
            //使用结束
            gItemToUse = null;
        }
        else if (ei.baseData.type == EEquipItemType.Torch)
        {
            UIManager.Inst.CloseUIBag();
            //火把
            //需要选择目标
            InputState = EPlayInputState.ChooseTarget;
        }
        else if (ei.baseData.type == EEquipItemType.Core)
        {
            //升级核心
            _Level++;
            OnLevelUP();
            // 移除数量
            RedeceEiCount(ei);
            //使用结束
            gItemToUse = null;
        }
        else if (ei.baseData.type == EEquipItemType.CoreDebris)
        {
            //核心碎片
            OnUseItem_CoreDebris(ei);
           
            //使用结束
            gItemToUse = null;
        }
        return true;
    }

    /// <summary>
    /// 当使用核心碎片
    /// </summary>
    /// <param name="ei"></param>
    private void OnUseItem_CoreDebris(EquipItem ei)
    {
        //检查数量
        int count = ei.count;
        int need = ei.baseData.GetIntData("need");
        if (count >= need)
        {
            // 移除数量
            RedeceEiCount(ei, need);

            EquipItem eiInbag = null;
            bool canPile = false;
            EquipItem eiNew = eiManager.GenerateAEquipItem(96, EEquipItemQLevel.Normal);
            if (DoAddAEquipToBag(eiNew))
            {
                UIManager.Inst.GeneralTip("合成成功", Color.yellow);
            }
        }
        else
        {
            UIManager.Inst.GeneralTip("数量不足", Color.red);
        }
    }

    /// <summary>
    /// 减少物品个数
    /// </summary>
    void RedeceEiCount(EquipItem ei, int count = 1) 
    {
        ei.count -= count;
        if (ei.count <= 0)
        {
            UIManager.Inst.uiMain.ClearItemUsed(ei);
            // 移除物品
            eiManager.RemoveFromItemsInBag(ei);
            //从背包移除
            UIManager.Inst.GetUIBag().RemoveAEquipItem(ei);
        }
        else
        {
            UIManager.Inst.GetUIBag().RefreshEquipItemCount(ei);
        }
        eiManager.SaveEquipItems();
    }
#endregion

    #region 试炼塔
    public void GoToTrial(int tier) 
    {
        GameMapBaseData map = GameDatas.GetGameMapBDByTier(tier);
        if (map != null)
        {
            PlayerToMap(map, -2);
        }
    }


    #endregion

    #region 传送站
    internal bool IsTransferActived(int mapID)
    {
        bool actived = false;
        string strTransferActived = GetTransferActivedSaved();
        if (strTransferActived.Contains(mapID + "&"))
        {
            actived = true;
        }
        return actived;
    }

    public string GetTransferActivedSaved()
    {
        string strTransActived = "";
        if (PlayerPrefs.HasKey(IConst.KEY_TRANSFER_ACTIVED))
        {
            strTransActived = PlayerPrefs.GetString(IConst.KEY_TRANSFER_ACTIVED);
        }
        return strTransActived;
    }

    public void SaveTransferActived(int mapID)
    {
        string strTransferActived = GetTransferActivedSaved();
        strTransferActived = strTransferActived + mapID + "&";
        PlayerPrefs.SetString(IConst.KEY_TRANSFER_ACTIVED, strTransferActived);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存已打开的门
    /// </summary>
    /// <param name="guid"></param>
    private void SaveDoorOpend(string guid)
    {
        string strDoorOpend = GetSavedDoorOpen();
        strDoorOpend = strDoorOpend + guid + "&";
        PlayerPrefs.SetString(IConst.KEY_DOOR_OPEND, strDoorOpend);
        PlayerPrefs.Save();
    }

    private string GetSavedDoorOpen()
    {
        string strDoorOpen = "";
        if (PlayerPrefs.HasKey(IConst.KEY_DOOR_OPEND))
        {
            strDoorOpen = PlayerPrefs.GetString(IConst.KEY_DOOR_OPEND);
        }
        return strDoorOpen;
    }

    /// <summary>
    /// 传送至地图
    /// </summary>
    /// <param name="map"></param>
    internal void OnTransferToMap(GameMapBaseData map)
    {
        PlayerToMap(map, -1);
    }
    #endregion

    public void SaveRecordGirlTip(string guid)
    {
        string str = PlayerPrefs.GetString(IConst.KEY_GIRLTIP);
        str = str + guid + "&";
        PlayerPrefs.SetString(IConst.KEY_GIRLTIP, str);
        PlayerPrefs.Save();
    }

    public bool HasRecordGirlTip(string guid)
    {
        string str = PlayerPrefs.GetString(IConst.KEY_GIRLTIP);
        if (!string.IsNullOrEmpty(str) && str.Contains(guid + "&"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal bool HasChestRecord(string guid)
    {
        string record = PlayerPrefs.GetString(IConst.KEY_CHEST_OPENED);
        return !string.IsNullOrEmpty(record) && record.Contains(guid + "&");
    }

    internal bool HasRecordDoorOpend(string guid)
    {
        string record = PlayerPrefs.GetString(IConst.KEY_DOOR_OPEND);
        return !string.IsNullOrEmpty(record) && record.Contains(guid + "&");
    }

    /// <summary>
    /// 更换装备后更新模型
    /// </summary>
    public void UpdateOnChangeEquip(EquipItem ei, bool isAdd)
    {
        if (!ei.IsInEquip())
        {
            return;
        }
        if (isAdd)
        {
            string modelSprite = ei.GetModel();

            if (ei._Part == EEquipPart.Hand1)
            {
                modelSprite += "_1";
            }
            else if (ei._Part == EEquipPart.Hand2)
            {
                modelSprite += "_2";
            }

            if (!string.IsNullOrEmpty(modelSprite))
            {
                NodeSprite ns = new NodeSprite(ei._Part, modelSprite, ei.GetColor());
                Hero.Inst.Avroar2D.SetDicSpriteNode(ns);
            }
        }
        else
        {
            // 脱下装备
            Hero.Inst.Avroar2D.RemoveSpriteNode(ei._Part);
        }
    }

    public void DoMoveAEquipItemToBag(EquipItem ei, int gridTo)
    {
        if (ei.IsInEquip())
        {
            UpdateOnChangeEquip(ei, false);
        }
        eiManager.MoveAEquipItemToBag(ei, gridTo);
    }

    public void DoMoveAEquipItemToEquip(EquipItem ei, EEquipPart part)
    {
        if (ei.IsInBag())
        {
            //从背包移到身上
            eiManager.MoveAEquipItemToEquip(ei, part);
            UpdateOnChangeEquip(ei, true);
        }
        else if (ei.IsInEquip())
        {
            //身上到身上
            UpdateOnChangeEquip(ei, false);
            eiManager.MoveAEquipItemToEquip(ei, part);
            UpdateOnChangeEquip(ei, true);
        }
        
    }

    /// <summary>
    /// 背包添加一件装备
    /// </summary>
    /// <param name="ei"></param>
    public bool DoAddAEquipToBag(EquipItem ei)
    {
        bool success = false;
        if (ei == null)
        {
            return false;
        }

        EquipItem eiInBag = null;//被修改的物品
        if (eiManager.AddAEquipItemToBag(ei, out eiInBag))
        {
            UIHeroBag bag = UIManager.Inst.GetUIBag();
            if (bag != null)
            {
                bag.RefreshAddAItem(eiInBag);
            }
           eiManager.SaveEquipItems();
            success = true;
        }
        else
        {
            UIManager.Inst.GeneralTip("背包已满", Color.red);
        }

        return success;
    }

    
}
