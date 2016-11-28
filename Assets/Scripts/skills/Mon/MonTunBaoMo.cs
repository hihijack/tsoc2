using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 屯宝魔：所有友军有额外的掉落
/// </summary>

public class MonTunBaoMo : IMonSkill {

    int dropoffet; // 额外掉落个数
    float goldOffset; // 金币倍数
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(19);
        dropoffet = skillBD.GetIntVal(level, "drop");
        goldOffset = skillBD.GetFloatVal(level, "gold");
    }

    public override void OnEnterBattle()
    {
        base.OnEnterBattle();
        List<Enermy> allies = _ECur.GetAlliesInBattle(true);
        foreach (Enermy eItems in allies)
        {
            eItems.dropCashOffet += goldOffset;
            eItems.dropOffset += dropoffet;
        }
    }
}
