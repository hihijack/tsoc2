using UnityEngine;
using System.Collections;

/// <summary>
/// 毒素强化。攻击附加毒素伤害
/// </summary>
public class DuSuQiangHua : IMonSkill {

    int dam;
    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(6);
        this.dam = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnAttackHit(IActor target, int attack)
    {
        Enermy curE = GetCurEnermy();
        curE.DamageTarget(target, new DmgData(dam, EDamageType.Poison));
    }
}
