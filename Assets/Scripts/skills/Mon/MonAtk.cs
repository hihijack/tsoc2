using UnityEngine;

/// <summary>
/// 轻攻击
/// </summary>
public class MonAtk : IMonSkill
{
    float percent;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(27);
        this.percent = skillBD.GetFloatVal(level, "val");
    }

    public override void StartEff(IActor target)
    {
        base.StartEff(target);
        int atk = Mathf.CeilToInt(_ECur._Prop.Atk * percent);
        _ECur.OnAttackHit(target, atk);
        target.OnAttackedHit(_ECur, atk);
        _ECur.DamageTarget(atk, target, EDamageType.Phy);
    }
}