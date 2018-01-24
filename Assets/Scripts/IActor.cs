using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum EActorState
{
    Normal, // 普通状态
    Battle, // 战斗状态
    Move,    // 移动状态，
    Dead        // 死亡
}

public enum EBattleState
{
    Normal, //自由状态
    AtkBefore,//攻击前摇中
    AtkAfter,//攻击后摇中
    Defing,//防御中
    Powering,//蓄力中
    Uncontrol,//无法控制
    Dodge, //闪避中
}

public enum EBattleStateNPC
{
    Idle, //静止状态
    AtkBefore, //攻击状态
    AtkAfter,
    Uncontrol,//硬直
    Dodge, //闪避
    Defing, //防御
}

public enum EAIState
{
    Normal, //休闲状态
    FindTarget, //发现目标
    Battle,
}

/// <summary>
/// 特殊buff状态
/// </summary>
public enum ESpecBuffState
{
    Hiding
}

public class IActor : MonoBehaviour {
    public string guid;

    protected EActorState state;

    EBattleState m_BattleState = EBattleState.Normal;

    public virtual EActorState _State
    {
        get { return state; }
        set { state = value; }
    }

    int curGridid = -1;
    public int _CurGridid
    {
        get { return curGridid; }
        set
        {
            curGridid = value;
        }
    }
    #region 属性
    private PropertyBase propertyHander;

    //等级
    public int level;
    public int tlMax;   // 体力上限

    //================================================================
    public int atkMag;  // 魔法攻击力

    public float animRate;

    // 动画时间1秒。攻速不会超过1
    //public virtual void SetAnimRateByIAS()
    //{
        // 根据动画时间和攻速设置实际时间
        //float atkTimeOri = atkAnimTimeBeforeBase + atkAnimTimeAfterBase;
        //float atkTime = 1 / Prop.IAS;

        //if (atkTime >= atkTimeOri)
        //{
        //    // 填补攻击间隔时间
        //    atkTimeInterval = atkTime - atkTimeOri;
        //    animRate = 1;
        //    atkAnimTimeBefore = atkAnimTimeBeforeBase;
        //    atkAnimTimeAfter = atkAnimTimeAfterBase;
        //}
        //else if (atkTime < atkTimeOri)
        //{
        //    // 压缩动画时间
        //    float rate = atkTime / atkTimeOri;
        //    atkTimeInterval = 0f;
        //    animRate = 1 / rate;
        //    atkAnimTimeBefore = atkAnimTimeBeforeBase * rate;
        //    atkAnimTimeAfter = atkAnimTimeAfterBase * rate;
        //}
    //}

    public float atkAnimTimeBeforeBase; // 基础攻击前摇时间
    public float atkAnimTimeAfterBase; //  基础攻击后摇时间

    public List<ESpecBuffState> buffStates = new List<ESpecBuffState>();
    #endregion

    #region 属性GeterAndSeter
    public virtual EBattleState _BattleState
    {
        get
        {
            return m_BattleState;
        }

        set
        {
            m_BattleState = value;
        }
    }
    public float _PowerVal
    {
        get
        {
            return powerVal;
        }

        set
        {
            powerVal = value;
            if (powerVal > 3)
            {
                powerVal = 3;
            }
        }
    }
    #endregion

   

    public string actorName;

    protected bool _isSkilling = false;

    public bool isHero;

    public bool IsSkilling {
        get {
            return _isSkilling;
        }
        set {
            _isSkilling = value;
        }
    }

    public PropertyBase Prop
    {
        get
        {
            return propertyHander;
        }

        set
        {
            propertyHander = value;
        }
    }



    /// <summary>
    /// 蓄力值[0,3]
    /// </summary>
    float powerVal;

    virtual public int GetAtkPhy()
    {
        return 0;
    }

    //protected Animation anim;

    void Awake()
    {
        //anim = Tools.GetComponentInChildByPath<Animation>(gameObject, "model");
    }

    public void Start()
    {
    }

    //public void AddEng(int engAdd)
    //{
    //    _Mp += engAdd;
    //    if (_Mp > mpMax)
    //    {
    //        _Mp = mpMax;
    //    }
    //    if (_Mp < 0)
    //    {
    //        _Mp = 0;
    //    }
    //}

    internal T GetBuff<T>() where T : IBaseBuff
    {
        T r = null;
        r = GetComponent<T>();
        return r;
    }

