using UnityEngine;
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
        if (!InCD && target != null && CheckCost())
        {
            StartCD();
            caster.IsSkilling = true;

            // 施法前摇
            yield return new WaitForSeconds(GetBaseData().casttime);

            // 特效
            GameManager.commonCPU.CreateEffect("eff_hand_two_1", target.transform.position, Color.red, -1f);

            int damage = (int)(caster.GetAtk() * damageRate);
            caster.DamageTarget(damage, target);
            caster.AddEng(engGet);
            UIManager._Instance.uiMain.RefreshHeroMP();

            // 施法后摇
            yield return new WaitForSeconds(0.5f);
            
            caster.IsSkilling = false;
        }
    }
}
