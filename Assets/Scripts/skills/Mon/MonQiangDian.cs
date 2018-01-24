using UnityEngine;
using System.Collections;

public class MonQiangDian : IMonSkill{

    int val;
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(26);
        this.val = skillBD.GetIntVal(level, "val");
    }

    public override void OnAttackedHit(IActor atker, int attack)
    {
        base.OnAttackedHit(atker, attack);
        _ECur.DamageTarget(atker, new DmgData(val, EDamageType.Lighting));
    }
}
