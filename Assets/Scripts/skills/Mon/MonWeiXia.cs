using UnityEngine;
using System.Collections;

/// <summary>
/// 威吓：降低对手的攻击力及护甲
/// </summary>

public class MonWeiXia : IMonSkill {

    float val;

    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(18);
        this.val = skillBD.GetFloatVal(level, "val");
    }

    public override void OnEnterBattle()
    {
        base.OnEnterBattle();
        Buff_KongBuWeiXia buff = GameManager.hero.gameObject.AddComponent<Buff_KongBuWeiXia>();
        buff.Init(GameManager.hero, val);
        buff.StartEffect();
    }
}
