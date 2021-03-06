﻿using UnityEngine;
using System.Collections;

public class NuJi : ISkill {

    public float damageRate;
    public int engGet;

    private IActor target;
    
    public override void Init()
    {
        SetBaseData(GameDatas.GetSkillBD(4));
        SetDamageRateByLevel();
        SetEngGet();
    }

    void SetDamageRateByLevel()
    {
        if (_Level > 0)
        {
            damageRate = GetBaseData().jdData[_Level - 1]["val"].AsFloat;
        }
    }

    void SetEngGet()
    {
        if (_Level > 0)
        {
            engGet = GetBaseData().jdData[_Level - 1]["eng"].AsInt;
        }
    }

    public override void SetCaster(IActor _caster)
    {
        caster = _caster;
    }

    public override void SetTarget(IActor _target)
    {
        target = _target;
    }

    public override void OnLevelChange()
    {
        SetDamageRateByLevel();
        SetEngGet();
    }

    public override IEnumerator Act()
    {
        if (CheckCast())
        {
            StartCD();
            StartCost();

            // 施法前摇
            yield return new WaitForSeconds(GetBaseData().casttime);

            // 特效
            GameManager.commonCPU.CreateEffect("eff_hand_two_1", target.transform.position, Color.red, -1f);

            int damage = (int)(caster.Prop.GetAtk(Hero.Inst.GetAtkWpon()) * damageRate);
            caster.DamageTarget(target, new DmgData(damage, EDamageType.Phy));
            caster.Prop.Vigor += engGet;
            UIManager.Inst.uiMain.RefreshHeroVigor();

            // 施法后摇
            yield return new WaitForSeconds(0.5f);

            GameManager.gameView._MHero.BsManager.ActionSkillEnd(this);
        }
        else
        {
            GameManager.gameView._MHero.BsManager.ActionSkillEnd(this);
        }
    }

    public override bool CheckCast()
    {
        return !InCD && target != null && CheckCost();
    }
}
