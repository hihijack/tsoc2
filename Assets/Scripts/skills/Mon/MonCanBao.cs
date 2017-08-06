using UnityEngine;
using System.Collections;

/// <summary>
/// 残暴：暴击率提升
/// </summary>
public class MonCanBao : IMonSkill {

    float val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(14);
        this.val = skillBD.GetFloatVal(level, "val");
    }

    public override void OnEnterBattle()
    {
        base.OnEnterBattle();
        Enermy eCur = GetCurEnermy();
        eCur.Prop.DeadlyStrike = val;
    }
}
