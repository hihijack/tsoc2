using UnityEngine;
using System.Collections;

/// <summary>
/// 尖刺外壳：每次受到攻击，反弹xx点伤害
/// </summary>
public class JianCiWaike : IMonSkill {

    int dam;

    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(7);
        this.dam = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnAttackedHit(IActor atker, int attack)
    {
        base.OnAttackedHit(atker, attack);
        Enermy eCur = GetCurEnermy();
        eCur.DamageTarget(atker, new DmgData(dam, EDamageType.Phy));
    }
}
