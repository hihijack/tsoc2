using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enermy : IActor {

    Animator animtor;
    SpriteRenderer spriteRender;
    public int monsterId;
    private MonsterBaseData monsterBD;

    public MonsterBaseData _MonsterBD
    {
        get { return monsterBD; }
        set 
        { 
            monsterBD = value; 
        }
    }
	
	public Hero curTarget;

    public int uiIndex; // 在战斗界面中的序列

    public bool needInitInStart = false; // 需要自动初始化

    public IMonSkill[] monSkills;

    public int dropOffset = 0; // 宝藏掉落修正个数
    public float dropCashOffet = 1f; // 金钱掉落倍数

    public bool isTierBoss = false; // 是否是试炼塔boss

	void Start () {
		
		base.Start();
		
		isHero = false;
        animtor = Tools.GetComponentInChildByPath<Animator>(gameObject, "model");
        spriteRender = Tools.GetComponentInChildByPath<SpriteRenderer>(gameObject, "model");

        if (needInitInStart)
        {
            MonsterBaseData mbd = GameDatas.GetMonsterBaseData(monsterId);
            Init(mbd);
        }
	}

    public SpriteRenderer GetSpriteRender() 
    {
        return spriteRender;
    }

    public override int GetAtkPhy()
    {
        return AtkWpon;
    }

    public void Init(MonsterBaseData mbd)
    {
        _MonsterBD = mbd;
        this.hp = mbd.hp;
        this._HpMax = mbd.hp;
        this.AtkWpon = mbd.atkMin;
        this.arm = mbd.arm;
        this.hit = mbd.hit;
        this.dodge = mbd.dodge;
        this.resFire = mbd.resfire;
        this.resForzen = mbd.resfrozen;
        this.resPoision = mbd.respoison;
        this.resThunder = mbd.reslighting;

        this.atkAnimTimeBeforeBase = mbd.atkTimeBefore;
        this.atkAnimTimeAfterBase = mbd.atkTimeAfter;
        this._IAS = mbd.ias;
        this._CurGridid = transform.parent.GetComponent<MapGrid>().g_Id;
        this._DeadlyStrike = 0.05f;

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
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        
	}

    public void RecoverHp(int hp)
    {
        this.hp += hp;
        if (this.hp > _HpMax)
        {
            this.hp = _HpMax;
        }
        UIManager._Instance.uiMain.RefreshTargetHP(this);
    }

    public void StartAttack()
    {
        StartCoroutine(CoStartAttackCurTarget());
    }

    public void PlayAnimAtk() 
    {
        animtor.Play("enermy_atk");
    }

    public void ShankColor(Color toColor) 
    {
        if (spriteRender != null)
        {
            spriteRender.color = toColor;
            StartCoroutine(CoTimingShankColor());
        }
    }

    IEnumerator CoTimingShankColor() 
    {
        yield return new WaitForSeconds(0.2f);
        spriteRender.color = Color.white;
    }

		// 不停自动攻击目标，直到目标死亡
	IEnumerator CoStartAttackCurTarget(){
		while(curTarget._State != EActorState.Dead){
            PlayAnimAtk();
            // 攻击前摇
			yield return new WaitForSeconds(this.atkAnimTimeBefore);
			
			if(_State == EActorState.Dead){
				break;
			}
			
			// 攻击伤害
            if (CheckHitTarget(curTarget))
            {
                int atk = GetAtk();
                OnAttackHit(curTarget, atk);
                curTarget.OnAttackedHit(this, atk);
                DamageTarget(atk, curTarget);
                UIManager._Instance.uiMain.RefreshHeroHP();
            }
            else
            {
                // 攻击被躲闪
                UIManager._Instance.ShowBattleStateInfo("闪避");
                OnAttackLost(curTarget);
                curTarget.OnAttackedLost(this);
            }
            
			// 攻击后摇
			yield return new WaitForSeconds(atkAnimTimeAfter);
			
			// 攻击间隔
			yield return new WaitForSeconds(atkTimeInterval);
		}
	}

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

        GameManager.hero.SetTargetEnermys(GameManager.hero.GetCurMapGrid());

        if (GameManager.hero.HasEnermyToBattle())
        {
            GameManager.hero.StartBattle();
        }
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
        if (monSkills == null)
        {
            return;
        }
        //技能触发检测
        for (int i = 0; i < monSkills.Length; i++)
        {
            IMonSkill skill = monSkills[i];
            skill.OnEnterBattle();
        }
    }

    /// <summary>
    /// 当死亡时
    /// </summary>
    public override void OnDead()
    {
        // 从警觉中移除
        GameManager.hero.RemoveFormAlterness(this);
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

    public override void OnHurted(int damage, EDamageType type, IActor target, bool isDS)
    {
        ShankColor(Color.red);

        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnHurt(target, damage, type, isDS);
            }
        }
    }

    public override void OnDamageTarget(int damage, IActor target, bool isDS)
    {
        base.OnDamageTarget(damage, target, isDS);
        if (monSkills != null)
        {
            // 技能触发检测
            for (int i = 0; i < monSkills.Length; i++)
            {
                IMonSkill skill = monSkills[i];
                skill.OnDamageTarget(target, damage, isDS);
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
}
