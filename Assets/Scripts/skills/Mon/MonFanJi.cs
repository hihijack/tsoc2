using UnityEngine;
using System.Collections;

/// <summary>
/// 反击：闪避时对目标造成一次攻击
/// </summary>
public class MonFanJi : IMonSkill {
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(12);
    }

    public override void OnAtkedLost(IActor atker)
    {
        Enermy eCur = GetCurEnermy();
        int atk = eCur.GetAtk();
        eCur.DamageTarget(atk, atker, EDamageType.Phy, true);
    }
}
