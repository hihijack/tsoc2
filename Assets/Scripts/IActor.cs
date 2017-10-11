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
    [System.Obsolete]
    public void SetAnimRateByIAS()
    {
        // 根据动画时间和攻速设置实际时间
        float atkTimeOri = atkAnimTimeBeforeBase + atkAnimTimeAfterBase;
        float atkTime = 1 / Prop.IAS;

        if (atkTime >= atkTimeOri)
        {
            // 填补攻击间隔时间
            atkTimeInterval = atkTime - atkTimeOri;
            animRate = 1;
            atkAnimTimeBefore = atkAnimTimeBeforeBase;
            atkAnimTimeAfter = atkAnimTimeAfterBase;
        }
        else if (atkTime < atkTimeOri)
        {
            // 压缩动画时间
            float rate = atkTime / atkTimeOri;
            atkTimeInterval = 0f;
            animRate = 1 / rate;
            atkAnimTimeBefore = atkAnimTimeBeforeBase * rate;
            atkAnimTimeAfter = atkAnimTimeAfterBase * rate;
        }
    }



    public float atkAnimTimeBeforeBase; // 基础攻击前摇时间
    public float atkAnimTimeAfterBase; //  基础攻击后摇时间

    private float atkAnimTimeBefore; // 攻击前摇时间
    private float atkAnimTimeAfter; //  攻击后摇时间
    private float atkTimeInterval; // 攻击间隔时间

    public List<ESpecBuffState> buffStates = new List<ESpecBuffState>();
    #endregion

    #region 属性GeterAndSeter
    public EBattleState _BattleState
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

    public float AtkAnimTimeBefore
    {
        get
        {
            return atkAnimTimeBefore;
        }

        set
        {
            atkAnimTimeBefore = value;
        }
    }

    public float AtkAnimTimeAfter
    {
        get
        {
            return atkAnimTimeAfter;
        }

        set
        {
            atkAnimTimeAfter = value;
        }
    }

    public float AtkTimeInterval
    {
        get
        {
            return atkTimeInterval;
        }

        set
        {
            atkTimeInterval = value;
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

    public int GetDamgerToOther(int damageOri, IActor other, EDamageType damageType = EDamageType.Phy) {
        int damage = 0;
        int atk = damageOri;

        int armOther = 0;
        switch (damageType)
        {
            case EDamageType.Phy:
                armOther = other.Prop.Arm;
                break;
            case EDamageType.Fire:
                armOther = other.Prop.ResFire;
                break;
            case EDamageType.Lighting:
                armOther = other.Prop.ResThunder;
                break;
            case EDamageType.Poison:
                armOther = other.Prop.ResPoision;
                break;
            case EDamageType.Forzen:
                armOther = other.Prop.ResForzen;
                break;
            default:
                break;
        }


        if (armOther >= 0)
        {
            int damgeOffset = (int)(atk * (armOther * 0.03 / (armOther * 0.03f + 1)));
            damage = atk - damgeOffset;
        }
        else
        {
            int N = Mathf.Abs(armOther);
            float damgeAddPercent = 2 - Mathf.Pow(0.94f, N);
            damage = (int)(atk * damgeAddPercent);
        }

        return damage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageOri"></param>
    /// <param name="target"></param>
    /// <param name="damageType">伤害类型</param>
    /// <param name="canDS">能否造成致命一击</param>
    public void DamageTarget(int damageOri, IActor target, EDamageType damageType = EDamageType.Phy, bool canDS = true) {

        if (damageOri <= 0 || target._State == EActorState.Dead)
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
            return;
        }

        //蓄力
        if (_PowerVal >= 1 && _PowerVal < 2)
        {
            //一段蓄力
            damageOri = Mathf.CeilToInt(IConst.Power1DamPer * damageOri);
        }
        else if (_PowerVal >= 2 && _PowerVal < 3)
        {
            //二段蓄力
            damageOri = Mathf.CeilToInt(IConst.Power2DamPer * damageOri);
        }
        else if (_PowerVal >= 3)
        {
            //三段蓄力
            damageOri = Mathf.CeilToInt(IConst.Power3DamPer * damageOri);
        }

        // 格挡
        bool isParry = false;
        int damParry = 0;
        float parryPercent = target.Prop.ParryDamPercent;
        if (target._BattleState == EBattleState.Defing)
        {
            if (target.isHero)
            {
                int vigorCost = target.Prop.VigorMax;
                if (target.Prop.ParryDmgVigor > 0)
                {
                    vigorCost = Mathf.CeilToInt(damageOri / target.Prop.ParryDmgVigor);
                }

                if (target.Prop.Vigor < vigorCost)
                {
                    //精力不足于完全格挡
                    parryPercent = target.Prop.Vigor / vigorCost;
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
           

            damParry = Mathf.RoundToInt(damageOri * parryPercent);
            //格挡百分百伤害
            target.OnParry(damParry, damageOri);
            damageOri -= damParry;
            isParry = true;
        }
        
        int damage = GetDamgerToOther(damageOri, target, damageType);
        bool isDs = false;// 是否致命一击
                          // 致命一击
        if (canDS && Tools.IsHitOdds(Prop.DeadlyStrike)) {
            damage = (int)(damage * Prop.DeadlyStrikeDamage);
            isDs = true;
        }

        //坚韧
        damage = Mathf.CeilToInt(damage * (1 - Prop.DamReduce));

        target.OnHurted(damage, damageType, this, isDs);
        OnDamageTarget(damage, target, isDs);

        int oriTargetHP = target.Prop.Hp;

        target.Prop.Hp -= damage;
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
                //curTarget.updataState(EFSMAction.NPC_DIE);
                //updataState(EFSMAction.HERO_RUN);
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
        if (!target.isHero) {
            UIManager.Inst.uiMain.RefreshTargetHP(target as Enermy);

            if (isDs)
            {
                string strParryDesc = "";
                if (isParry)
                {
                    strParryDesc = string.Format("(格挡{0})", damParry);
                }
                UIManager.Inst.ShowDSDamageTxt(damage.ToString() + strParryDesc);
            }
            else
            {
                UIManager.Inst.ShowDamageTxt(damage, damageType, damParry);
            }
        }
        else
        {
            UIManager.Inst.uiMain.RefreshHeroHP();
            UIManager.Inst.ShowHurtedTxt(damage, damageType, damParry);
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

    public void Hurted(int damageOri) {
        int damage = GetDamgerToOther(damageOri, this);

        Prop.Hp -= damage;
        if (Prop.Hp <= 0) {
            Prop.Hp = 0;
        }
        if (isHero) {
            UIManager.Inst.uiMain.RefreshHeroHP();
            GameManager.gameView.UIShowHurt(damage);
        }
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

    //当一次普通攻击命中
    public virtual void OnAttackHit(IActor target, int atkOri) { }

    /// <summary>
    /// 当受到一次普通攻击
    /// </summary>
    /// <param name="atker"></param>
    /// <param name="atkOri"></param>
    public virtual void OnAttackedHit(IActor atker, int atkOri) { }

    // 当受到伤害
    public virtual void OnHurted(int damage, EDamageType type, IActor target, bool isDS) { }

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
    public virtual void OnDamageTarget(int damage, IActor target, bool isDS) { }

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

    public virtual void OnMoveEnd() { }


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
        StartCoroutine(CoMoveToAGrid(mgNext));
    }

    public IEnumerator CoMoveToAGrid(MapGrid mgNext)
    {
        float timePerGrid = 0.15f;

        _State = EActorState.Move;

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

            GameManager.gameView._RoundLogicState = GameRoundLogicState.HeroAction;

            OnLeaveAGrid(mgCur);

            OnTryToAGrid(mgNext);

            iTween.MoveTo(gameObject, iTween.Hash("position", mgNext.transform.position, "time", timePerGrid, "easetype", iTween.EaseType.linear));

            yield return new WaitForSeconds(timePerGrid);

            // 每次移动消耗怒气
            //if (isHero)
            //{
            //    _Prop.EnergyPoint--;
            //    UIManager._Instance.uiMain.RefreshHeroMP();
            //}

            transform.parent = GetCurMapGrid().transform;

            if (isHero)
            {
                GameView.Inst.PlayerActionEnd();
            }
            
        }

        if (GameManager.gameView._RoundLogicState != GameRoundLogicState.Battle)
        {
            _State = EActorState.Normal;
        }
    }

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
