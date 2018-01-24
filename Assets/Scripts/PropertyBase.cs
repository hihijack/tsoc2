using UnityEngine;

public class PropertyBase
{
    #region 属性
    private float deadlyStrike;	// 致命一击几率[0~1]
    private int hp;//当前血量
    private int hpMax;// 生命值上限
    private float _vigor;//精力
    //TODO精力上限
    private int _vigorMax = 100;//精力上限

    private int stamina; // 体能
    private int strength; // 力量
    private int agility; // 敏捷
    private int tenacity;  // 坚韧
    private int endurance;//持久力

    private float baseWeaponIAS; // 基础武器速度
    private float ias;//最终攻速
    private float iasParmaA;
    private float iasParmaB = 1f;
    private float iasParmaC;
    private float atkParmaC = 1f;//技能buff增伤
    private int atkParmaD;//技能buff增伤
    public int resFire;//火抗
    int valAResFire;
    float valBResFire = 1f;
    public int resPoision;//毒抗
    int valAResPoision;
    float valBResPoision = 1f;
    public int resThunder;
    int valAResThunder;//电抗
    float valBResThunder = 1f;
    public int resForzen;//冰抗
    int valAResForzen;
    float valBResForzen = 1f;
    public int arm;//护甲
    int valAArm;
    float valBArm = 1f;
    int moveSpeedPramaA;
    float moveSpeedPramaB = 1f;
    private int moveSpeedBase;   // 基础移动速度
    private int _moveSpeed;//最终移动速度
    public int wardOffBlows;    // 招架
    public float parry;       // 格挡几率[0~1]
    public int parryDamage; // 格挡伤害
    public float parryDmgPerParamAdd = 0f;//格挡伤害百分比参数A
    public float parryDmbPerParamDot = 1f;//格挡伤害百分比参数B
    public float parryDmgVigorBase = 0f;//格挡值基础
    public float parryDmgVigorParamMul = 1f;
    public float parryDmgVigorParamAdd = 0f;
    public int atkFireParamAdd = 0; // 火焰伤害参数A
    public float atkFireParamDot = 1f; // 火焰伤害参数B
    public int atkThunderParamAdd = 0;  // 额外的闪电伤害参数A
    public float atkThunderParamDot = 1f;   // 额外的闪电伤害参数B
    public int atkPoisonParamAdd = 0;   // 额外的毒素伤害参数A
    public float atkPoisonParamDot = 1f;    // 额外的毒素伤害参数B
    public int atkIceParamAdd = 0;      // 额外的冰冷伤害参数A
    public float atkIceParmaDot = 1f;   // 额外的冰冷伤害参数B
    private float deadlyStrikeDamage = 2f; //致命一击倍率

    public float powerSpeedParamAdd = 0f; //蓄力速度提升
    public float powerSpeedParamDot = 1f; //蓄力速度提升百分百

    [System.Obsolete]
    private int energyPoint;
    int loadPramaA;
    float loadPramaB = 1f;
    private int _loadBase;//负重
    private int _load;//最终负重

    //精力恢复速度
    private int _engRecoverSpeedBase;
    private float _engRecoverSpeedParmPercent = 1f;
    private int _engRecoveSpeedParmAdd = 0;

    private int _tenacityUCtl;//韧性
    public int maxTenactiyUCtl;
    public int tenacityLevel;//霸体等级
    #endregion

    #region GeterAndSeter

    public int TenacityUCtl
    {
        get
        {
            return _tenacityUCtl;
        }
        set
        {
            _tenacityUCtl = Mathf.Clamp(value, 0, maxTenactiyUCtl);
        }
    }

