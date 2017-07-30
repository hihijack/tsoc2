using UnityEngine;

public class PropertyBase
{
    #region 属性
    private float deadlyStrike;	// 致命一击几率[0~1]
    private int hp;//当前血量
    private int hpMax;// 生命值上限
    private int stamina; // 体能
    private int strength; // 力量
    private int agility; // 敏捷
    private int tenacity;  // 坚韧
    private float baseWeaponIAS; // 基础武器速度
    private float ias;//最终攻速
    private float iasParmaA;
    private float iasParmaB = 1f;
    private float iasParmaC;
    private int atkBaseA;//主手基础攻击
    private int atkBaseB;//副手基础攻击
    private int atk;//最终攻击
    private float atkParmaA = 1f;//增伤词缀A
    private int atkParmaB;//增伤词缀B
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
    public float parryDmgPerParamA = 0f;//格挡伤害百分比参数A
    public float parryDmbPerParamB = 1f;//格挡伤害百分比参数B
    public float parryDamPerBase; //格挡伤害百分比基础。
    public int atkFireParamAdd = 0; // 火焰伤害参数A
    public float atkFireParamDot = 1f; // 火焰伤害参数B
    public int atkThunderParamAdd = 0;  // 额外的闪电伤害参数A
    public float atkThunderParamDot = 1f;   // 额外的闪电伤害参数B
    public int atkPoisonParamAdd = 0;   // 额外的毒素伤害参数A
    public float atkPoisonParamDot = 1f;    // 额外的毒素伤害参数B
    public int atkIceParamAdd = 0;      // 额外的冰冷伤害参数A
    public float atkIceParmaDot = 1f;   // 额外的冰冷伤害参数B
    private float deadlyStrikeDamage = 2f; //致命一击倍率
    private int energyPoint;
    private int mp;
    float powerSpeed;//蓄力速度
    int loadPramaA;
    float loadPramaB = 1f;
    private int _loadBase;//负重
    private int _load;//最终负重
    #endregion

    #region GeterAndSeter
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
    /// 格挡伤害百分百
    /// </summary>
    public float ParryDamPercent
    {
        get
        {
            return (parryDamPerBase + parryDmgPerParamA) * parryDmbPerParamB;
        }
    }

    /// <summary>
    /// 火焰攻击
    /// </summary>
    public int AtkFire
    {
        get
        {
            return Mathf.RoundToInt(atkFireParamAdd * atkFireParamDot);
        }
    }

    /// <summary>
    /// 闪电攻击
    /// </summary>
    public int AtkThunder
    {
        get
        {
            return Mathf.RoundToInt(atkThunderParamAdd * atkThunderParamDot);
        }
    }

    /// <summary>
    /// 毒素攻击
    /// </summary>
    public int AtkPoison
    {
        get { return Mathf.RoundToInt(atkPoisonParamAdd * atkPoisonParamDot); }
    }

    /// <summary>
    /// 冰冷攻击
    /// </summary>
    public int AtkIce
    {
        get { return Mathf.RoundToInt(atkIceParamAdd * atkIceParmaDot); }
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

    public int Mp
    {
        get { return mp; }
        set
        {
            mp = value;
            if (mp < 0)
            {
                mp = 0;
            }
            CalEnergyPoint();
        }
    }
    public float PowerSpeed
    {
        get
        {
            return powerSpeed;
        }

        set
        {
            powerSpeed = value;
        }
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
            CalAtk();
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
    /// 主手基础攻击力
    /// </summary>
    public int AtkBaseA
    {
        get
        {
            return atkBaseA;
        }

        set
        {
            atkBaseA = value;
            CalAtk();
        }
    }

    /// <summary>
    /// 副手基础攻击力
    /// </summary>
    public int AtkBaseB
    {
        get
        {
            return atkBaseB;
        }

        set
        {
            atkBaseB = value;
            CalAtk();
        }
    }

    /// <summary>
    /// 最终攻击力
    /// </summary>
    public int Atk
    {
        get
        {
            return atk;
        }

        set
        {
            atk = value;
        }
    }

    /// <summary>
    /// 增伤词缀A
    /// </summary>
    public float AtkParmaA
    {
        get
        {
            return atkParmaA;
        }

        set
        {
            atkParmaA = value;
            CalAtk();
        }
    }

    /// <summary>
    /// 增伤词缀B
    /// </summary>
    public int AtkParmaB
    {
        get
        {
            return atkParmaB;
        }

        set
        {
            atkParmaB = value;
            CalAtk();
        }
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
            CalAtk();
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
            CalAtk();
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
        //每30点转换成1点能量点
        int eng = mp / 30;
        EnergyPoint += eng;
        mp = mp % 30;
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
        powerSpeed *= val;
    }
}