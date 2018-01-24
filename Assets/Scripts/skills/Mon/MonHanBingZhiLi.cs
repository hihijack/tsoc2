using UnityEngine;
using System.Collections;
/// <summary>
/// 寒冰之力：受冰霜攻击提升攻击力
/// </summary>
public class MonHanBingZhiLi : IMonSkill
{
    int val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(22);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnHurt(IActor damager, DmgData dmgData)
    {
        base.OnHurt(damager, dmgData);
        if (dmgData.HasEleDmg(EDamageType.Frozen))
        {
            _ECur.Prop.AtkParmaD += val;
        }
    }
}
