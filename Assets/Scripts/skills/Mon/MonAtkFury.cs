using UnityEngine;
using System.Collections;

public class MonAtkFury : IMonSkill
{
    float percent;
    float stiffDur;
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(32);
        this.percent = skillBD.GetFloatVal(level, "val");
        stiffDur = skillBD.GetFloatVal(level, "stiff");
    }

    public override void StartEff(IActor target)
    {
        base.StartEff(target);
        int atk = UnityEngine.Mathf.CeilToInt(_ECur.Prop.GetAtk(null) * percent);
        _ECur.OnAttackHit(target, atk);
        target.OnAttackedHit(_ECur, atk);
        _ECur.DamageTarget(target, new DmgData(atk, EDamageType.Phy));
    }

    public override void OnAtkEnd()
    {
        base.OnAtkEnd();
    }
}
