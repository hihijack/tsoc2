using UnityEngine;
using System.Collections;

/// <summary>
/// 火焰强化，普通攻击附加火焰伤害
/// </summary>
public class HuoYanQiangHua : IMonSkill {

    int dam;

    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(4);
        this.dam = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnAttackHit(IActor target, int attack)
    {
        Enermy curE = GetCurEnermy();
        curE.DamageTarget(dam, target, EDamageType.Fire);
    }
}
