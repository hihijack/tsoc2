using UnityEngine;
using System.Collections;

/// <summary>
/// 强袭：暴击造成额外伤害
/// </summary>
public class MonQiangXi : IMonSkill {

    float val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(11);
        this.val = skillBD.GetFloatVal(level, "val");
    }

    public override void OnEnterBattle()
    {
        base.OnEnterBattle();
        Enermy eCur = GetCurEnermy();
        eCur._DeadlyStrikeDamage += val;
    }
}