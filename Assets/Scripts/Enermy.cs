using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Enermy : IActor {
    #region 成员属性
    Animator animtor;
    SpriteRenderer spriteRender;
    public int monsterId;
    
    [Tooltip("怪物是否会刷新")]
    public bool refresh = true;

    private MonsterBaseData monsterBD;
    public Hero curBattleTarget;
    public int uiIndex; // 在战斗界面中的序列

    

    public bool needInitInStart = false; // 需要自动初始化

    public IMonSkill[] monSkills;

    public int dropOffset = 0; // 宝藏掉落修正个数

    public float dropCashOffet = 1f; // 金钱掉落倍数

    public bool isSummedBoss = false; // 是否召唤的精英怪

    private EAIState aiState = EAIState.Normal;//AI状态

    [System.NonSerialized]
    public FlagSpeedCtl _flagSpeedCtl;

    public FlagEnermyWaring _flagWaring;

    public bool enableAction = false;

    public ManagerBattleStateNPC gFSMManager;

    IAI AI;

    int gSkillIDCasting;
    IActor gSkillTargetCasting;

    /// <summary>
    /// 特别指定的掉落。设定时不使用基础掉落
    /// </summary>
    public string drops;

    public bool mToBattle = false;

    /// <summary>
    /// 受到有冲击力的伤害时间
    /// </summary>
    private float _lastHurtWithForce;
    #endregion

    #region GeterSeter
    public MonsterBaseData _MonsterBD
    {
        get { return monsterBD; }
        set
        {
            monsterBD = value;
        }
    }

    public EAIState _AIState
    {
        get
        {
            return aiState;
        }

        set
        {
            aiState = value;
            RefreshWaringFlag(HasBuff<Buff_Hiding>());
        }
    }

    public SpriteRenderer SpRender
    {
        get
        {
            if (spriteRender == null)
            {
                spriteRender = Tools.GetComponentInChildByPath<SpriteRenderer>(gameObject, "model");
            }
            return spriteRender;
        }
    }

    public void RefreshHiding(bool hiding)
    {
        if (hiding)
        {
            SpRender.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        }
        else
        {
            SpRender.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
        }
        RefreshWaringFlag(hiding);
    }

    public Animator Anim
    {
        get
        {
            if (animtor == null)
            {
                animtor = Tools.GetComponentInChildByPath<Animator>(gameObject, "model");
            }
            return animtor;
        }
    }

    #endregion
    void Start () {
		
		base.Start();
		
		isHero = false;

        if (!refresh  && GameView.Inst.HasKilledRecord(this))
        {
            //有击杀记录且不刷新。自动销毁
            DestroyObject(gameObject);
        }
        else
        {
            if (needInitInStart)
            {
                MonsterBaseData mbd = GameDatas.GetMonsterBaseData(monsterId);
                Init(mbd);
            }
        }
	}

    internal void OnAtkBeforeEnd()
    {
        gFSMManager.ActionAtkBeforeEnd(gSkillIDCasting, gSkillTargetCasting);
    }

    internal void OnAtkAfterEnd()
    {
        gFSMManager.ActionAtkAfterEnd();
    }

    public SpriteRenderer GetSpriteRender() 
    {
        return SpRender;
    }

    public void Init(MonsterBaseData mbd)
    {
        Prop = new PropertyNPC(this);
        _MonsterBD = mbd;
        Prop.Hp = mbd.hp;
        Prop.HpMax = mbd.hp;
        this.Prop.arm = mbd.arm;
        this.Prop.MoveSpeedBase = mbd.moveSpeed;
        this.Prop.resFire = mbd.resfire;
        this.Prop.resForzen = mbd.resfrozen;
        this.Prop.resPoision = mbd.respoison;
        this.Prop.resThunder = mbd.reslighting;

        //this.atkAnimTimeBeforeBase = mbd.atkTimeBefore;
        //this.atkAnimTimeAfterBase = mbd.atkTimeAfter;
       
        this._CurGridid = transform.parent.GetComponent<MapGrid>().g_Id;
        this.Prop.DeadlyStrike = 0.05f;

        this.Prop.maxTenactiyUCtl = mbd.tenacity;
        this.Prop.tenacityLevel = mbd.tenLevel;
        this.Prop.TenacityUCtl = this.Prop.maxTenactiyUCtl;
        // 初始化技能
        if (!string.IsNullOrEmpty(_MonsterBD.skills))
        {
            string[] skillNodes = _MonsterBD.skills.Split('&');
            for (int i = 0; i < skillNodes.Length; i++)
            {
                string skillNode = skillNodes[i];
                string[] strs = skillNode.Split('_');
                int level = int.Parse(strs[0]);
                int id = int.Parse(strs[1]);
                InitSkillById(level, id);
            }
            monSkills = GetComponents<IMonSkill>();
        }

        RefreshSpeedFlag();

        //Prop.BaseWeaponIAS = mbd.ias;

        GameView.Inst.AddToListEnermy(this);

        AI = GetComponent<IAI>();
        AI.Init(this);

        actorName = mbd.name;

        gFSMManager = new ManagerBattleStateNPC(this);

        //根据攻击时间计算攻击动画速率
    }

    private void RefreshSpeedFlag()
    {
        //if (_flagSpeedCtl == null)
        //{
        //   GameObject gobjFlag = Tools.LoadResourcesGameObject("Prefabs/Effects/speedflag", gameObject, 0f, 0f, 0f);
        //    _flagSpeedCtl = gobjFlag.GetComponent<FlagSpeedCtl>();

        //    _flagSpeedCtl.SetVisible(false);
        //}

        //_flagSpeedCtl.RefreshState(Hero.Inst.Prop.MoveSpeed - Prop.MoveSpeed);
    }

    public void RefreshWaringFlag(bool hiding)
    {
        if (_flagWaring == null)
        {
            GameObject gobjFlag = Tools.LoadResourcesGameObject("Prefabs/Effects/warningflag", gameObject, 0f, 0f, 0f);
            _flagWaring = gobjFlag.GetComponent<FlagEnermyWaring>();
        }
        if (hiding)
        {
            _flagWaring.SetVisible(false);
        }
        else
        {
            switch (_AIState)
            {
                case EAIState.Normal:
                    _flagWaring.SetVisible(false);
                    break;
                case EAIState.FindTarget:
                    _flagWaring.SetVisible(true);
                    break;
                case EAIState.Battle:
                    _flagWaring.SetVisible(false);
                    break;
                default:
                    break;
            }
        }
    }

    void InitSkillById(int level, int id) 
    {
        switch (id)
        {
            case 1:
                {
                    // 领主
                    LingZhu lz = gameObject.AddComponent<LingZhu>();
                    lz.Init(level);
                }
                break;
            case 2:
                {
                    // 自爆
                    ZiBao zb = gameObject.AddComponent<ZiBao>();
                    zb.Init(level);
                }
                break;
            case 3:
                {
                    // 冰冷强化
                    BingLengQiangHua blqh = gameObject.AddComponent<BingLengQiangHua>();
                    blqh.Init(level);
                }
                break;
            case 4: 
                {
                    // 火焰强化
                    HuoYanQiangHua hyqh = gameObject.AddComponent<HuoYanQiangHua>();
                    hyqh.Init(level);
                }
                break;
            case 5:
                {
                    // 闪电强化
                    ShanDianQiangHua sdqh = gameObject.AddComponent<ShanDianQiangHua>();
                    sdqh.Init(level);
                }
                break;
            case 6:
                {
                    // 毒素强化
                    DuSuQiangHua dsqh = gameObject.AddComponent<DuSuQiangHua>();
                    dsqh.Init(level);
                }
                break;
            case 7:
                {
                    // 尖刺外壳
                    JianCiWaike jcwk = gameObject.AddComponent<JianCiWaike>();
                    jcwk.Init(level);
                }
                break;
            case 8:
                {
                    // 嗜血
                    ShiXue sx = gameObject.AddComponent<ShiXue>();
                    sx.Init(level);
                }
                break;
            case 9: 
                {
                    // 不灭的怨恨
                    BuMieYuanHen mbyh = gameObject.AddComponent<BuMieYuanHen>();
                    mbyh.Init(level);
                }
                break;
            case 10:
                {
                    // 仇恨
                    MonChouHen mch = gameObject.AddComponent<MonChouHen>();
                    mch.Init(level);
                }
                break;
            case 11: 
                {
                    //强袭
                    MonQiangXi mqx = gameObject.AddComponent<MonQiangXi>();
                    mqx.Init(level);
                }
                break;
            case 12:
                {
                    //反击
                    MonFanJi mfj = gameObject.AddComponent<MonFanJi>();
                    mfj.Init(level);
                }
                break;
            case 13:
                {
                    // 恶魔集结
                    MonTuanJie mtj = gameObject.AddComponent<MonTuanJie>();
                    mtj.Init(level);
                }
                break;
            case 14:
                {
                    // 残暴
                    MonCanBao mcb = gameObject.AddComponent<MonCanBao>();
                    mcb.Init(level);
                }
                break;
            case 15:
                {
                    // 绝地狂怒
                    MonJueDiKuangNu jdkn = gameObject.AddComponent<MonJueDiKuangNu>();
                    jdkn.Init(level);
                }
                break;
            case 16:
                {
                    // 恶魔甲
                    MonEMoJia emj = gameObject.AddComponent<MonEMoJia>();
                    emj.Init(level);
                }
                break;
            case 17:
                {
                    // 血热
                    MonXueRe mxr = gameObject.AddComponent<MonXueRe>();
                    mxr.Init(level);
                }
                break;
            case 18:
                {
                    // 恐怖威吓
                    MonWeiXia mwx = gameObject.AddComponent<MonWeiXia>();
                    mwx.Init(level);
                }
                break;
            case 19:
                {
                    // 屯宝魔
                    MonTunBaoMo mtbm = gameObject.AddComponent<MonTunBaoMo>();
                    mtbm.Init(level);
                }
                break;
            case 20:
                {
                    // 火焰之力
                    MonHuoYanZhiLi mhyzl = gameObject.AddComponent<MonHuoYanZhiLi>();
                    mhyzl.Init(level);
                }
                break;
            case 21:
                {
                    // 闪电之力
                    MonShanDianZhiLi msdzl = gameObject.AddComponent<MonShanDianZhiLi>();
                    msdzl.Init(level);
                }
                break;
            case 22:
                {
                    // 冰霜之力
                    MonHanBingZhiLi mhbzl = gameObject.AddComponent<MonHanBingZhiLi>();
                    mhbzl.Init(level);
                }
                break;
            case 23:
                {
                    // 毒素之力
                    MonDuSuZhiLi mdszl = gameObject.AddComponent<MonDuSuZhiLi>();
                    mdszl.Init(level);
                }
                break;
            case 25:
                {
                    // 幽灵攻击
                    MonYouLingGongJi mylgj = gameObject.AddComponent<MonYouLingGongJi>();
                    mylgj.Init(level);
                }
                break;
            case 26:
                {
                    // 强电
                    MonQiangDian mqd = gameObject.AddComponent<MonQiangDian>();
                    mqd.Init(level);
                }
                break;
            case 27:
                {
                    //轻攻击
                    MonAtk ma = gameObject.AddComponent<MonAtk>();
                    ma.Init(level);
                }
                break;
            case 28:
                {
                    //重攻击
                    MonAtkHeavy mah = gameObject.AddComponent<MonAtkHeavy>();
                    mah.Init(level);
                }
                break;
            case 30:
                {
                    //火焰防护
                    MoFaHuZhao mfhz = gameObject.AddComponent<MoFaHuZhao>();
                    mfhz.Init(level);
                }
                break;
            case 31:
                {
                    //死灵支配
                    SiLingZhiPei slzp = gameObject.AddComponent<SiLingZhiPei>();
                    slzp.Init(level);
                }
                break;
            case 32:
                {
                    //愤怒攻击
                    MonAtkFury furyAtk = gameObject.AddComponent<MonAtkFury>();
                    furyAtk.Init(level);
                }
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        //RefreshSpeedFlag();

        if (_State == EActorState.Battle)
        {
            AI.DoUpdate();
            gFSMManager.Update();
        }

        //韧性受击N秒后恢复
        if (Time.time - _lastHurtWithForce > IConst.TenRecoverTimeForNPC)
        {
            Prop.TenacityUCtl = Prop.maxTenactiyUCtl;
        }
	}

    public void RecoverHp(int hp)
    {
        this.Prop.Hp += hp;
        if (this.Prop.Hp > Prop.HpMax)
        {
            this.Prop.Hp = Prop.HpMax;
        }
        UIManager.Inst.uiMain.RefreshTargetHP(this);
    }

    internal IBaseBuff[] GetBuffs()
    {
        return GetComponents<IBaseBuff>();
    }

    //public void StartAttack()
    //{
    //    StartCoroutine(CoStartAttackCurTarget());
    //}

    //public void PlayAnimAtk() 
    //{
    //    Anim.speed = animRate;
    //    Anim.Play("enermy_atk");
    //}

    public void ShankColor(Color toColor) 
    {
        if (SpRender != null)
        {
            SpRender.color = toColor;
            StartCoroutine(CoTimingShankColor());
        }
    }

    IEnumerator CoTimingShankColor() 
    {
        yield return new WaitForSeconds(0.2f);
        SpRender.color = Color.white;
    }

    // 不停自动攻击目标，直到目标死亡
    //IEnumerator CoStartAttackCurTarget()
    //{
    //    while (curBattleTarget._State != EActorState.Dead)
    //    {
    //        PlayAnimAtk();
    //        SpRender.color = Color.red;
    //        // 攻击前摇
    //        yield return new WaitForSeconds(this.AtkAnimTimeBefore);

    //        if (_State == EActorState.Dead)
    //        {
    //            break;
    //        }

    //        // 攻击伤害
    //        if (CheckHitTarget(curBattleTarget))
    //        {
    //            int atk = Prop.GetAtk(null);
    //            OnAttackHit(curBattleTarget, atk);
    //            curBattleTarget.OnAttackedHit(this, atk);
    //            DamageTarget(atk, curBattleTarget);
    //            UIManager.Inst.uiMain.RefreshHeroHP();
    //        }
    //        else
    //        {
    //            // 攻击被躲闪
    //            UIManager.Inst.ShowBattleStateInfo("闪避");
    //            OnAttackLost(curBattleTarget);
    //            curBattleTarget.OnAttackedLost(this);
    //        }

    //        SpRender.color = Color.white;
    //        // 攻击后摇
    //        yield return new WaitForSeconds(AtkAnimTimeAfter);

    //        float ranTime = UnityEngine.Random.Range(0.5f, 1.5f);
    //        // 攻击间隔
    //        yield return new WaitForSeconds(AtkTimeInterval * ranTime);
    //    }
    //}


    public override void OnIntoAGrid(MapGrid grid)
    {
        transform.parent = grid.transform;

        // 获取周围的敌人
        //targetEnermys.Clear();
        //List<MapGrid> mgs = grid.GetNearGrids();
        //for (int i = 0; i < mgs.Count; i++)
        //{
        //    MapGrid itemMg = mgs[i];
        //    if (itemMg.g_Id == GameManager.hero._CurGridid && (GameManager.hero._State == EActorState.Normal || GameManager.hero._State == EActorState.AIAttack))
        //    {
        //        GameManager.hero.AddAEnermy(this);
        //        break;
        //    }
        //}

        //GameManager.hero.SetTargetEnermys(GameManager.hero.GetCurMapGrid());

        //if (GameManager.hero.HasEnermyToBattle())
        //{
        //    GameManager.hero.StartBattle();
        //}
    }

    public override void OnLeaveAGrid(MapGrid grid)
    {
    }

    /// <summary>
    /// 获取战斗中的友军
    /// </summary>
    /// <returns></returns>
    public List<Enermy> GetAlliesInBattle(bool includeSelf = false)
    {
        List<Enermy> allies = new List<Enermy>();
        for (int i = 0; i < GameManager.hero.GetTargetsInBattle().Count; i++)
        {
            Enermy temp = GameManager.hero.GetTargetsInBattle()[i];
            if (temp != null && temp._State != EActorState.Dead)
            {
                if (includeSelf || (!includeSelf && temp != this))
                {
                    allies.Add(temp);
                }
            }
        }
        return allies;
    }

    /// <summary>
    /// 当进入战斗
    /// </summary>
    public override void OnEnterBattle()
    {
        gFSMManager.Start();

        //技能触发检测
        if (monSkills != null)
        {
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnEnterBattle();
            }
        }
    }

    /// <summary>
    /// 当死亡时
    /// </summary>
    public override void OnDead()
    {
        GameView.Inst.RemoveFormListEnermy(this);
        // 从警觉中移除
        //GameManager.hero.RemoveFormAlterness(this);
        // 技能触发检测
        if (monSkills != null)
        {
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnDead();
            }
        }
       

        List<Enermy> allys = GetAlliesInBattle();
        for (int allyIndex = 0; allyIndex < allys.Count; allyIndex++)
        {
            Enermy allyItem = allys[allyIndex];
            IMonSkill[] monSkillsAlly = allyItem.monSkills;
            if (monSkillsAlly != null)
            {
                for (int iAlly = 0; iAlly < monSkillsAlly.Length; iAlly++)
                {
                    IMonSkill skill = monSkillsAlly[iAlly];
                    skill.OnAllyDead(this);
                }
            }
        }

        //不刷新的怪，保存击杀记录
        if (!refresh)
        {
            GameView.Inst.SaveKillRecord(guid);
        }

        //音效
        CommonCPU.Inst.PlayerAudio(IPath.AudioMon +  monsterBD.soundDie);
    }

    /// <summary>
    /// 当一次攻击命中
    /// </summary>
    /// <param name="target"></param>
    public override void OnAttackHit(IActor target, int atkOri)
    {
        if (monSkills == null)
        {
            return;
        }
        // 技能触发检测
        for (int i = 0; i < monSkills.Length; i++)
        {
            IMonSkill skill = monSkills[i];
            skill.OnAttackHit(target, atkOri);
        }
    }

    public override void OnAttackedHit(IActor atker, int atkOri)
    {
        if (monSkills == null)
        {
            return;
        }
        // 技能触发检测
        for (int i = 0; i < monSkills.Length; i++)
        {
            IMonSkill skill = monSkills[i];
            skill.OnAttackedHit(atker, atkOri);
        }
    }

    public override void OnHurted(IActor target, DmgData dmgData)
    {
        ShankColor(Color.red);
      
        if (dmgData.force > 0)
        {
            _lastHurtWithForce = Time.time;
        }

        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnHurt(target, dmgData);
            }
        }

        //韧性处理
        if (!gFSMManager.InUnControl())
        {
            if (dmgData.force > Prop.tenacityLevel)
            {
                //直接硬直
                gFSMManager.ActionUnControl(GetUCtrlTimeByFore(dmgData.force));
            }
            else
            {
                //造成削韧
                Prop.TenacityUCtl -= dmgData.dp;
                if (Prop.TenacityUCtl <= 0)
                {
                    gFSMManager.ActionUnControl(GetUCtrlTimeByFore(dmgData.force));
                }
            }
        }
    }

    /// <summary>
    /// 根据冲击力获得硬直时间
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    private float GetUCtrlTimeByFore(int force)
    {
        float r = 0f;
        r = force * 1.5f;
        return r;
    }

    public override void OnDamageTarget(IActor target, DmgData dmgData)
    {
        base.OnDamageTarget(target,dmgData);
        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnDamageTarget(target, dmgData.TotalDmg(), dmgData.isDS);
            }
        }
      
    }

    public override void OnAttackLost(IActor target)
     {
        base.OnAttackLost(target);
        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnAtkLost(target);
            }
        }
        AI.OnAtkMiss();
    }

    public override void OnAttackedLost(IActor atker)
    {
        base.OnAttackedLost(atker);
        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnAtkedLost(atker);
            }
        }
        
    }

    public IMonSkill GetSkillById(int id)
    {
        IMonSkill skill = null;
        for (int i = 0; i < monSkills.Length; i++)
        {
            if (monSkills[i].skillBD.id == id)
            {
                skill = monSkills[i];
                break;
            }
        }
        return skill;
    }

    public override void OnHPChange(int valBefore, int valCur)
    {
        base.OnHPChange(valBefore, valCur);
        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnHPChange(valBefore, valCur);
            }
        }
    }

    public void AIAction()
    {
        MapGrid mgHero = Hero.Inst.GetCurMapGrid();
        MapGrid mgCur = GetCurMapGrid();
        int dis = MapGrid.GetDis(mgHero, mgCur);

        if (enableAction)
        {
            if (_AIState == EAIState.Normal)
            {
                if (dis <= monsterBD.view && Hero.Inst.CanFinded())
                {
                    //发现敌人
                    OnFindTarget();
                }
                AIActionEnd();
            }
            else if (_AIState == EAIState.FindTarget)
            {
                if (Hero.Inst.CanFinded())
                {
                    //处于攻击位置
                    bool canIntoBattle = CanIntoBattle();
                    //在近战范围内，进入近战战斗
                    if (dis == 1)
                    {
                        StartToBattle();
                    }
                    else if (!canIntoBattle)
                    {
                        //不在攻击位置，追击
                        MapGrid mgEnermyTo = PathToTarget(mgHero);

                        if (mgEnermyTo != null && mgEnermyTo.IsEnablePass())
                        {
                            MoveToAGrid(mgEnermyTo);
                        }
                        else
                        {
                            _AIState = EAIState.Normal;
                            AIActionEnd();
                        }
                    }
                    else if (canIntoBattle)
                    {
                        // 距离不等于1且在攻击位置
                        //远程怪物发动攻击
                        //yield return StartCoroutine(CoAtkInLongRange());
                        AtkInLongRange();
                    }
                }
                else
                {
                    LostTarget();
                    AIActionEnd();
                }
            }
        }
        else
        {
            AIActionEnd();
        }
    }

    private void StartToBattle()
    {
        if (HasBuff<Buff_Hiding>())
        {
            AtkInHiding();
        }
        else
        {
            OnToBattle();
        }
    }

    private void AtkInHiding()
    {
        RefreshHiding(false);
        int atk = 3 * Prop.GetAtk(null);
        OnAttackHit(Hero.Inst, atk);
        Hero.Inst.OnAttackedHit(this, atk);
        DamageTarget(Hero.Inst, new DmgData(atk, EDamageType.Phy));
        UIManager.Inst.uiMain.RefreshHeroHP();
        Invoke("AtkInHidingEnd", 1f);
    }

    void AtkInHidingEnd()
    {
        OnToBattle();
    }

    public override void OnMoveEnd()
    {
        base.OnMoveEnd();
        int dis = MapGrid.GetDis(Hero.Inst.GetCurMapGrid(), GetCurMapGrid());
        //在近战范围内
        if (dis == 1)
        {
            StartToBattle();
        }
        else
        {
            AIActionEnd();
        }
    }

    //internal IEnumerator CoAIAction()
    //{
    //    MapGrid mgHero = Hero.Inst.GetCurMapGrid();
    //    MapGrid mgCur = GetCurMapGrid();
    //    int dis = MapGrid.GetDis(mgHero, mgCur);
    //    //int readDis = MapGrid.GetDis(mgHero, mgCur);

    //    if (enableAction)
    //    {
    //        bool toBattle = false;//进入近战战斗
    //        if (_AIState == EAIState.Normal)
    //        {
    //            if (dis <= monsterBD.view && Hero.Inst.CanFinded())
    //            {
    //                //发现敌人
    //                OnFindTarget();
    //            }
    //        }
    //        else if (_AIState == EAIState.FindTarget)
    //        {
    //            if (Hero.Inst.CanFinded())
    //            {
    //                //处于攻击位置
    //                bool canIntoBattle = CanIntoBattle();
    //                //在近战范围内，进入近战战斗
    //                if (dis == 1)
    //                {
    //                    toBattle = true;
    //                }
    //                else if(!canIntoBattle)
    //                {
    //                    Debug.LogError(name);//#####
    //                    //不在攻击位置，追击
    //                    MapGrid mgEnermyTo = PathToTarget(mgHero);
                    
    //                    if (mgEnermyTo != null && mgEnermyTo.IsEnablePass())
    //                    {
    //                        Debug.LogError("move to" + mgEnermyTo);//########
    //                        yield return StartCoroutine(CoMoveToAGrid(mgEnermyTo));
    //                        //追击移动后
    //                        mgCur = GetCurMapGrid();
    //                        dis = MapGrid.GetDis(mgHero, mgCur);
    //                        //在近战范围内
    //                        if (dis == 1)
    //                        {
    //                            toBattle = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        _AIState = EAIState.Normal;
    //                    }
    //                }
    //                else if(canIntoBattle)
    //                {
    //                    // 距离不等于1且在攻击位置
    //                    //远程怪物发动攻击
    //                    yield return StartCoroutine(CoAtkInLongRange());
    //                }
    //            }
    //            else
    //            {
    //                LostTarget();
    //            }
    //        }
    //        //进入近战战斗
    //        if (toBattle)
    //        {
    //            //隐匿现身攻击
    //            if (HasBuff<Buff_Hiding>())
    //            {
    //                yield return StartCoroutine(CoAtkInHiding());
    //            }
    //            OnToBattle();
    //        }
    //    }
    //    yield return 0;
    //}

    /// <summary>
    /// 开始战斗AI
    /// </summary>
    internal void StartBattleAI()
    {
        AI.DoStart();
    }
    
    void AtkInLongRange()
    {
        GameObject gobjArrow = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/eff_arrow"));
        gobjArrow.transform.position = GetPos();
        ProjCtl projCtl = ProjCtl.ProjAGobj(gobjArrow);
        projCtl.SetProjMode(new ProjModeToPosInV(6f, Hero.Inst.GetPos(), OnLongRangeAtkHero));
    }

    //发动一次远程攻击
    //private IEnumerator CoAtkInLongRange()
    //{
    //    GameObject gobjArrow = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/eff_arrow"));
    //    gobjArrow.transform.position = GetPos();
    //    ProjCtl projCtl = ProjCtl.ProjAGobj(gobjArrow);
    //    projCtl.SetProjMode(new ProjModeToPosInV(6f, Hero.Inst.GetPos(), OnLongRangeAtkHero));
    //    while (projCtl != null)
    //    {
    //        yield return 1;
    //    }
    //}

    private void OnLongRangeAtkHero(ProjCtl ctl)
    {
        int atk = Prop.GetAtk(null);
        OnAttackHit(Hero.Inst, atk);
        Hero.Inst.OnAttackedHit(this, atk);
        DamageTarget(Hero.Inst, new DmgData(atk, EDamageType.Phy));
        UIManager.Inst.uiMain.RefreshHeroHP();
        DestroyObject(ctl.gameObject);
        AIActionEnd();
    }

    private void AIActionEnd()
    {
        GameView.Inst.OnAAIActionEnd(this);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnToBattle()
    {
        _AIState = EAIState.Battle;
        AIActionEnd();
    }

    /// <summary>
    /// AI判断当前位置是否可以进入战斗/攻击位置
    /// </summary>
    /// <returns></returns>
    public bool CanIntoBattle()
    {
        bool toBattle = false;
        MapGrid mgCur = GetCurMapGrid();
        MapGrid mgHero = Hero.Inst.GetCurMapGrid();
        int dis = MapGrid.GetRectDis(mgCur, mgHero);
        int disReal = MapGrid.GetDis(mgCur, mgHero);
        if (disReal <= monsterBD.atkrange)
        {
            //在攻击范围内
            toBattle = true;
        }
        else if (monsterBD.atkrange == 1 && dis == 1)
        {
            //近战怪物判断是否会被动加入战斗
            if (mgCur.IsNear(mgHero))
            {
                toBattle = true;
            }
            else
            {
                List<MapGrid> mgNear = mgCur.GetNearGrids();
                for (int j = 0; j < mgNear.Count; j++)
                {
                    MapGrid mgTemp = mgNear[j];
                    Enermy eTemp = mgTemp.GetItem<Enermy>();
                    if (eTemp != null)
                    {
                        int disTemp = MapGrid.GetDis(eTemp.GetCurMapGrid(), Hero.Inst.GetCurMapGrid());
                        if (disTemp == 1)
                        {
                            toBattle = true;
                            break;
                        }
                    }
                }
            }
           
        }
       
        return toBattle;
    }

    /// <summary>
    /// 从当前格子寻路至目标格子
    /// </summary>
    /// <param name="mgHero"></param>
    /// <returns></returns>
    private MapGrid PathToTarget(MapGrid mgTarget)
    {
        MapGrid next = null;
        MapGrid mgStart = GetCurMapGrid();
        mgStart.pathData.Clear();
        //Debug.LogError("path for " + monsterBD.name + ","  + mgStart.g_Id + "->" + mgTarget.g_Id);//#######
        List<MapGrid> closeList = new List<MapGrid>();
        List<MapGrid> openList = new List<MapGrid>();
        //起始格加到开启列表
        openList.Add(mgStart);
        MapGrid mgCur = null;
        bool findPath = false;
        while (openList.Count != 0)
        {
            //找出F值最小的点
            MapGrid minFGrid = openList[0];
            int disMin = minFGrid.pathData.F;
            foreach (MapGrid mgTemp in openList)
            {
                if (mgTemp.pathData.F < disMin)
                {
                    minFGrid = mgTemp;
                    disMin = minFGrid.pathData.F;
                }
            }
            //最小F格当作当前格
            mgCur = minFGrid;
            //F值最低点放到关闭列表
            openList.Remove(minFGrid);

            closeList.Add(minFGrid);

            if (closeList.Contains(mgTarget))
            {
                findPath = true;
                //路径被找到
                break;
            }
            //检索当前格子相邻格子
            List<MapGrid> listMgNear = mgCur.GetNearGrids();
            foreach (MapGrid mgNearTemp in listMgNear)
            {
                //可通过且不再关闭列表中
                if (mgNearTemp.IsEnablePass() && !closeList.Contains(mgNearTemp))
                {
                    if (openList.Contains(mgNearTemp))
                    {
                        //已经在开启列表中，检查新路径（经过当前格再到这个格子）是否更好
                        if (mgCur.pathData.G + 1 < mgNearTemp.pathData.G)
                        {
                            mgNearTemp.pathData.parent = mgCur;
                            mgNearTemp.pathData.G = mgCur.pathData.G + 1;
                            mgNearTemp.pathData.CalF();
                        }
                    }
                    else
                    {
                        //不在开启列表中，把它添加进去。把当前格作为这一格的父节点。记录这一格的F,G,和H值。
                        openList.Add(mgNearTemp);
                        mgNearTemp.pathData.parent = mgCur;
                        mgNearTemp.pathData.G = mgCur.pathData.G + 1;
                        mgNearTemp.pathData.H = MapGrid.GetDis(mgNearTemp, mgTarget);
                        mgNearTemp.pathData.CalF();
                    }
                }
            }
        }

        if (findPath)
        {
            List<MapGrid> path = new List<MapGrid>();
            path.Add(mgTarget);
            MapGrid parentOfLast = path[path.Count - 1].pathData.parent;

            while (parentOfLast != null)
            {
                path.Add(parentOfLast);
                parentOfLast = parentOfLast.pathData.parent;
            }

            if (path.Count >= 2)
            {
                next = path[path.Count - 2];
            }
        }
       
        return next;
    }

    private void OnFindTarget()
    {
        _AIState = EAIState.FindTarget;
        enableAction = false;
        //告诉周围的友军发现敌人
        List<MapGrid> mgs = GetCurMapGrid().GetNearGrids();
        for (int i = 0; i < mgs.Count; i++)
        {
            MapGrid mgNear = mgs[i];
            Enermy enermy = mgNear.GetItem<Enermy>();
            if (enermy != null && enermy._AIState == EAIState.Normal) 
            {
                enermy.OnFindTarget();
            }
        }
        GameView.Inst.AddToEnermysFindTargetInCurRound(this);

        //音效
        CommonCPU.Inst.PlayerAudio(IPath.AudioMon + monsterBD.soundFind);
    }

    /// <summary>
    /// 丢失目标
    /// </summary>
    void LostTarget()
    {
        _AIState = EAIState.Normal;
        enableAction = false;
    }

    #region 战斗状态
    public override EBattleState _BattleState
    {
        get
        {
            return gFSMManager.CurState.StateType;
        }
    }

    internal void OnBSStartDodge()
    {
        Anim.Play("dodge");
        CommonCPU.Inst.CreateEffect("eff_dodge", GetPos(), Color.white, -1, false);
    }

    internal void OnBSEndDodge()
    {
       
    }


    internal void OnBSStartIdle()
    {
        Anim.Play("idle");
    }

    internal void OnBSStartDef()
    {
        CommonCPU.Inst.CreateEffAtGobj("eff_shield", gameObject, Vector3.zero, new Color32(118, 118, 118, 255), 0);
    }

    public void OnBSStartUnCtl()
    {
        Anim.Play("stiff");
        CommonCPU.Inst.CreateEffAtGobj("eff_stun", gameObject, Vector3.zero, Color.white, 0);
        //恢复韧性
        Prop.TenacityUCtl = Prop.maxTenactiyUCtl;
    }

    public void OnBSEndUnCtl()
    {
        GameObject eff = Tools.GetGameObjectInChildByPathSimple(gameObject, "eff_stun");
        CommonCPU.Inst.RemoveEff(eff);
    }

    internal void OnBSEndDef()
    {
        GameObject eff = Tools.GetGameObjectInChildByPathSimple(gameObject, "eff_shield");
        CommonCPU.Inst.RemoveEff(eff);
    }

    /// <summary>
    /// 开始技能，技能前摇
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="target"></param>
    public void OnBSStartAtk(int skillId, IActor target)
    {
        MonSkillBD skill = GameDatas.GetMonSkillBD(skillId);
        gSkillIDCasting = skillId;
        gSkillTargetCasting = target;
        if (skill != null)
        {
            string anim = skill.anim;
            if (!string.IsNullOrEmpty(anim))
            {
                Anim.Play(anim, 0, 0f);
            }
        }
    }

    /// <summary>
    /// 开始攻击后摇
    /// </summary>
    /// <param name="skillid"></param>
    public void OnBSStartAtkAfter(int skillId, IActor target)
    {
        if (_State != EActorState.Dead)
        {
            //技能生效
            IMonSkill skill = GetSkillById(skillId);
            if (skill != null)
            {
                skill.StartEff(target);
            }
        }
    }

    internal void OnBSEndAtkAfter(int skillId, IActor target)
    {
        if (_State != EActorState.Dead)
        {
            //技能结束触发效果
            IMonSkill skill = GetSkillById(skillId);
            if (skill != null)
            {
                skill.OnAtkEnd();
            }
        }
    }

    #endregion 战斗状态

    internal void ExitFromBattle()
    {
        _State = EActorState.Normal;
        transform.localPosition = Vector3.zero;
    }

    internal string GetDrops()
    {
        if (!string.IsNullOrEmpty(drops))
        {
            return drops;
        }
        else
        {
            return _MonsterBD.drops;
        }
    }

    /// <summary>
    /// 检测技能触发 - 当发现目标
    /// </summary>
    internal void SkillCheckOnFindTarget()
    {
        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnFindTarget();
            }
        }
    }

    public override void OnStartAtked(IActor atker)
    {
        base.OnStartAtked(atker);
        //AI检测
        AI.OnAtked(atker);
    }
}
