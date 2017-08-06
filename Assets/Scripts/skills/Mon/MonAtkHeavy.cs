public class MonAtkHeavy : IMonSkill
{
    float percent;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(28);
        this.percent = skillBD.GetFloatVal(level, "val");
    }

    public override void StartEff(IActor target)
    {
        base.StartEff(target);
        int atk = UnityEngine.Mathf.CeilToInt(_ECur.Prop.Atk * percent);
        _ECur.OnAttackHit(target, atk);
        target.OnAttackedHit(_ECur, atk);
        _ECur.DamageTarget(atk, target, EDamageType.Phy);
    }
}