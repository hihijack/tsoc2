using UnityEngine;
using System.Collections;

/// <summary>
/// 冰冷强化，普通攻击附加冰冷伤害
/// </summary>
public class BingLengQiangHua : IMonSkill {

    int dam;

    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(3);
        this.dam = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnAttackHit(IActor target, int attack)
    {
        Enermy curE = GetCurEnermy();
        curE.DamageTarget(target, new DmgData(dam, EDamageType.Frozen));
    }
}