    internal T AddBuff<T>() where T : IBaseBuff
    {
        T r = null;
        r = gameObject.AddComponent<T>();
        return r;
    }

    internal bool HasBuff<T>() where T : IBaseBuff
    {
        bool hasBuff = false;
        if (GetComponent<T>() != null)
        {
            hasBuff = true;
        }
        return hasBuff;
    }

    public int GetDamgerToOther(DmgData dmgData, IActor other) {
        int damage = 0;
        dmgData.ApplyRes(other.Prop.Arm, other.Prop.ResFire, other.Prop.ResThunder, other.Prop.ResPoision, other.Prop.ResForzen);
        damage = dmgData.TotalDmg();
        return damage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageOri"></param>
    /// <param name="target"></param>
    /// <param name="damageType">伤害类型</param>
    /// <param name="canDS">能否造成致命一击</param>
    /// <param name="dp">削韧</param>
    /// <param name="force">冲击力</param>
    public void DamageTarget(IActor target, DmgData dmgData) {

        if (!dmgData.HasDmg() || target._State == EActorState.Dead)
        {
            return;
        }

        //闪避
        if (target._BattleState == EBattleState.Dodge)
        {
            if (target.isHero)
            {
                UIManager.Inst.ShowDodgeTip();
            }
            else
            {
                UIManager.Inst.ShowEnermyDodgeTip();
            }
            OnAttackLost(target);
            return;
        }

        // 格挡
        bool isParry = false;
        int damParry = 0;
        float parryPercentPhy = target.Prop.ParryDamPercent(EDamageType.Phy);
        float parryPercentFire = target.Prop.ParryDamPercent(EDamageType.Fire);
        float parryPercentLighting = target.Prop.ParryDamPercent(EDamageType.Lighting);
        float parryPercentPoison = target.Prop.ParryDamPercent(EDamageType.Poison);
        float parryPercentFrozen = target.Prop.ParryDamPercent(EDamageType.Frozen);
        if (target._BattleState == EBattleState.Defing)
        {
            if (target.isHero)
            {
                int vigorCost = target.Prop.VigorMax;
                if (target.Prop.ParryDmgVigor > 0)
                {
                    vigorCost = Mathf.CeilToInt(dmgData.TotalDmg() / target.Prop.ParryDmgVigor);
                }

                if (target.Prop.Vigor < vigorCost)
                {
                    //精力不足于完全格挡
                    float p = target.Prop.Vigor / vigorCost;
                    parryPercentPhy *= p;
                    parryPercentFire *= p;
                    parryPercentLighting *= p;
                    parryPercentPoison *= p;
                    parryPercentFrozen *= p;
                }

                //精力消耗
                target.Prop.Vigor -= vigorCost;
                UIManager.Inst.uiMain.RefreshHeroVigor();
                //格挡受击硬直
                if (target.Prop.Vigor == 0)
                {
                    Hero.Inst.BsManager.ActionUnContol(3f);
                }
            }
           

            damParry = Mathf.RoundToInt(dmgData.dmgPhy * parryPercentPhy) 
                + Mathf.RoundToInt(dmgData.dmgFire * parryPercentFire)
                + Mathf.RoundToInt(dmgData.dmgLighting * parryPercentLighting)
                + Mathf.RoundToInt(dmgData.dmgPoison * parryPercentPoison)
                + Mathf.RoundToInt(dmgData.dmgForzen * parryPercentFrozen);
            //格挡百分百伤害
            target.OnParry(damParry, dmgData.TotalDmg());
            //应用格挡
            dmgData.ApplyParry(parryPercentPhy, parryPercentFire, parryPercentLighting, parryPercentPoison, parryPercentFrozen);
            isParry = true;
        }

        bool isDs = false;// 是否致命一击
        // 致命一击
        if (dmgData.enableDS && Tools.IsHitOdds(Prop.DeadlyStrike)) {
            dmgData.ApplyDS(Prop.DeadlyStrikeDamage);
            isDs = true;
        }

        //抗性计算
        dmgData.ApplyRes(target.Prop.Arm, target.Prop.ResFire, target.Prop.ResThunder, target.Prop.ResPoision, target.Prop.ResForzen);
        
        //伤害减免
        //damage = Mathf.CeilToInt(damage * (1 - Prop.DamReduce));

        target.OnHurted(this, dmgData);
        OnDamageTarget(target, dmgData);

        int oriTargetHP = target.Prop.Hp;

        int totalDmg = dmgData.TotalDmg();
        target.Prop.Hp -= totalDmg;
        if (target.Prop.Hp <= 0) {
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
                if (GameManager.gameView._RoundLogicState == GameRoundLogicState.Battle)
                {
                    GameManager.gameView.OnKillCurTarget();
                }
            }
        }
        else
        {
            target.OnHPChange(oriTargetHP, target.Prop.Hp);
        }
        if (!target.isHero)
        {
            //玩家攻击怪物
            UIManager.Inst.uiMain.RefreshTargetHP(target as Enermy);

            if (isDs)
            {
                string strParryDesc = "";
                if (isParry)
                {
                    strParryDesc = string.Format("(格挡{0})", damParry);
                }
                UIManager.Inst.ShowDSDamageTxt(totalDmg.ToString() + strParryDesc);
            }
            else
            {
                UIManager.Inst.ShowDamageTxt(totalDmg, dmgData.GetEleDmgType(), damParry);
            }
        }
        else
        {
            UIManager.Inst.uiMain.RefreshHeroHP();
            UIManager.Inst.ShowHurtedTxt(totalDmg, dmgData.GetEleDmgType(), damParry);
        }
    }

