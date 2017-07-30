using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;

public class Hero : IActor
{
    public static Hero _Inst;
    #region 成员属性
    public string nickname;
    public int[] arrItemUesd = new int[3]; // 使用的道具id
    public float speed = 10.0f;
    List<Enermy> targetEnermys = new List<Enermy>(4);
    public ISkill[] mSkills = new ISkill[4];
    /// <summary>
    /// 处于警觉中的敌人
    /// </summary>
    List<Enermy> enermysInAlterness = new List<Enermy>(4);

    public int expCur;
    public int expMax;
    public int cash;
    public int score;

    private MissionBD curMainMission;    // 当前主线任务
    public List<int> curMissionIds = new List<int>(5);    // 当前支线任务id

    public Enermy curTarget; // 当前攻击目标

    // 人物属性
    public int[] skillids;
    public int[] skillLvs;

    public int[] skillIdsTractics;
    public ISkill[] skillsTractics;

    public bool isInBattleState = false;

    // buff start
    // buff end

    public GameObject g_GobjWeaponRootHand1;
    public GameObject g_GobjWeaponRootHand2;
    public GameObject g_GobjWeaponRootBack;
    public GameObject g_GobjWeaponRootWaist1;
    public GameObject g_GobjWeaponRootWaist2;
    public GameObject gobjWeapon1;
    public GameObject gobjWeapon2;
    public GameObject gobjWeaponTwoHand;

    // 已装备的装备
    public List<EquipItem> itemsHasEquip = new List<EquipItem>();
    public int expCurLevel; // 当今等级经验

    private int gold; // 金币
    int bestTrial;
    /// <summary>
    /// 未分配技能点
    /// </summary>
    int g_SkillNeedAllot = 0;
    Avtoar2D avroar2D;
    // 背包里的物品
    public List<EquipItem> itemsInBag = new List<EquipItem>();
    #endregion

    #region GeterAndSeter

