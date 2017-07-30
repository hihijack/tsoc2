using UnityEngine;
using System.Collections;
/// <summary>
/// 闪电之力：受闪电攻击提升攻击力
/// </summary>
public class MonShanDianZhiLi : IMonSkill
{
    int val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(21);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnHurt(IActor damager, int damage, EDamageType damageType, bool isDS)
    {
        base.OnHurt(damager, damage, damageType, isDS);
        if (damageType == EDamageType.Lighting)
        {
            _ECur._Prop.AtkParmaD += val;
        }
    }
}
