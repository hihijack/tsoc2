using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 不灭的怨恨：死亡时使友军提升攻击力
/// </summary>
public class BuMieYuanHen : IMonSkill {

    float val;
    public override void Init(int level)
    {
        base.Init(level);
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(9);
        val = skillBD.GetFloatVal(level, "val");
    }

    public override void OnDead()
    {
        base.OnDead();
        List<Enermy> allies = GetCurEnermy().GetAlliesInBattle();
        for (int i = 0; i < allies.Count; i++)
        {
            Enermy eItem = allies[i];
            eItem._Prop.AtkParmaC *= (1 + val);
            GameManager.commonCPU.CreateEffect("eff_monatk_up", eItem.transform.position, Color.white, -1);
        }
        
    }
}