    public override EActorState _State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }


    public MissionBD _CurMainMission
    {
        get { return curMainMission; }
        set
        {
            curMainMission = value;
            if (curMainMission != null)
            {
                GameManager.commonCPU.SaveMissionStep();
            }
            else
            {
                Debug.LogError("curMainMission is NULL");
            }
        }
    }


    public int _Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            if (gold < 0)
            {
                gold = 0;
            }
            PlayerPrefs.SetInt(IConst.KEY_GOLD, gold);
            UIManager.Inst.RefreshHeroBagGold();
        }
    }



    public int _BestTrial
    {
        get { return bestTrial; }
        set
        {
            bestTrial = value;
            PlayerPrefs.SetInt(IConst.KEY_BEST_TRIAL, bestTrial);
        }
    }

    public int _SkillNeedAllot
    {
        get { return g_SkillNeedAllot; }
        set { g_SkillNeedAllot = value; GameManager.commonCPU.SaveSP(); }
    }

    public Avtoar2D _Avroar2D
    {
        get
        {
            if (avroar2D == null)
            {
                avroar2D = Tools.GetComponentInChildByPath<Avtoar2D>(gameObject, "model");
            }
            return avroar2D;
        }
    }

    public ManagerBattleState BsManager
    {
        get
        {
            return bsManager;
        }

        set
        {
            bsManager = value;
        }
    }
    #endregion

    public List<Enermy> GetTargetsInBattle()
    {
        return targetEnermys;
    }
    public ISkill GetSkillByIndex(int skillIndex)
    {
        ISkill skill = null;
        skill = mSkills[skillIndex - 1];
        return skill;
    }

    #region 战斗状态逻辑
    internal void OnBSStartSkill(ISkill skill)
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
       
        StartCoroutine(skill.Act());
        UIManager.Inst.uiMain.uiBattle.ToSkill(skill);
    }

    internal void OnBSStartUnControl(float dur)
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
        UIManager.Inst.uiMain.uiBattle.ToUnControl(dur);
    }

    internal void OnBSUpdateUnControl(float dur)
    {
        UIManager.Inst.uiMain.uiBattle.UpdateUnControlTime(dur);
    }

    /// <summary>
    /// 开始蓄力
    /// </summary>
    internal void OnBSStartPowering()
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
        UIManager.Inst.uiMain.uiBattle.ToPowering();
    }

    /// <summary>
    /// 蓄力更新
    /// </summary>
    internal void OnBSUpdatePowering()
    {
        _PowerVal += _Prop.PowerSpeed * Time.deltaTime;
        UIManager.Inst.uiMain.uiBattle.UpdatePowerVal(_PowerVal);
    }

    /// <summary>
    /// 进入普通状态
    /// </summary>
    internal void OnBSStartNormal()
    {
        UIManager.Inst.uiMain.uiBattle.ToNormalState();
        _PowerVal = 0f;
    }

    internal void OnBSStartDef()
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
        UIManager.Inst.uiMain.uiBattle.ShowDef(true);
    }

    internal void OnBSEndDef()
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
        UIManager.Inst.uiMain.uiBattle.ShowDef(false);
    }
    /// <summary>
    /// 后摇
    /// </summary>
    internal void OnBSStartAtkAfter()
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
        UIManager.Inst.uiMain.uiBattle.ToAfterPoint(AtkAnimTimeAfter);
    }

    /// <summary>
    /// 后摇
    /// </summary>
    internal void OnBSStartAtkBefore()
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            return;
        }
        UIManager.Inst.uiMain.uiBattle.ToAtkPoint(AtkAnimTimeBefore);
        OnStartAAttack(curTarget);
    }

    internal void OnBSStartHit()
    {
        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle || curTarget == null)
        {
            return;
        }
        // 攻击特效
        GameManager.commonCPU.CreateEffect(GetAtkEffName(), curTarget.transform.position, Color.white, -1f);

        if (CheckHitTarget(curTarget))
        {
            int atk = _Prop.Atk;
            // 攻击伤害
            OnAttackHit(curTarget, atk);
            curTarget.OnAttackedHit(this, atk);
            DamageTarget(atk, curTarget);
            DamageTarget(_Prop.AtkFire, curTarget, EDamageType.Fire);
            DamageTarget(_Prop.AtkThunder, curTarget, EDamageType.Lighting);
            DamageTarget(_Prop.AtkPoison, curTarget, EDamageType.Poison);
            DamageTarget(_Prop.AtkIce, curTarget, EDamageType.Forzen);
        }
        else
        {
            // 攻击被躲闪
            UIManager.Inst.ShowTargetBattleStateInfo("闪避");
            OnAttackLost(curTarget);
            curTarget.OnAttackedLost(this);
        }
    }
    #endregion

    public void RemoveFormAlterness(Enermy enermy) 
    {
        if (enermysInAlterness.Contains(enermy))
        {
            enermysInAlterness.Remove(enermy);
        }
    }

    public void ClearAlterness() 
    {
        enermysInAlterness.Clear();
    }

    public void AddToItemsHasEquip(EquipItem ei) 
    {
        itemsHasEquip.Add(ei);
    }

    public void RemoveFromItemHasEquip(EquipItem ei) 
    {
        if (itemsHasEquip.Contains(ei))
        {
            itemsHasEquip.Remove(ei);
        }
    }

    //public override int GetAtkPhy()
    //{
    //    return Mathf.RoundToInt(AtkWpon * (_Strength + 100) / 100f);
    //}

    public void AddToItemsInBag(EquipItem ei) 
    {
        itemsInBag.Add(ei);
    }

    public void RemoveFromItemsInBag(EquipItem ei) 
    {
        if (itemsInBag.Contains(ei))
        {
            itemsInBag.Remove(ei);
        }
    }

    private ManagerBattleState bsManager;

    void Start(){
		base.Start();
		isHero = true;
	}
	
	void Update(){
        if (_State == EActorState.Battle)
        {
            BsManager.Update();
        }
	}

    public void Init()
    {
        _Inst = this;
        BsManager = new ManagerBattleState(this);
        _Prop = new PropertyHero(this);
    }

    public override void SetDir(EDirection dir) 
    {
        Avtoar2D av2d = Tools.GetComponentInChildByPath<Avtoar2D>(gameObject, "model");
        switch (dir)
        {
            case EDirection.Up:
                av2d.animState = EAnimState.WalkB;
                break;
            case EDirection.Down:
                av2d.animState = EAnimState.WalkF;
                break;
            case EDirection.Left:
                av2d.animState = EAnimState.WalkL;
                break;
            case EDirection.Right:
                av2d.animState = EAnimState.WalkR;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 初始属性
    /// </summary>
    public void InitBaseProp()
    {
        atkAnimTimeBeforeBase = 0.5f;
        atkAnimTimeAfterBase = 0.5f;

        _Prop.BaseWeaponIAS = IConst.BaseIAS;
        _Prop.DeadlyStrike = IConst.BaseDS;
        _Prop.Strength = IConst.BASE_STR;
        _Prop.Agility = IConst.BASE_AGI;
        _Prop.Tenacity = IConst.BASE_TEN;
        _Prop.Stamina = IConst.BASE_STA;
        _Prop.PowerSpeed = IConst.BASE_POWERSPEED;
        _Prop.MoveSpeedBase = IConst.BASE_MOVESPEED;
    }

    public void InitWeapon()
    {
    
    }

    /// <summary>
    /// 初始化战术技能
    /// </summary>
    public void InitTacticsSkill()
    {
        skillsTractics = new ISkill[skillIdsTractics.Length];
        for (int i = 0; i < skillIdsTractics.Length; i++)
        {
            int skillId = skillIdsTractics[i];
            skillsTractics[i] = CreateSkillById(skillId, 1);
        }
    }

    /// <summary>
    /// 直接将玩家放到某个位置。
    /// </summary>
    /// <param name="mg"></param>
    public void SetMapGrid(MapGrid mg)
    {
        MapGrid mgCur = GetCurMapGrid();
        if (mgCur != null)
        {
            OnLeaveAGrid(mgCur);
        }
        _CurGridid = mg.g_Id;
        transform.position = mg.GetPos();
        transform.parent = mg.transform;
    }

    #region Game Mehtods

    public ISkill CreateSkillById(int skillId, int level)
    {
		ISkill r = null;
		switch (skillId) {
		case 1:{
			r = gameObject.AddComponent<Wqzw>();
            r._Level = level;
            r.Init();
		}
			break;
		case 2:{
			r = gameObject.AddComponent<MortalStrike>();
            r._Level = level;
            r.Init();
		}
			break;
        case 3:
            {
                r = gameObject.AddComponent<Sneer>();
                r._Level = 1;
                r.Init();
            }
            break;
        case 4:
            {
                r = gameObject.AddComponent<NuJi>();
                r._Level = level;
                r.Init();
            }
            break;
        case 5:
            {
                r = gameObject.AddComponent<HuoYanWuQi>();
                r._Level = level;
                r.Init();
            }
            break;
        case 6:
            {
                r = gameObject.AddComponent<DuoShan>();
                r._Level = level;
                r.Init();
            }
            break;
		default:
		break;
		}
		return r;
	}

    /// <summary>
    /// 设置技能等级
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="level"></param>
    public void SetBattleSkillLevel(int skillId, int level) 
    {
        ISkill skill = GetSkillHasAllot(skillId);
        if (skill != null)
        {
            // 已分配
            if (level > 0)
            {
                skill._Level = level;
            }
            else
            {
                // 降级至0.移除技能分配
                skill.RemoveEff();
                DestroyObject(skill);
            }
        }
        else
        {
            // 未分配
            if (level > 0)
            {
                // 新配技能
                ISkill skillNew = CreateSkillById(skillId, level);
                skillNew.StartEff();
                for (int i = 0; i < mSkills.Length; i++)
                {
                    if (mSkills[i] == null)
                    {
                        mSkills[i] = skillNew;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取已分配技能等级
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public int GetSkillLevel(int skillId) 
    {
        int level = 0;
        ISkill skill = GetSkillHasAllot(skillId);
        if (skill != null)
        {
            level = skill._Level;
        }
        return level;
    }
	#endregion
	
	#region InteractiveEventHandle
	#endregion
	
	void Killed(){
		
	}

    //public IEnumerator CoOnMove(EDirection dir) 
    //{
    //    MapGrid mgNext = GetCurMapGrid().GetNextGrid(dir);
    //    if (mgNext != null && mgNext.GetItemGobj() == null)
    //    {
    //        OnLeaveAGrid(GetCurMapGrid());
    //        SetPlayerDirection(dir);
    //        if (mgNext.GetItemGobj() == null)
    //        {
    //            GameManager.gameView.EnableInput = false;
    //            StartCoroutine(CoMoveToAGrid(mgNext));
    //            yield return new WaitForSeconds(0.2f);
    //            GameManager.gameView.EnableInput = true;
    //            _CurGridid = mgNext.g_Id;
    //            // 每次移动消耗怒气
    //            _Mp -= 10;
    //            UIManager._Instance.uiMain.RefreshHeroMP();

    //            // 回合+1
    //            OnIntoAGrid(GetCurMapGrid());
    //        }
    //    }
    //}

    public int GetSavedLvel()
    {
        int level = 1;
        if (PlayerPrefs.HasKey(IConst.KEY_LEVEL))
        {
            level = PlayerPrefs.GetInt(IConst.KEY_LEVEL);
        }
        return level;
    }

    /// <summary>
    /// 正在被敌人注意
    /// </summary>
    /// <returns></returns>
    public bool HasAlternessEnermy() 
    {
        return enermysInAlterness.Count > 0;
    }

    public int GetSavedExpCurLevel()
    {
        int exp = 0;
        if (PlayerPrefs.HasKey(IConst.KEY_EXP_CURLEVEL))
        {
            exp = PlayerPrefs.GetInt(IConst.KEY_EXP_CURLEVEL);
        }
        return exp;
    }

    public void SaveLevel()
    {
        PlayerPrefs.SetInt(IConst.KEY_LEVEL, level);
    }

    public void SaveExuCurLevel()
    {
        PlayerPrefs.SetInt(IConst.KEY_EXP_CURLEVEL, expCurLevel);
    }

    public void SetPlayerDirection(EDirection dir)
    {
        //float angy = 0f;
        //switch (dir)
        //{
        //    case EDirection.Up:
        //        angy = 0f;
        //        break;
        //    case EDirection.Down:
        //        angy = 180f;
        //        break;
        //    case EDirection.Left:
        //        angy = -90f;
        //        break;
        //    case EDirection.Right:
        //        angy = 90f;
        //        break;
        //    default:
        //        break;
        //}
        //transform.eulerAngles = new Vector3(0f, angy, 0f);
    }

    void PlayAnimAttack()
    {
        
    }

    void PlayAnimReady()
    {
       
    }

	// 不停自动攻击目标，直到目标死亡
	IEnumerator CoStartAttackCurTarget(){
		while(curTarget._State != EActorState.Dead){
            if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
            {
                break;
            }
			if(IsSkilling){
				yield return 1;
				continue;
			}
            PlayAnimAttack();

            UIManager.Inst.uiMain._AtkBar.ReStart();
            
            OnStartAAttack(curTarget);
			// 攻击前摇
            yield return 1;
            UIManager.Inst.uiMain._AtkBar.ToAtkPoint(AtkAnimTimeBefore);
            yield return new WaitForSeconds(AtkAnimTimeBefore);

            if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
            {
                UIManager.Inst.uiMain._AtkBar.ReStart();
                break;
            }

			if(IsSkilling){
				yield return 1;
				continue;
			}
			
			if(_State == EActorState.Dead){
                UIManager.Inst.uiMain._AtkBar.ReStart();
				break;
			}

            // 攻击特效
            GameManager.commonCPU.CreateEffect(GetAtkEffName(), curTarget.transform.position, Color.white, -1f);

            if (CheckHitTarget(curTarget))
            {
                int atk = _Prop.Atk;
                // 攻击伤害
                OnAttackHit(curTarget, atk);
                curTarget.OnAttackedHit(this, atk);
                DamageTarget(atk, curTarget);
                DamageTarget(_Prop.AtkFire, curTarget, EDamageType.Fire);
                DamageTarget(_Prop.AtkThunder, curTarget, EDamageType.Lighting);
                DamageTarget(_Prop.AtkPoison, curTarget, EDamageType.Poison);
                DamageTarget(_Prop.AtkIce, curTarget, EDamageType.Forzen);
            }
            else
            {
                // 攻击被躲闪
                UIManager.Inst.ShowTargetBattleStateInfo("闪避");
                OnAttackLost(curTarget);
                curTarget.OnAttackedLost(this);
            }
		
			// 攻击后摇
            yield return 1;
            UIManager.Inst.uiMain._AtkBar.ToAfterPoint(AtkAnimTimeAfter);
			yield return new WaitForSeconds(AtkAnimTimeAfter);
            if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
            {
                UIManager.Inst.uiMain._AtkBar.ReStart();
                break;
            }

			if(IsSkilling){
				yield return 1;
				continue;
			}
            
            PlayAnimReady();
			// 攻击间隔
            yield return 1;
            UIManager.Inst.uiMain._AtkBar.ToEnd(AtkTimeInterval);
			yield return new WaitForSeconds(AtkTimeInterval);
		}
	}

    int lastAtkHand = 0;// 0主手，1副手
    /// <summary>
    /// 获取攻击特效名称
    /// </summary>
    /// <returns></returns>
    string GetAtkEffName() 
    {
        string effName = "";
        int atkhand = 0;
        int handType = 1;   // 1单手，2双手
        EquipItem eiHand1 = GameManager.gameView.GetEquipItemHasEquip(this, EEquipPart.Hand1);
        EquipItem eiHand2 = GameManager.gameView.GetEquipItemHasEquip(this, EEquipPart.Hand2);
        if (eiHand1 != null && eiHand1.baseData.type == EEquipItemType.WeaponOneHand && eiHand2 != null && eiHand2.baseData.type == EEquipItemType.WeaponOneHand)
        {
            // 双持单手
            atkhand = (lastAtkHand == 0 ? 1 : 0);
            lastAtkHand = atkhand;
            handType = 1;
        }
        else if (eiHand1 != null && eiHand1.baseData.type == EEquipItemType.WeaponOneHand)
        {
            // 单持主手
            atkhand = 0;
            lastAtkHand = atkhand;
            handType = 1;
        }
        else if (eiHand2 != null && eiHand2.baseData.type == EEquipItemType.WeaponOneHand)
        {
            // 单持副手
            atkhand = 1;
            lastAtkHand = atkhand;
            handType = 1;
        }
        else if (eiHand1 != null && eiHand1.baseData.type == EEquipItemType.WeaponTwoHand)
        {
            // 双手
            atkhand = (lastAtkHand == 0 ? 1 : 0);
            lastAtkHand = atkhand;
            handType = 2;
        }

        if (handType == 1)
        {
            if (atkhand == 0)
            {
                effName = "eff_hand_one_2";
            }
            else 
            {
                effName = "eff_hand_one_1";
            }
        }
        else if (handType == 2)
        {
            if (atkhand == 0)
            {
                effName = "eff_hand_two_2";
            }
            else
            {
                effName = "eff_hand_two_1";
            }
        }

        return effName;
    }

	public void RecoverHp(int hp){
		this._Prop.Hp += hp;
        if (_Prop.Hp > _Prop.HpMax)
        {
            _Prop.Hp = _Prop.HpMax;
		}
		UIManager.Inst.uiMain.RefreshHeroHP();
        GameManager.gameView.UIShowHeal(hp);
	}
	
    /// <summary>
    /// 消耗怒气
    /// </summary>
    /// <param name="engReduce"></param>
	public void ReduceEng(int engReduce){
        _Prop.Mp -= engReduce;
		if(_Prop.Mp < 0){
            _Prop.Mp = 0;
		}
        UIManager.Inst.uiMain.RefreshHeroMP();
        //GameManager.gameView.UpdateUIHeroEng(this);
	}

    //public void MoveTo(Vector3 pos, int nextgridid)
    //{
    //    PlayAnim("Run");
    //    WeaponToHand();
    //    iTween.MoveTo(gameObject, iTween.Hash("x", pos.x, "z", pos.z, "time", 0.3f, "oncomplete", "OnMoveEnd", "easetype", iTween.EaseType.linear, "oncompletetarget", gameObject, "oncompleteparams", nextgridid));
    //}

    //void OnMoveEnd(int nextid)
    //{
    //    CurGridid = nextid;
    //    PlayAnim("Stand");
    //    WeaponToBack();
    //    //if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
    //    //{
           
    //    //}
    //    GameManager.gameView.OnMoveEnd();
    //}

	#region buff
	#endregion

    public override void OnTryToAGrid(MapGrid mgTo)
    {
        // 警觉敌人跟随
        // 如果目标格子不在所有警觉敌人攻击范围
        //for (int i = 0; i < enermysInAlterness.Count; i++)
        //{
        //    Enermy enermy = enermysInAlterness[i];
        //    MapGrid mgEnermy = enermy.GetCurMapGrid();
        //    bool isToBattle = false;
        //    if (mgTo.IsNear(mgEnermy))
        //    {
        //        isToBattle = true;
        //    }

        //    if (!isToBattle)
        //    {
        //        // 获取目标格子
        //        MapGrid mgEnermyTo = null;
        //        List<MapGrid> mgs = mgTo.GetCornerGrids();
        //        for (int j = 0; j < mgs.Count; j++)
        //        {
        //            MapGrid mgCorner = mgs[j];
        //            if (mgCorner.IsNear(mgEnermy))
        //            {
        //                mgEnermyTo = mgCorner;
        //                break;
        //            }
        //        }

        //        // 如果目标格子可以行走
        //        if (mgEnermyTo.GetItemGobj() == null)
        //        {
        //            enermy.MoveToAGrid(mgEnermyTo);
        //        }
        //    }
        //}
    }

    public override void OnIntoAGrid(MapGrid grid)
    {
        //GameManager.gameView._RoundLogicState = GameRoundLogicState.WorldEventAction;

        //TODO 玩家进入格子事件

        //List<MapGrid> mgsCorner = grid.GetCornerGrids();
        ////警觉脱离检测
        //List<Enermy> enermysToRemove = new List<Enermy>();
        //for (int i = 0; i < enermysInAlterness.Count; i++)
        //{
        //    Enermy enermy = enermysInAlterness[i];
        //    // 如果敌人不在4个角，则脱离警觉
        //    bool inCorner = false;
        //    for (int j = 0; j < mgsCorner.Count; j++)
        //    {
        //        MapGrid mgTemp = mgsCorner[j];
        //        if (enermy._CurGridid == mgTemp.g_Id)
        //        {
        //            inCorner = true;
        //            break;
        //        }
        //    }
        //    if (!inCorner || enermy._State != EActorState.Normal)
        //    {
        //        enermysToRemove.Add(enermy);
        //    }
        //}
        //for (int i = 0; i < enermysToRemove.Count; i++)
        //{
        //    Enermy enermyRemove = enermysToRemove[i];
        //    RemoveFormAlterness(enermyRemove);
        //    UIManager._Instance.ShowFloatTip(enermyRemove._MonsterBD.name + "不再注意你");
        //}

        //// 敌人警觉检查

        //for (int i = 0; i < mgsCorner.Count; i++)
        //{
        //    MapGrid mg = mgsCorner[i];
        //    Enermy enermy = mg.GetItem<Enermy>();
        //    if (enermy != null)
        //    {
        //        if (!enermysInAlterness.Contains(enermy))
        //        {
        //            enermysInAlterness.Add(enermy);
        //            UIManager._Instance.ShowFloatTip(enermy._MonsterBD.name + "注意到你了");
        //            // 停止行走
        //            _State = EActorState.Normal;
        //        }
        //    }
        //}

        //// 设置战斗敌人
        //SetTargetEnermys(grid);

        //if (targetEnermys.Count > 0)
        //{
        //    StartBattle();
        //}
        //else
        //{
        //    CheckChangeMap();
        //    GameManager.gameView.OnRoundEnd();
        //}
    }

    /// <summary>
    /// 设置战斗敌人
    /// </summary>
    public void SetTargetEnermys(MapGrid curGrid)
    {
        targetEnermys.Clear();
        List<MapGrid> mgs = curGrid.GetNearGrids();
        for (int i = 0; i < mgs.Count; i++)
        {
            MapGrid itemMg = mgs[i];
            Enermy enermy = itemMg.GetItem<Enermy>();
            if (enermy != null)
            {
                targetEnermys.Add(enermy);
            }
        }

        // 如果目标不足4个，添加警觉中的敌人，直到满4个
        int indexAlterness = 0;
        while (targetEnermys.Count > 0 && targetEnermys.Count < 4)
        {
            // 取一个警觉中的敌人
            if (indexAlterness < enermysInAlterness.Count)
            {
                Enermy enermyInAlterness = enermysInAlterness[indexAlterness];
                targetEnermys.Add(enermyInAlterness);
                indexAlterness++;
            }
            else 
            {
                break;
            }
            
        }
    }

    public List<Enermy> GetBattleTargets()
    {
        return targetEnermys;
    }

    public void AddAEnermyToBattle(Enermy enermy)
    {
        targetEnermys.Add(enermy);
    }

    public void ClearBattleTargets()
    {
        targetEnermys.Clear();
    }

    public override void OnLeaveAGrid(MapGrid grid)
    {
        UIManager.Inst.CloseUIChangeMapTip();
    }

    /// <summary>
    /// 检测是否处于地图入出口
    /// </summary>
    public bool CheckChangeMap()
    {
        bool isInChange = false;
        MapGrid mg = GetCurMapGrid();
        if (mg != null)
        {
            if (mg.Type == EGridType.ChangeMap)
            {
                GameMapBaseData mapTarget = GameDatas.GetGameMapBD(mg._ToMapId);
                if (mapTarget != null)
                {
                    isInChange = true;
                    UIManager.Inst.ShowUIChangeMapTip(mapTarget, mg._ToMapTargetGrid);
                }
            }
            else if (mg.Type == EGridType.StartAndToHome)
            {
                GameMapBaseData mapTarget = GameDatas.GetGameMapBD(GameManager.commonCPU.ReadCurHomeMap());
                if (mapTarget != null)
                {
                    isInChange = true;
                    UIManager.Inst.ShowUIChangeMapTip(mapTarget, -1);
                }
            }
        }
        return isInChange;
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    public void StartBattle()
    {
        StartCoroutine(GameManager.gameView.CoOnEnterBattle(targetEnermys));
    }

    public void SetAttackTarget(Enermy target)
    {
        curTarget = target;
        // 朝向目标
        // UI目标效果
        UIManager.Inst.uiMain.SetTarget(target);
    }

    /// <summary>
    /// 获取下一个未死亡目标
    /// </summary>
    /// <returns></returns>
    public Enermy GetNextTarget()
    {
        Enermy target = null;
        for (int i = 0; i < targetEnermys.Count; i++)
        {
            Enermy enermy = targetEnermys[i];
            if (enermy._State != EActorState.Dead)
            {
                target = enermy;
            }
        }
        return target;
    }

    /// <summary>
    /// 是否有需要战斗的敌人
    /// </summary>
    /// <returns></returns>
    public bool HasEnermyToBattle()
    {
        return targetEnermys.Count > 0;
    }

    public override void OnAttackHit(IActor target, int atkOri)
    {
        base.OnAttackHit(target, atkOri);
        // 获得10点怒气
        _Prop.Mp += 10;
        UIManager.Inst.uiMain.RefreshHeroMP();
    }

    public override void OnHurted(int damage, EDamageType damagetype, IActor target, bool isDS)
    {
        base.OnHurted(damage, damagetype, target, isDS);
        // 获得伤害10%怒气
        _Prop.Mp += (int)(damage * 0.1f);
        UIManager.Inst.uiMain.RefreshHeroMP();

        bsManager.ActionUnContol(2f);
    }

#region 装备
    /// <summary>
    /// 取背包里指定类型的装备
    /// </summary>
    /// <returns></returns>
    public EquipItem GetEquipItemInBagById(int id)
    {
        EquipItem ei = null;
        for (int i = 0; i < itemsInBag.Count; i++)
        {
            EquipItem eiItem = itemsInBag[i];
            if (eiItem.baseData.id == id)
            {
                ei = eiItem;
                break;
            }
        }
        return ei;
    }
#endregion

    public void SetItemUsed(int index, int eiId) 
    {
        arrItemUesd[index] = eiId;
    }
}
