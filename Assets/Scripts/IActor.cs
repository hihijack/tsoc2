using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EActorState
{
    Normal, // 普通状态
    Battle, // 战斗状态
    Move,    // 移动状态，
    Dead        // 死亡
}

public class IActor : MonoBehaviour {
    public int id;

    protected EActorState state;

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
    /// <summary>
    /// 等级
    /// </summary>
    public int level;

    /// <summary>
    /// 当前生命值
    /// </summary>
    public int hp;   // 当前生命值

    /// <summary>
    /// 生命值上限
    /// </summary>
    private int hpMax;
    public int _HpMax
    {
        get { return hpMax; }
        set
        {
            hpMax = value;
            if (hp > hpMax)
            {
                hp = hpMax;
            }
        }
    }


    public int tl;      // 体力
    public int tlMax;   // 体力上限

    /// <summary>
    /// 能量
    /// </summary>
    private int mp; //魔法值
    public int _Mp
    {
        get { return mp; }
        set
        {
            mp = value;
            if (mp < 0)
            {
                mp = 0;
            }
        }
    }
    public int mpMax;

    //================================================================
    /// <summary>
    /// 物理攻击力
    /// </summary>
    public int atkPhy;//来自属性及装备攻击力
    int valAAtkPhy = 0;//增伤A。技能Buff增伤
    float valBAtkPhy = 1f;//增伤B。技能buff增伤
    /// <summary>
    /// 获取最终攻击力
    /// </summary>
    public int GetAtk()
    {
        return Mathf.FloorToInt((atkPhy + valAAtkPhy) * valBAtkPhy);
    }
    /// <summary>
    /// 增加攻击力
    /// </summary>
    /// <returns></returns>
    public void AtkIncrease(float percent)
    {
        valBAtkPhy *= percent;
    }

    /// <summary>
    /// 增加攻击力
    /// </summary>
    /// <returns></returns>
    public void AtkIncrease(int val)
    {
        valAAtkPhy += val;
    }

    public int atkMag;  // 魔法攻击力

