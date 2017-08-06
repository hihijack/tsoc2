using UnityEngine;
using System.Collections;

/// <summary>
/// 幽灵攻击：攻击燃烧能量
/// </summary>
/// 
public class MonYouLingGongJi : IMonSkill {

    int val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(25);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnAttackHit(IActor target, int attack)
    {
        base.OnAttackHit(target, attack);
        target.Prop.Vigor -= val;
    }
}