    /// <summary>
    /// 攻击目标时检测是否命中
    /// </summary>
    /// <returns></returns>
    protected bool CheckHitTarget(IActor target)
    {
        //bool ishit = false;
        //if ((hit + target.dodge) != 0)
        //{
        //    if (Tools.IsHitOdds((float)hit / (hit + target.dodge)))
        //    {
        //        ishit = true;
        //    }
        //}
        //else
        //{
        //    if (Tools.IsHitOdds(0.5f))
        //    {
        //        ishit = true;
        //    }
        //}
        //return ishit;
        return true;
    }

    public Vector3 GetPos() {
        return gameObject.transform.position;
    }


    #region 事件触发
    // 当一个单位进入范围
    public virtual void OnActorIntoRange(IActor other) { }

    // 当一个单位离开范围
    public virtual void OnActorLeaveRange(IActor other) { }

    //当进入一个格子
    public virtual void OnIntoAGrid(MapGrid grid)
    {

    }

    // 当离开一个格子
    public virtual void OnLeaveAGrid(MapGrid grid)
    {

    }

    /// <summary>
    /// 尝试前往一个格子
    /// </summary>
    /// <param name="mgTo"></param>
    public virtual void OnTryToAGrid(MapGrid mgTo) { }

    //当尝试对目标进行一次攻击
    public virtual void OnStartAAttack(IActor target) { }

    /// <summary>
    /// 当目标尝试对自己进行攻击
    /// </summary>
    /// <param name="atker"></param>
    public virtual void OnStartAtked(IActor atker) { }

    //当一次普通攻击命中
    public virtual void OnAttackHit(IActor target, int atkOri) { }

    /// <summary>
    /// 当受到一次普通攻击
    /// </summary>
    /// <param name="atker"></param>
    /// <param name="atkOri"></param>
    public virtual void OnAttackedHit(IActor atker, int atkOri) { }

    // 当受到伤害
    public virtual void OnHurted(IActor atker, DmgData dmgData) { }

    //当进入战斗
    public virtual void OnEnterBattle() { }

    //当死亡
    public virtual void OnDead() { }

    /// <summary>
    /// 当对目标进行一次伤害
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    /// <param name="isDS"></param>
    public virtual void OnDamageTarget(IActor target, DmgData dmgData) { }

    /// <summary>
    /// 当攻击被闪避
    /// </summary>
    /// <param name="target"></param>
    public virtual void OnAttackLost(IActor target) { }

    /// <summary>
    /// 当闪避一次攻击
    /// </summary>
    /// <param name="atker"></param>
    public virtual void OnAttackedLost(IActor atker) { }

    /// <summary>
    /// 当生命值变化
    /// </summary>
    /// <param name="valChange"></param>
    public virtual void OnHPChange(int valBefore, int valCur) { }

    /// <summary>
    /// 当格挡时
    /// </summary>
    /// <param name="damageParry"></param>
    public virtual void OnParry(int damageParry, int dmgOri) { }
    #endregion