    public float animRate;
    private float ias;     // 攻击速度。每秒攻击次数
    // 动画时间1秒。攻速不会超过1
    public float _IAS
    {
        get { return ias; }
        set
        {
            // 根据动画时间和攻速设置实际时间
            ias = value;

            float atkTimeOri = atkAnimTimeBeforeBase + atkAnimTimeAfterBase;
            float atkTime = 1 / ias;

            if (atkTime > atkTimeOri)
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
    }

    private float baseWeaponIAS; // 基础武器速度

    public float _BaseWeaponIAS
    {
        get { return baseWeaponIAS; }
        set
        {
            float oldVal = baseWeaponIAS;
            baseWeaponIAS = value;
            if (oldVal != 0)
            {
                _IAS = _IAS / oldVal * baseWeaponIAS;
            }
            else
            {
                _IAS = baseWeaponIAS;
            }

        }
    }
    /// <summary>
    /// 火抗
    /// </summary>
    public int resFire;
    int valAResFire;
    float valBResFire = 1f;
    public int GetResFire()
    {
        return Mathf.FloorToInt((resFire + valAResFire) * valBResFire);
    }

    public void ResFireIncrease(float percent)
    {
        valBResFire *= percent;
    }

    public void ResFireIncrease(int val)
    {
        valAResFire += val;
    }

    /// <summary>
    /// 毒抗
    /// </summary>
    public int resPoision;
    int valAResPoision;
    float valBResPoision = 1f;
    public int GetResPoision()
    {
        return Mathf.FloorToInt((resPoision + valAResPoision) * valBResPoision);
    }

    public void ResPosisionIncrease(float percent)
    {
        this.valBResPoision *= percent;
    }

    public void ResPosisionIncrease(int val)
    {
        this.valAResPoision += val;
    }

    /// <summary>
    /// 电抗
    /// </summary>
    public int resThunder;
    int valAResThunder;
    float valBResThunder = 1f;
    public int GetResThunder()
    {
        return Mathf.FloorToInt((resThunder + valAResThunder) * valBResThunder);
    }

    public void ResThunderIncrease(float percent)
    {
        this.valBResThunder *= percent;
    }

    public void ResThunderIncrease(int val)
    {
        this.valAResThunder += val;
    }

    /// <summary>
    /// 冰抗
    /// </summary>
    public int resForzen;
    int valAResForzen;
    float valBResForzen = 1f;
    public int GetResForzen()
    {
        return Mathf.FloorToInt((resForzen + valAResForzen) * valBResForzen);
    }

    public void ResForzenIncrease(float percent)
    {
        valBResForzen *= percent;
    }

    public void ResForzenIncrease(int val)
    {
        valAResForzen += val;
    }

    /// <summary>
    /// 护甲
    /// </summary>
    public int arm;
    int valAArm;
    float valBArm = 1f;
    /// <summary>
    /// 获取最终护甲
    /// </summary>
    /// <returns></returns>
    public int GetArm()
    {
        return Mathf.FloorToInt((arm + valAArm) * valBArm);
    }

    public void DefIncrease(float percent)
    {
        valBArm *= percent;
    }

    public void DefIncrease(int val)
    {
        valAArm += val;
    }

    private float deadlyStrike;	// 致命一击几率[0~1]

    public float _DeadlyStrike
    {
        get { return deadlyStrike; }
        set { deadlyStrike = value; }
    }
    public int hit;     // 命中
    public int dodge;   // 躲闪
    public int wardOffBlows;    // 招架
    public float parry;       // 格挡几率[0~1]
    public int parryDamage; // 格挡伤害

    public int atkFireParamAdd = 0; // 火焰伤害参数A
    public float atkFireParamDot = 1f; // 火焰伤害参数B
    public int _AtkFire
    {
        get
        {
            return Mathf.RoundToInt(atkFireParamAdd * atkFireParamDot);
        }
    }


    public int atkThunderParamAdd = 0;  // 额外的闪电伤害参数A
    public float atkThunderParamDot = 1f;   // 额外的闪电伤害参数B
    public int _AtkThunder
    {
        get
        {
            return Mathf.RoundToInt(atkThunderParamAdd * atkThunderParamDot);
        }
    }



    public int atkPoisonParamAdd = 0;   // 额外的毒素伤害参数A
    public float atkPoisonParamDot = 1f;    // 额外的毒素伤害参数B

    public int _AtkPoison
    {
        get { return Mathf.RoundToInt(atkPoisonParamAdd * atkPoisonParamDot); }
    }


    public int atkIceParamAdd = 0;      // 额外的冰冷伤害参数A
    public float atkIceParmaDot = 1f;   // 额外的冰冷伤害参数B

    public int _AtkIce
    {
        get { return Mathf.RoundToInt(atkIceParamAdd * atkIceParmaDot); }
    }

    private float deadlyStrikeDamage = 2f; //致命一击倍率

    public float _DeadlyStrikeDamage
    {
        get { return deadlyStrikeDamage; }
        set { deadlyStrikeDamage = value; }
    }

    public float atkAnimTimeBeforeBase; // 基础攻击前摇时间
    public float atkAnimTimeAfterBase; //  基础攻击后摇时间

    public float atkAnimTimeBefore; // 攻击前摇时间
    public float atkAnimTimeAfter; //  攻击后摇时间
    public float atkTimeInterval; // 攻击间隔时间

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

    //protected Animation anim;

    void Awake()
    {
        //anim = Tools.GetComponentInChildByPath<Animation>(gameObject, "model");
    }

    public void Start()
    {
    }

    public void AddEng(int engAdd)
    {
        _Mp += engAdd;
        if (_Mp > mpMax)
        {
            _Mp = mpMax;
        }
    }

    public int GetDamgerToOther(int damageOri, IActor other, EDamageType damageType = EDamageType.Phy) {
        int damage = 0;
        int atk = damageOri;

        int armOther = 0;
        switch (damageType)
        {
            case EDamageType.Phy:
                armOther = other.GetArm();
                break;
            case EDamageType.Fire:
                armOther = other.GetResFire();
                break;
            case EDamageType.Lighting:
                armOther = other.GetResThunder();
                break;
            case EDamageType.Poison:
                armOther = other.GetResPoision();
                break;
            case EDamageType.Forzen:
                armOther = other.GetResForzen();
                break;
            default:
                break;
        }


        if (armOther >= 0)
        {
            int damgeOffset = (int)(atk * (armOther * 0.06 / (armOther * 0.06 + 1)));
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

        // 格挡。只有物理伤害能格挡
        bool isParry = false;
        if (damageType == EDamageType.Phy && Tools.IsHitOdds(target.parry))
        {
            damageOri -= target.parryDamage;
            target.OnParry(target.parryDamage);
            isParry = true;
        }

        int damage = GetDamgerToOther(damageOri, target, damageType);
        bool isDs = false;// 是否致命一击
                          // 致命一击
        if (canDS && Tools.IsHitOdds(_DeadlyStrike)) {
            damage = (int)(damage * _DeadlyStrikeDamage);
            isDs = true;
        }

        target.OnHurted(damage, damageType, this, isDs);
        OnDamageTarget(damage, target, isDs);

        int oriTargetHP = target.hp;

        target.hp -= damage;
        if (target.hp <= 0) {
            target.hp = 0;
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
                if (GameManager.gameView.State == GameState.Battle)
                {
                    GameManager.gameView.OnKillCurTarget();
                }
            }
        }
        else
        {
            target.OnHPChange(oriTargetHP, target.hp);
        }
        if (!target.isHero) {
            UIManager._Instance.uiMain.RefreshTargetHP(target as Enermy);

            if (isDs)
            {
                string strParryDesc = "";
                if (isParry)
                {
                    strParryDesc = string.Format("(格挡{0})", target.parryDamage);
                }
                UIManager._Instance.ShowDSDamageTxt(damage.ToString() + strParryDesc);
            }
            else
            {
                if (isParry)
                {
                    UIManager._Instance.ShowDamageTxt(damage, damageType, target.parryDamage);
                }
                else
                {
                    UIManager._Instance.ShowDamageTxt(damage, damageType);
                }
            }
        }
        else
        {
            UIManager._Instance.uiMain.RefreshHeroHP();
            if (isParry)
            {
                UIManager._Instance.ShowHurtedTxt(damage, damageType, target.parryDamage);
            }
            else
            {
                UIManager._Instance.ShowHurtedTxt(damage, damageType);
            }

        }
    }

    /// <summary>
    /// 攻击目标时检测是否命中
    /// </summary>
    /// <returns></returns>
    protected bool CheckHitTarget(IActor target)
    {
        bool ishit = false;
        if ((hit + target.dodge) != 0)
        {
            if (Tools.IsHitOdds((float)hit / (hit + target.dodge)))
            {
                ishit = true;
            }
        }
        else
        {
            if (Tools.IsHitOdds(0.5f))
            {
                ishit = true;
            }
        }
        return ishit;
    }

    public void Hurted(int damageOri) {
        int damage = GetDamgerToOther(damageOri, this);

        hp -= damage;
        if (hp <= 0) {
            hp = 0;
        }
        if (isHero) {
            UIManager._Instance.uiMain.RefreshHeroHP();
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
    public virtual void OnParry(int damageParry) { }
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
                    _Mp -= 10;
                    UIManager._Instance.uiMain.RefreshHeroMP();
                }
                OnIntoAGrid(GetCurMapGrid());
            }
        }

        if (GameManager.gameView.State != GameState.Battle)
        {
            _State = EActorState.Normal;
        }

        yield return 0;

    }

    public void MoveToAGrid(MapGrid mgNext)
    {
        StartCoroutine(CoMoveToAGrid(mgNext));
    }

    IEnumerator CoMoveToAGrid(MapGrid mgNext)
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

            OnLeaveAGrid(mgCur);

            OnTryToAGrid(mgNext);

            iTween.MoveTo(gameObject, iTween.Hash("position", mgNext.transform.position, "time", timePerGrid, "easetype", iTween.EaseType.linear));

            yield return new WaitForSeconds(timePerGrid);

            // 每次移动消耗怒气
            if (isHero)
            {
                _Mp -= 10;
                UIManager._Instance.uiMain.RefreshHeroMP();
            }
            OnIntoAGrid(GetCurMapGrid());
        }

        if (GameManager.gameView.State != GameState.Battle)
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
}
