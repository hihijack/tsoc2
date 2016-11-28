using UnityEngine;
using System.Collections;

/// <summary>
/// 嗜血：每次攻击吸收x点生命值
/// </summary>
public class ShiXue : IMonSkill {

    float val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(8);
        this.val = skillBD.GetFloatVal(level, "val");
    }

    public override void OnAttackHit(IActor target, int attack)
    {
        base.OnAttackHit(target, attack);
        Enermy eCur = GetCurEnermy();
        eCur.RecoverHp(Mathf.FloorToInt(val * attack));
    }
}
