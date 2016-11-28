using UnityEngine;
using System.Collections;
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

    public override void OnHurt(IActor damager, int damage, EDamageType damageType, bool isDS)
    {
        base.OnHurt(damager, damage, damageType, isDS);
        if (damageType == EDamageType.Fire)
        {
            int hprecover = Mathf.FloorToInt(damage * percent);
            _ECur.RecoverHp(hprecover);
        }
    }
}