    /// <summary>
    /// 格挡值。每点精力格挡伤害
    /// </summary>
    public float ParryDmgVigor
    {
        get
        {
            //如果副手有装备，取副手
            //否则取主手
            float baseVal = 0.1f;
            EquipItem eiHand2 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand2);
            if (eiHand2 != null)
            {
                baseVal = eiHand2.baseData.parryVigor;
            }
            else
            {
                EquipItem eiHand1 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand1);
                if (eiHand1 != null)
                {
                    baseVal = eiHand1.baseData.parryVigor;
                }
            }
            return baseVal * parryDmgVigorParamMul + parryDmgVigorParamAdd;
        }
    }

    public int EngRecoverSpeedBase
    {
        set
        {
            _engRecoverSpeedBase = value;
        }
    }

    public float EngRecoverSpeedParmPercent
    {
        set
        {
            _engRecoverSpeedParmPercent = value;
        }
    }

    public int EngRecoveSpeedParmAdd
    {
        set
        {
            _engRecoveSpeedParmAdd = value;
        }
    }

    public int VigorRecoveSpeed
    {
        get
        {
            return  Mathf.CeilToInt(_engRecoverSpeedBase * _engRecoverSpeedParmPercent) + _engRecoveSpeedParmAdd;
        }
    }

    /// <summary>
    /// 能量点
    /// </summary>
    public int EnergyPoint
    {
        get
        {
            return energyPoint;
        }

        set
        {
            energyPoint = value;
            if (energyPoint < 0)
            {
                energyPoint = 0;
            }
            if (energyPoint > 5)
            {
                energyPoint = 5;
            }
        }
    }

    /// <summary>
    /// 当韧性被清空
    /// </summary>
    public virtual void OnTenacityUCtlToZero()
    {

    }

    /// <summary>
    /// 格挡伤害百分比
    /// </summary>
    public virtual float ParryDamPercent(EDamageType dmgType)
    {
        return 0f;
    }

    public virtual int GetAtkFire(EquipItem ei)
    {
        return 0;
    }

    public virtual int GetAtkThunder(EquipItem ei)
    {
        return 0;
    }

    public virtual int GetAtkPoison(EquipItem ei)
    {
        return 0;
    }

    public virtual int GetAtkIce(EquipItem ei)
    {
        return 0;
    }

    public int ResFire
    {
        get
        {
            return Mathf.FloorToInt((resFire + valAResFire) * valBResFire);
        }
    }

    /// <summary>
    /// 负重
    /// </summary>
    public int Load
    {
        get
        {
            return _load;
        }
    }

    public int ResPoision
    {
        get
        {
            return Mathf.FloorToInt((resPoision + valAResPoision) * valBResPoision);
        }
    }

    public int ResThunder
    {
        get
        {
            return Mathf.FloorToInt((resThunder + valAResThunder) * valBResThunder);
        }
    }

    public int ResForzen
    {
        get
        {
            return Mathf.FloorToInt((resForzen + valAResForzen) * valBResForzen);
        }
    }

    public int Arm
    {
        get
        {
            return Mathf.FloorToInt((arm + valAArm) * valBArm);
        }
    }

    public int MoveSpeed
    {
        get
        {
            return _moveSpeed;
        }
    }

    /// <summary>
    /// 精力
    /// </summary>
    public float Vigor
    {
        get { return _vigor; }
        set
        {
            _vigor = value;
            _vigor = Mathf.Clamp(_vigor, 0, VigorMax);
        }
    }

    /// <summary>
    /// 取蓄力速度
    /// </summary>
    /// <param name="ei"></param>
    /// <returns></returns>
    public virtual float GetPowerSpeed(EquipItem ei)
    {
        return 0f;
    }

    /// <summary>
    /// 取蓄力伤害
    /// </summary>
    /// <param name="ei"></param>
    /// <param name="power"></param>
    /// <returns></returns>
    public virtual int GetPowerAtk(EquipItem ei, int power)
    {
        return 0;
    }

    public virtual int GetPowerDP(EquipItem ei, int power)
    {
        return 0;
    }

    public virtual int GetPowerForce(EquipItem ei, int power)
    {
        return 0;
    }

    public float DeadlyStrikeDamage
    {
        get { return deadlyStrikeDamage; }
        set { deadlyStrikeDamage = value; }
    }
    /// <summary>
    /// 致命一击几率[0~1]
    /// </summary>
    public float DeadlyStrike
    {
        get { return deadlyStrike; }
        set { deadlyStrike = value; }
    }


    /// <summary>
    /// 坚韧。减少百分百伤害
    /// </summary>
    public int Tenacity
    {
        get
        {
            return tenacity;
        }

        set
        {
            tenacity = value;
        }
    }

    public int Endurance
    {
        get
        {
            return endurance;
        }
        set
        {
            endurance = value;
            VigorMax = IConst.VIGOR_PER_END * endurance;
        }
    }

    /// <summary>
    /// 当前血量
    /// </summary>
    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
        }
    }

    /// <summary>
    /// 生命值上限
    /// </summary>
    public int HpMax
    {
        get { return hpMax; }
        set
        {
            hpMax = value;
            if (Hp > hpMax)
            {
                Hp = hpMax;
            }
        }
    }

    public int VigorMax
    {
        get
        {
            return _vigorMax;
        }

        set
        {
            _vigorMax = value;
            if (Vigor > _vigorMax)
            {
                Vigor = _vigorMax;
            }
        }
    }

    /// <summary>
    /// 体能
    /// </summary>
    public int Stamina
    {
        get
        {
            return stamina;
        }

        set
        {
            stamina = value;

            HpMax = stamina * IConst.HP_PER_STA;
        }
    }

    public float IasParmaA
    {
        get
        {
            return iasParmaA;
        }

        set
        {
            iasParmaA = value;
            CalIAS();
        }
    }

    public float IasParmaB
    {
        get
        {
            return iasParmaB;
        }

        set
        {
            iasParmaB = value;
            CalIAS();
        }
    }

    public float IasParmaC
    {
        get
        {
            return iasParmaC;
        }

        set
        {
            iasParmaC = value;
            CalIAS();
        }
    }

    /// <summary>
    /// 武器基础攻速
    /// </summary>
    public float BaseWeaponIAS
    {
        get { return baseWeaponIAS; }
        set
        {
            baseWeaponIAS = value;
            CalIAS();
        }
    }

    /// <summary>
    /// 攻速
    /// </summary>
    public float IAS
    {
        get { return ias; }
        set { ias = value; }
    }

    /// <summary>
    /// 力量
    /// </summary>
    public int Strength
    {
        get { return strength; }
        set
        {
            strength = value;
            //CalAtk();
        }
    }

    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility
    {
        get
        {
            return agility;
        }

        set
        {
            agility = value;
            CalIAS();
        }
    }

    /// <summary>
    /// 最终攻击力
    /// </summary>
    public virtual int GetAtk(EquipItem ei)
    {
        return 0;
    }

    /// <summary>
    /// 技能buff增伤
    /// </summary>
    public float AtkParmaC
    {
        get
        {
            return atkParmaC;
        }

        set
        {
            atkParmaC = value;
            //CalAtk();
        }
    }

    /// <summary>
    /// 技能buff增伤
    /// </summary>
    public int AtkParmaD
    {
        get
        {
            return atkParmaD;
        }

        set
        {
            atkParmaD = value;
            //CalAtk();
        }
    }

    /// <summary>
    /// 伤害减免
    /// </summary>
    public float DamReduce
    {
        get
        {
            return UnityEngine.Mathf.Min(Tenacity * IConst.DAMREDUCE_PER_TEN, 0.6f);
        }
    }

    /// <summary>
    /// 基础移动速度
    /// </summary>
    public int MoveSpeedBase
    {
        get
        {
            return moveSpeedBase;
        }

        set
        {
            moveSpeedBase = value;
            CalMoveSpeed();
        }
    }

    /// <summary>
    /// 基础负重
    /// </summary>
    public int LoadBase
    {
        get
        {
            return _loadBase;
        }

        set
        {
            _loadBase = value;
            CalLoad();
        }
    }
    #endregion

    public void CalLoad()
    {
        _load = Mathf.FloorToInt((LoadBase + loadPramaA) * loadPramaB);
        CalMoveSpeed();
        CalIAS();
    }

    public virtual void  CalAtk()
    {
        
    }

    public virtual void CalIAS() { }

    public void ResFireIncrease(float percent)
    {
        valBResFire *= percent;
    }

    public void ResFireIncrease(int val)
    {
        valAResFire += val;
    }

    public void ResPosisionIncrease(float percent)
    {
        this.valBResPoision *= percent;
    }

    public void ResPosisionIncrease(int val)
    {
        this.valAResPoision += val;
    }

    public void ResThunderIncrease(float percent)
    {
        this.valBResThunder *= percent;
    }

    public void ResThunderIncrease(int val)
    {
        this.valAResThunder += val;
    }

    public void ResForzenIncrease(float percent)
    {
        valBResForzen *= percent;
    }

    public void ResForzenIncrease(int val)
    {
        valAResForzen += val;
    }

    public void DefIncrease(float percent)
    {
        valBArm *= percent;
    }

    public void DefIncrease(int val)
    {
        valAArm += val;
    }

    public void LoadIncrease(int val)
    {
        loadPramaA += val;
        CalLoad();
    }

    public void LoadIncrease(float val)
    {
        loadPramaB *= val;
        CalLoad();
    }

    public void MoveSpeedIncrease(int val)
    {
        moveSpeedPramaA += val;
    }

    public void MoveSpeedIncrease(float val)
    {
        moveSpeedPramaB *= val;
    }

    private void CalEnergyPoint()
    {
        ////每30点转换成1点能量点
        //int eng = _vigor / 30;
        //EnergyPoint += eng;
        //_vigor = _vigor % 30;
    }

    void CalMoveSpeed()
    {
        int offset = 0;
        //负重影响
        if (Load >= 16 && Load < 26)
        {
            offset = -2;
        }
        else if (Load >= 26)
        {
            offset = -3;
        }
        _moveSpeed = Mathf.FloorToInt((MoveSpeedBase + offset + moveSpeedPramaA) * moveSpeedPramaB);
    }

    public void PowerSpeedIncrease(float val)
    {
       
    }

    public virtual float GetAtkTimeBefore()
    {
        return 0f;
    }

    public virtual float GetAtkTimeAfter()
    {
        return 0f;
    }
}