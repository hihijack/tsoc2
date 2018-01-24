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

    public override void OnHurt(IActor damager, DmgData dmgData)
    {
        base.OnHurt(damager, dmgData);
        if (dmgData.HasEleDmg(EDamageType.Fire))
        {
            _ECur.Prop.AtkParmaD += val;
        }
    }
}
