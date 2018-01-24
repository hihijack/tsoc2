using UnityEngine;

class PropertyHero : PropertyBase
{
    Hero hero;
    public PropertyHero(Hero hero)
    {
        this.hero = hero;
    }

    /// <summary>
    /// 计算攻击力
    /// </summary>
    /// <param name="ei">生效的武器</param>
    /// <returns></returns>
    public override int GetAtk(EquipItem ei)
    {
        int atk = 0;
        int baseAtk = 0;
        if (ei != null)
        {
            baseAtk = ei.atk;
        }
        else
        {
            baseAtk = IConst.ATK_EMPTY;
        }

        atk = UnityEngine.Mathf.CeilToInt(
            (
                baseAtk * (1 + Strength * IConst.ATK_PHY_PER_STR))
            );
        atk = UnityEngine.Mathf.CeilToInt(atk * AtkParmaC + AtkParmaD);

        return atk;
    }

    public override int GetAtkFire(EquipItem ei)
    {
        if (ei != null)
        {
            return Mathf.CeilToInt((ei.atkFire + atkFireParamAdd) * atkFireParamDot);
        }
        else
        {
            return Mathf.CeilToInt(atkFireParamAdd * atkFireParamDot);
        }
    }

    public override int GetAtkThunder(EquipItem ei)
    {
        if (ei != null)
        {
            return Mathf.CeilToInt((ei.atkThunder + atkThunderParamAdd) * atkThunderParamDot);
        }
        else
        {
            return Mathf.CeilToInt(atkThunderParamAdd * atkThunderParamDot);
        }
        
    }

    public override int GetAtkIce(EquipItem ei)
    {
        if (ei != null)
        {
            return Mathf.CeilToInt((ei.atkIce + atkIceParamAdd) * atkIceParmaDot);
        }
        else
        {
            return Mathf.CeilToInt((atkIceParamAdd) * atkIceParmaDot);
        }
       
    }

    public override int GetAtkPoison(EquipItem ei)
    {
        if (ei != null)
        {
            return Mathf.CeilToInt((ei.atkPoison + atkPoisonParamAdd) * atkPoisonParamDot);
        }
        else
        {
            return Mathf.CeilToInt((atkPoisonParamAdd) * atkPoisonParamDot);
        }
    }

    public override int GetPowerAtk(EquipItem ei, int power)
    {
        int r = ei.atk;
        if (power == 1)
        {
            r = Mathf.CeilToInt(ei.atk * ei.baseData.poweratk_1 * ei.powerDmg);
        }
        else if (power == 2)
        {
            r = Mathf.CeilToInt(ei.atk * ei.baseData.poweratk_2 * ei.powerDmg);
        }
        else if (power == 3)
        {
            r = Mathf.CeilToInt(ei.atk * ei.baseData.poweratk_3 * ei.powerDmg);
        }
        return r;
    }

    public override int GetPowerDP(EquipItem ei, int power)
    {
        int r = ei.baseData.dp;
        if (power == 1)
        {
            r = ei.baseData.dp1;
        }
        else if (power == 2)
        {
            r = ei.baseData.dp2;
        }
        else if (power == 3)
        {
            r = ei.baseData.dp3;
        }
        return r;
    }

    public override int GetPowerForce(EquipItem ei, int power)
    {
        int r = ei.baseData.atkForce;
        if (power == 1)
        {
            r = ei.baseData.atkForce1;
        }
        else if (power == 2)
        {
            r = ei.baseData.atkForce2;
        }
        else if (power == 3)
        {
            r = ei.baseData.atkForce3;
        }
        return r;
    }

    public override float GetPowerSpeed(EquipItem ei)
    {
        if (ei != null)
        {
            return (ei.powerSpeed + powerSpeedParamAdd) * powerSpeedParamDot;
        }
        else
        {
            //空手蓄力速度
            return (IConst.POWER_SPEED_EMPTY + powerSpeedParamAdd) * powerSpeedParamDot;
        }
    }

    public override void CalIAS()
    {
        base.CalIAS();
        //负重惩罚
        float loadOff = 1f;
        if (Load >= 26)
        {
            //攻速降低10%
            loadOff = 0.9f;
        }
        IAS = (BaseWeaponIAS * loadOff * (1 + Agility * IConst.IAS_PERCENT_PER_AGI * 0.01f) + IasParmaA) * IasParmaB + IasParmaC;
    }

    public override float GetAtkTimeAfter()
    {
        return 1 / IAS * 0.5f;
    }

    public override float GetAtkTimeBefore()
    {
        return 1 / IAS * 0.5f;
    }

    public override float ParryDamPercent(EDamageType dmgType)
    {
        //如果副手有装备，取副手
        //否则取主手
        float baseVal = 0f;
        EquipItem eiHand2 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand2);
        if (eiHand2 != null)
        {
            baseVal = eiHand2.GetParry(dmgType);
        }
        else
        {
            EquipItem eiHand1 = GameView.Inst.eiManager.GetEquipItemHasEquip(EEquipPart.Hand1);
            if (eiHand1 != null)
            {
                baseVal = eiHand1.GetParry(dmgType);
            }
        }
        return (baseVal + parryDmgPerParamAdd) * parryDmbPerParamDot;
    }
}