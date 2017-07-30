using UnityEngine;
using System.Collections;
/// <summary>
/// 火焰之力：受火焰攻击提升攻击力
/// </summary>
public class MonHuoYanZhiLi : IMonSkill
{

    int val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(20);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnHurt(IActor damager, int damage, EDamageType damageType, bool isDS)
    {
        base.OnHurt(damager, damage, damageType, isDS);
        if (damageType == EDamageType.Fire)
        {
            _ECur._Prop.AtkParmaD += val;
        }
    }
}
