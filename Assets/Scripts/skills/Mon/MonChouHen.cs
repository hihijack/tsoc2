using UnityEngine;
using System.Collections;

/// <summary>
/// 仇恨：每个死去的友军提升攻击力
/// </summary>
public class MonChouHen : IMonSkill {

    int val;
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(10);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnAllyDead(Enermy eDead)
    {
        base.OnAllyDead(eDead);
        Enermy eCur = GetCurEnermy();
        eCur._Prop.AtkParmaD += val;
        GameManager.commonCPU.CreateEffect("eff_monatk_up", eCur.GetPos(), Color.white, -1);
    }
}
