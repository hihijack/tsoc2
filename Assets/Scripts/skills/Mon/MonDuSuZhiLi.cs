using UnityEngine;
using System.Collections;

/// <summary>
/// 毒素之力：收到毒素攻击提升攻击力
/// </summary>
public class MonDuSuZhiLi : IMonSkill
{

    int val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(23);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnHurt(IActor damager, DmgData dmgData)
    {
        base.OnHurt(damager, dmgData);
        if (dmgData.HasEleDmg(EDamageType.Poison))
        {
            _ECur.Prop.AtkParmaD += val;
        }
    }
}
