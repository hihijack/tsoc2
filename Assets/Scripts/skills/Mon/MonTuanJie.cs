using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 恶魔集结：每个友军提示攻击力/防御力
/// </summary>

public class MonTuanJie : IMonSkill {

    float percentAtk;
    float percentDef;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(13);
        this.percentAtk = skillBD.GetFloatVal(level, "val_atk");
        this.percentDef = skillBD.GetFloatVal(level, "val_def");
    }

    public override void OnEnterBattle()
    {
        base.OnEnterBattle();
        Enermy eCur = GetCurEnermy();
        List<Enermy> allyEnermys = eCur.GetAlliesInBattle();
        eCur.Prop.AtkParmaC *= (1 + percentAtk * allyEnermys.Count);
        eCur.Prop.DefIncrease(1 + percentDef * allyEnermys.Count);
        GameManager.commonCPU.CreateEffect("eff_emojijie", eCur.GetPos(), Color.white, -1f);
        foreach (Enermy itemEnermy in allyEnermys)
        {
            itemEnermy.Prop.AtkParmaC *= (1 + percentAtk * allyEnermys.Count);
            itemEnermy.Prop.DefIncrease(1 + percentDef * allyEnermys.Count);
            GameManager.commonCPU.CreateEffect("eff_emojijie", itemEnermy.GetPos(), Color.white, -1f);
        }
    }
}