    //public IEnumerator CoMoveToAGrid(MapGrid mg)
    //{
    //    float time = 0.1f;
    //    iTween.MoveTo(gameObject, iTween.Hash("position", mg.transform.position, "time", time, "orienttopath", true, "oncomplete", "OnMoveEnd", "oncompletetarget", gameObject));
    //    yield return new WaitForSeconds(time);
    //}

    public virtual void OnMoveEnd()
    {
        transform.parent = GetCurMapGrid().transform;
    }


    public void MoveByGrids(List<MapGrid> mgs, bool includeStart = true)
    {
        StartCoroutine(CoMoveByGrids(mgs, includeStart));
    }


    public virtual void SetDir(EDirection dir) { }

    /// <summary>
    /// 根据指定路径移动
    /// </summary>
    /// <param name="mgs"></param>
    /// includeStart: 该路径是否包含起点
    /// <returns></returns>
    public IEnumerator CoMoveByGrids(List<MapGrid> mgs, bool includeStart = true)
    {
        //Debug.LogError("move");//######

        float timePerGrid = 0.3f;

        _State = EActorState.Move;

        for (int i = 0; i < mgs.Count; i++)
        {
            if (i == 0 && includeStart)
            {
                continue;
            }
            MapGrid mgNext = mgs[i];
            if (mgNext.GetItemGobj() == null && _State == EActorState.Move)
            {
                MapGrid mgCur = GetCurMapGrid();
                if (isHero)
                {
                    // 设置方向
                    EDirection dir = mgCur.GetDirToOther(mgNext);
                    SetDir(dir);
                }
                // 立即设置格子id
                _CurGridid = mgNext.g_Id;

                OnLeaveAGrid(mgCur);

                OnTryToAGrid(mgNext);

                //Debug.LogError(Time.time + "start move");//########

                iTween.MoveTo(gameObject, iTween.Hash("position", mgNext.transform.position, "time", timePerGrid, "easetype", iTween.EaseType.linear));

                yield return new WaitForSeconds(timePerGrid);

                //Debug.LogError(Time.time + "end move");//########

                yield return 0;

                // 每次移动消耗怒气
                if (isHero)
                {
                    Prop.EnergyPoint--;
                    UIManager.Inst.uiMain.RefreshHeroVigor();
                }
                OnIntoAGrid(GetCurMapGrid());
            }
        }

        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            _State = EActorState.Normal;
        }

        yield return 0;

    }

    /// <summary>
    /// 移动向一个格子
    /// </summary>
    /// <param name="mgNext"></param>
    public void MoveToAGrid(MapGrid mgNext)
    {
        float timePerGrid = 0.15f;

        _State = EActorState.Move;

        if (_State == EActorState.Move)
        {
            MapGrid mgCur = GetCurMapGrid();
            if (isHero)
            {
                // 设置方向
                EDirection dir = mgCur.GetDirToOther(mgNext);
                SetDir(dir);
            }
            // 立即设置格子id
            _CurGridid = mgNext.g_Id;

            GameManager.gameView._RoundLogicState = GameRoundLogicState.HeroAction;

            OnLeaveAGrid(mgCur);

            OnTryToAGrid(mgNext);

            iTween.MoveTo(gameObject, iTween.Hash("position", mgNext.transform.position, "time", timePerGrid, "easetype", iTween.EaseType.linear, "oncomplete", "OnMoveEnd", "oncompletetarget", gameObject));

        }

        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            _State = EActorState.Normal;
        }
    }

    //public IEnumerator CoMoveToAGrid(MapGrid mgNext)
    //{
    
    //}

    public MapGrid GetCurMapGrid()
    {
        return  GameManager.gameView.GetMapGridById(_CurGridid);
    }

    public ISkill GetSkillHasAllot(int skillId)
    {
        ISkill skill = null;
        ISkill[] skills = GetComponents<ISkill>();
        for (int i = 0; i < skills.Length; i++)
        {
            ISkill temp = skills[i];
            if (temp.GetBaseData().id == skillId)
            {
                skill = temp;
                break;
            }
        }
        return skill;
    }

    public int GetSkillHasAllotLevel(int id)
    {
        int level = 0;
        ISkill skill = GetSkillHasAllot(id);
        if (skill != null)
        {
            level = skill._Level;
        }
        return level;
    }

    [ContextMenu("GenGUID")]
    public void GenGUID()
    {
        guid = Tools.GetGUID();
    }
}
