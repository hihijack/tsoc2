using UnityEngine;
/// <summary>
/// 冰冷亲和：受到冰冷攻击恢复生命
/// </summary>
public class MonBingLengQinHe : IMonSkill {

    float percent;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(23);
        this.percent = skillBD.GetFloatVal(level, "val");
    }

    public override void OnHurt(IActor damager, DmgData dmgData)
    {
        base.OnHurt(damager, dmgData);
        if (dmgData.HasEleDmg(EDamageType.Frozen))
        {
            int hprecover = Mathf.FloorToInt(dmgData.dmgForzen * percent);
            _ECur.RecoverHp(hprecover);
        }
    }
}
